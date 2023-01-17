using System.Text.Json.Serialization;

namespace androkat.domain.Model.WebResponse;

public class VideoResponse
{
    [JsonPropertyName("cim")]
    public string Cim { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("img")]
    public string Img { get; set; }

    [JsonPropertyName("videolink")]
    public string VideoLink { get; set; }

    [JsonPropertyName("forras")]
    public string Forras { get; set; }

    [JsonPropertyName("channelId")]
    public string ChannelId { get; set; }
}