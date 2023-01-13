using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace androkat.infrastructure.DataManager.Config;

[ExcludeFromCodeCoverage]
public class TempContentConfig : IEntityTypeConfiguration<TempContent>
{
    public void Configure(EntityTypeBuilder<TempContent> builder)
    {
        builder.ToTable("napiolvaso2");
        builder.HasKey(u => u.Nid);
        builder.Property(u => u.Cim).HasColumnType("text");
        builder.Property(u => u.Idezet).HasColumnType("longtext");
        builder.Property(u => u.Inserted).HasColumnType("timestamp");
    }
}