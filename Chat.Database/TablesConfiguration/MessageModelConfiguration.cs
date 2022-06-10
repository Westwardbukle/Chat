using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Database.TablesConfiguration
{
    public class MessageModelConfiguration : IEntityTypeConfiguration<MessageModel>
    {
        public void Configure(EntityTypeBuilder<MessageModel> builder)
        {
            builder.Property(p => p.DateCreated).HasColumnType("timestamp without time zone").IsRequired();
        }
    }
}