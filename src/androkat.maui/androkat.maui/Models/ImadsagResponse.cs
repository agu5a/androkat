using System.Text.Json.Serialization;

namespace androkat.hu.Models;

public class ImadsagResponse
{

    [JsonPropertyName("nid")]
    public Guid Nid { get; set; }

    [JsonPropertyName("cim")]
    public string Title { get; set; }

    [JsonPropertyName("leiras")]
    public string Content { get; set; }

    [JsonPropertyName("time")]
    public DateTime RecordDate { get; set; }

    [JsonPropertyName("csoport")]
    public int Csoport { get; set; }
}
