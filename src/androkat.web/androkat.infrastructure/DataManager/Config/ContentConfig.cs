using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace androkat.infrastructure.DataManager.Config;

[ExcludeFromCodeCoverage]
public class ContentConfig : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> builder)
    {
        builder.ToTable("napiolvaso");
        builder.HasKey(e => e.Nid);
        builder.Property(u => u.Cim).HasColumnType("text");
        builder.Property(u => u.Idezet).HasColumnType("longtext");
        builder.Property(u => u.Inserted).HasColumnType("timestamp");
    }
}