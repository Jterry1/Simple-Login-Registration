using System.ComponentModel.DataAnnotations;

namespace SimpleLogReg.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Must be a valid email.")]
        [EmailAddress]
        [Display(Name = "Email: ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must have at least 8 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }
    }
}