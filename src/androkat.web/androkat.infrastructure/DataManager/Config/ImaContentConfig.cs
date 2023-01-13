using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace androkat.infrastructure.DataManager.Config;

[ExcludeFromCodeCoverage]
public class ImaContentConfig : IEntityTypeConfiguration<ImaContent>
{
    public void Configure(EntityTypeBuilder<ImaContent> builder)
    {
        builder.ToTable("ima");
        builder.HasKey(u => u.Nid);
    }
}