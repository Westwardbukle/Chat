using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Database.TablesConfiguration
{
    public class FriendModelConfiguration : IEntityTypeConfiguration<FriendModel>
    {
        public void Configure(EntityTypeBuilder<FriendModel> builder)
        {
            builder.Property(p => p.DateCreated).HasColumnType("timestamp without time zone").IsRequired();
        }
    }
}