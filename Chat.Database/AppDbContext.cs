using Chat.Database.Model;
using Chat.Database.TablesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserModel> UserModels { get; set; }
        
        public DbSet<CodeModel> CodeModels { get; set; }
        
        public DbSet<FriendModel> FriendModels { get; set; }
        
        public DbSet<ChatModel> ChatModels { get; set; }
        
        public DbSet<MessageModel> MessageModels { get; set; }
        
        public DbSet<UserChatModel> UserChatModels { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChatModelsConfiguration());
            modelBuilder.ApplyConfiguration(new CodeModelConfiguration());
            modelBuilder.ApplyConfiguration(new FriendModelConfiguration());
            modelBuilder.ApplyConfiguration(new MessageModelConfiguration());
            modelBuilder.ApplyConfiguration(new UserChatModelConfiguration());
            modelBuilder.ApplyConfiguration(new UserModelsConfiguration());
        }
    }
}