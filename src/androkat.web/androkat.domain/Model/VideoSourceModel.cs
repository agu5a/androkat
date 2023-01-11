namespace androkat.domain.Model;

public class VideoSourceModel
{
	public VideoSourceModel(string channelId, string channelName)
	{
		ChannelId = channelId;
		ChannelName = channelName;
	}

	public string ChannelId { get; private set; }
	public string ChannelName { get; private set; }
}
