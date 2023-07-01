using System.Text.Json.Serialization;

namespace androkat.maui.library.Models.Responses;

public class ServerInfoResponse
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}
