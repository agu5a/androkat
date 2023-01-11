using System;

namespace androkat.domain.Model;

public class VideoModel
{
	public VideoModel(Guid nid, string img, string videoLink, string cim, string date, string forras, string channelId, DateTime inserted)
	{
		Nid = nid;
		Img = img;
		VideoLink = videoLink;
		Cim = cim;
		Date = date;
		Forras = forras;
		ChannelId = channelId;
		Inserted = inserted;
	}

	public Guid Nid { get; }
    public string Img { get; }
    public string VideoLink { get; }
    public string Cim { get; }
    public string Date { get; }
    public string Forras { get; }
    public string ChannelId { get; }
    public DateTime Inserted { get; }
}