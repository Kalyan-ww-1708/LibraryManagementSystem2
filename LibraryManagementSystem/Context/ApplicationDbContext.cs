using Microsoft.EntityFrameworkCore;

using LibraryManagementSystem.Model;
namespace LibraryManagementSystem.Context

{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories{ get; set; }

    }
}
