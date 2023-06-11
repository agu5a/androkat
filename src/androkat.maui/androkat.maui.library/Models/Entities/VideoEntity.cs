using SQLite;
using System.Runtime.Serialization;

namespace androkat.maui.library.Models.Entities;

[Table("Video_V1")]
public class VideoEntity
{
    [DataMember(Name = "nid")]
    public Guid Nid { get; set; }

    [DataMember(Name = "link")]
    public string Link { get; set; }

    [DataMember(Name = "cim")]
    public string Cim { get; set; }

    [DataMember(Name = "channelId")]
    public string ChannelId { get; set; }

    [DataMember(Name = "date")]
    public DateTime Datum { get; set; }

    [DataMember(Name = "forras")]
    public string Forras { get; set; }

    [DataMember(Name = "img")]
    public string Image { get; set; }
}
