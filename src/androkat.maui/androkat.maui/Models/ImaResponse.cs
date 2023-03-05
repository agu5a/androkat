namespace androkat.hu.Models;
using System.Text.Json.Serialization;

public class ImaResponse
{
    [JsonPropertyName("hasMore")]
    public bool HasMore { get; set; }
    [JsonPropertyName("imak")]
    public List<ImadsagResponse> Imak { get; set; }
}
