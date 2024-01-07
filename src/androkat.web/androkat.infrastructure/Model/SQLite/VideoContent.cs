using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace androkat.infrastructure.Model.SQLite;

[Table("video")]
public class VideoContent
{
    [Key]
    [Required]
    [DataMember(Name = "nid")]
    public Guid Nid { get; set; }

    [DataMember(Name = "img")]
    public string Img { get; set; } = string.Empty;

    [DataMember(Name = "videolink")]
    public string VideoLink { get; set; } = string.Empty;

    [DataMember(Name = "cim")]
    public string Cim { get; set; } = string.Empty;

    [DataMember(Name = "date")]
    public string Date { get; set; }  = string.Empty; //"yyyy-MM-dd"

    [DataMember(Name = "forras")]
    public string Forras { get; set; } = string.Empty;

    [DataMember(Name = "channelId")]
    public string ChannelId { get; set; } = string.Empty;

    [DataMember(Name = "inserted")]
    public DateTime Inserted { get; set; } //"yyyy-MM-dd HH:mm:ss"
}