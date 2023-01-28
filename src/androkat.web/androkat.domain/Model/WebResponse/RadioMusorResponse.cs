using System.Text.Json.Serialization;

namespace androkat.domain.Model.WebResponse;

public class RadioMusorResponse
{
    [JsonPropertyName("musor")]
    public string Musor { get; set; }
}