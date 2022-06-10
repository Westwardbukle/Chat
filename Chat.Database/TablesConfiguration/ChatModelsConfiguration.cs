using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Database.TablesConfiguration
{
    public class ChatModelsConfiguration : IEntityTypeConfiguration<ChatModel>
    {
        public void Configure(EntityTypeBuilder<ChatModel> builder)
        {
            builder.Property(p => p.DateCreated).HasColumnType("timestamp without time zone").IsRequired();
            /*builder.Property(p => p.Name).HasMaxLength(30);
            builder.Property(p => p.Type).IsRequired();
            builder.Property(p => p.MessageModels).IsRequired();
            builder.Property(p => p.UserChats).IsRequired();
            builder.Property(p => p.Id).IsRequired();*/
        }
    }
}