using System.Text.Json.Serialization;

namespace androkat.maui.library.Models.Responses;

public class ImadsagResponse
{

    [JsonPropertyName("nid")]
    public Guid Nid { get; set; }

    [JsonPropertyName("cim")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("leiras")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("time")]
    public DateTime RecordDate { get; set; }

    [JsonPropertyName("csoport")]
    public int Csoport { get; set; }
}
