using System;

namespace androkat.domain.Model;

public class VideoModel
{
	public Guid Nid { get; set; }
	public string Img { get; set; }
	public string VideoLink { get; set; }
	public string Cim { get; set; }
	public string Date { get; set; }
	public string Forras { get; set; }
	public string ChannelId { get; set; }
	public DateTime Inserted { get; set; }
}