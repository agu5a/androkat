using SQLite;
using System.Runtime.Serialization;

namespace androkat.maui.library.Models.Entities;

[Table("Video_V1")]
public class VideoEntity
{
    [DataMember(Name = "nid")]
    public Guid Nid { get; set; }

    [DataMember(Name = "link")]
    public string Link { get; set; } = string.Empty;

    [DataMember(Name = "cim")]
    public string Cim { get; set; } = string.Empty;

    [DataMember(Name = "channelId")]
    public string ChannelId { get; set; } = string.Empty;

    [DataMember(Name = "date")]
    public DateTime Datum { get; set; }

    [DataMember(Name = "forras")]
    public string Forras { get; set; } = string.Empty;

    [DataMember(Name = "img")]
    public string Image { get; set; } = string.Empty;
}
