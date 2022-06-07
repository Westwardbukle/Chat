using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserModel> UserModels { get; set; }
        
        public DbSet<CodeModel> CodeModels { get; set; }
        
        public DbSet<ChatModel> ChatModels { get; set; }
        
        public DbSet<MessageModel> MessageModels { get; set; }
        
        public DbSet<UserChatModel> UserChatModels { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
    }
}