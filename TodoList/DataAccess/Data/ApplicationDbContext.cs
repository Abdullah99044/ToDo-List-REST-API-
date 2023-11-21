using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TodoList.Data
{
    public class ApplicationDbContext : IdentityDbContext 
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {


            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server=LAPTOP-EL0218PF\\SQLEXPRESS;Database=ToDoApp;Trusted_Connection=True;TrustServerCertificate=true");
        }
    }
}
