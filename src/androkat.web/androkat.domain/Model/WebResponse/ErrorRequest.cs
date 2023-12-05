using System.Text.Json.Serialization;

namespace androkat.domain.Model.WebResponse;

public class ErrorRequest
{
    [JsonPropertyName("error")]
    public string Error { get; set; }
}