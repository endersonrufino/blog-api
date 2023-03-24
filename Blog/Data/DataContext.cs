using blog.Data.Mappings;
using blog.Models;
using Microsoft.EntityFrameworkCore;

namespace blog.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => base.OnConfiguring(optionsBuilder.UseSqlServer("Data Source=DESKTOP-FVOS0IE\\SQLEXPRESS;Initial Catalog=BlogApi;Integrated Security=True;TrustServerCertificate=True"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new PostMap());
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}