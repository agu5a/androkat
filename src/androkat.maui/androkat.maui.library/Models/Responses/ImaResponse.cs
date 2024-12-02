using System.Text.Json.Serialization;

namespace androkat.maui.library.Models.Responses;

public class ImaResponse
{
    [JsonPropertyName("hasMore")]
    public bool HasMore { get; set; }

    [JsonPropertyName("imak")]
    public List<ImadsagResponse> Imak { get; set; } = new List<ImadsagResponse>();
}
