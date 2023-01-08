using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace androkat.infrastructure.Model.SQLite;

[Table("video")]
public class Video
{
	[Key]
	[Required]
	[DataMember(Name = "nid")]
	public Guid Nid { get; set; }

	[DataMember(Name = "img")]
	public string Img { get; set; }

	[DataMember(Name = "videolink")]
	public string VideoLink { get; set; }

	[DataMember(Name = "cim")]
	public string Cim { get; set; }

	[DataMember(Name = "date")]
	public string Date { get; set; } //"yyyy-MM-dd"

	[DataMember(Name = "forras")]
	public string Forras { get; set; }

	[DataMember(Name = "channelId")]
	public string ChannelId { get; set; }

	[DataMember(Name = "inserted")]
	public DateTime Inserted { get; set; } //"yyyy-MM-dd HH:mm:ss"
}