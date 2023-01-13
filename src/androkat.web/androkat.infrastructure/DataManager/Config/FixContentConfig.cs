using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace androkat.infrastructure.DataManager.Config;

[ExcludeFromCodeCoverage]
public class FixContentConfig : IEntityTypeConfiguration<FixContent>
{
    public void Configure(EntityTypeBuilder<FixContent> builder)
    {
        builder.ToTable("napifixolvaso");
        builder.HasKey(e => e.Nid);
        builder.Property(u => u.Cim).HasColumnType("text");
        builder.Property(u => u.Idezet).HasColumnType("longtext");
    }
}