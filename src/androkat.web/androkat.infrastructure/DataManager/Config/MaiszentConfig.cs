using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace androkat.infrastructure.DataManager.Config;

[ExcludeFromCodeCoverage]
public class MaiszentConfig : IEntityTypeConfiguration<Maiszent>
{
    public void Configure(EntityTypeBuilder<Maiszent> builder)
    {
        builder.ToTable("maiszent");
        builder.HasKey(e => e.Nid);
        builder.Property(u => u.Cim).HasColumnType("text");
        builder.Property(u => u.Idezet).HasColumnType("longtext");
        builder.Property(u => u.Inserted).HasColumnType("timestamp");
    }
}