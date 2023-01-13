using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace androkat.infrastructure.DataManager.Config;

[ExcludeFromCodeCoverage]
public class SystemInfoConfig : IEntityTypeConfiguration<SystemInfo>
{
    public void Configure(EntityTypeBuilder<SystemInfo> builder)
    {
        builder.ToTable("systeminfo");
        builder.HasKey(u => u.Id);
    }
}