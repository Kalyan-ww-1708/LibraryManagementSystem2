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

        public DbSet<User> Users { get; set; }

        public DbSet<Borrow> Borrows { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
