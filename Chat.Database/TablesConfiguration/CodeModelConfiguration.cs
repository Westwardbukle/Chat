using Chat.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Database.TablesConfiguration
{
    public class CodeModelConfiguration: IEntityTypeConfiguration<CodeModel>
    {
        public void Configure(EntityTypeBuilder<CodeModel> builder)
        {
            builder.Property(p => p.DateCreated).HasColumnType("timestamp without time zone").IsRequired();
        }
    }
}