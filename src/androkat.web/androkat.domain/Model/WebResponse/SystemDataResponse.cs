using System.Text.Json.Serialization;

namespace androkat.domain.Model.WebResponse;

public class SystemDataResponse
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}