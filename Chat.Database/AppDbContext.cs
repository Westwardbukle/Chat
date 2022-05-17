using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserModel> UserModels { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}