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

	public Guid Nid { get; private set; }
    public string Img { get; private set; }
    public string VideoLink { get; private set; }
    public string Cim { get; private set; }
    public string Date { get; private set; }
    public string Forras { get; private set; }
    public string ChannelId { get; private set; }
    public DateTime Inserted { get; private set; }
}