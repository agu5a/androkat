using androkat.infrastructure.Model.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace androkat.infrastructure.DataManager.Config;

[ExcludeFromCodeCoverage]
public class VideoContentConfig : IEntityTypeConfiguration<VideoContent>
{
    public void Configure(EntityTypeBuilder<VideoContent> builder)
    {
        builder.ToTable("video");
        builder.HasKey(u => u.Nid);
    }
}