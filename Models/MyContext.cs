using Microsoft.EntityFrameworkCore;
namespace SimpleLogReg.Models
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions options) : base(options){}
        public DbSet<User> Users {get;set;}
    }
}