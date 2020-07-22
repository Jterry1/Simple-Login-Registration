using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleLogReg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace SimpleLogReg.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetString("FirstName", newUser.FirstName);
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Login");

            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost("login")]
        public IActionResult Login(Login userSubmission)
        {
            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher<Login>();
                var signedInUser = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
                if (signedInUser == null)
                {
                    ViewBag.Message = "Email/Password is invalid";
                    return View("Login");
                }
                else
                {
                    var result = hasher.VerifyHashedPassword(userSubmission, signedInUser.Password, userSubmission.Password);
                    if (result == 0)
                    {
                        ViewBag.Message = "Email/Password is invalid";
                        return View("Login");
                    }
                }

                HttpContext.Session.SetInt32("UserId", signedInUser.UserId);
                return RedirectToAction("Success");
            }
            else
            {
                return View("Login");
            }
        }
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpGet("success")]
        public IActionResult Success()
        {
            User userInDb = dbContext.Users.FirstOrDefault(u => u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            if(userInDb==null)
            {
                return RedirectToAction("Logout");
            }
            else
            {
                return View("Success");
            }
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}
