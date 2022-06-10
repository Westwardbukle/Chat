using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Database.TablesConfiguration
{
    public class UserChatModelConfiguration : IEntityTypeConfiguration<UserChatModel>
    {
        public void Configure(EntityTypeBuilder<UserChatModel> builder)
        {
            builder.Property(p => p.DateCreated).HasColumnType("timestamp without time zone").IsRequired();
        }
    }
}