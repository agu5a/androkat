using System.Text.Json.Serialization;

namespace androkat.maui.library.Models.Responses;

public class VideoResponse
{
    [JsonPropertyName("cim")]
    public string Cim { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("img")]
    public string Img { get; set; } = string.Empty;

    [JsonPropertyName("videolink")]
    public string Videolink { get; set; } = string.Empty;

    [JsonPropertyName("forras")]
    public string Forras { get; set; } = string.Empty;

    [JsonPropertyName("channelId")]
    public string ChannelId { get; set; } = string.Empty;
}
