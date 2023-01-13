using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace androkat.infrastructure.DataManager.Config;

[ExcludeFromCodeCoverage]
public class RadioMusorConfig : IEntityTypeConfiguration<RadioMusor>
{
    public void Configure(EntityTypeBuilder<RadioMusor> builder)
    {
        builder.ToTable("radio");
        builder.HasKey(u => u.Nid);
    }
}