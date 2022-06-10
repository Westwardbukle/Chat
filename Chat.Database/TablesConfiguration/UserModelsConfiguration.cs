using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Database.TablesConfiguration
{
    public class UserModelsConfiguration: IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.Property(p => p.DateCreated).HasColumnType("timestamp without time zone").IsRequired();
        }
    }
}