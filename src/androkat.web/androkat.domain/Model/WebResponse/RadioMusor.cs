using System.Text.Json.Serialization;

namespace androkat.domain.Model.WebResponse;

public class RadioMusor
{
    [JsonPropertyName("musor")]
    public string Musor { get; set; }
}