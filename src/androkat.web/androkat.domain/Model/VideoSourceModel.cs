namespace androkat.domain.Model;

public class VideoSourceModel
{
	public VideoSourceModel(string channelId, string channelName)
	{
		ChannelId = channelId;
		ChannelName = channelName;
	}

	public string ChannelId { get; }
	public string ChannelName { get; }
}
