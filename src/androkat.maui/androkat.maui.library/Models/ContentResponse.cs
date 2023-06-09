using System.Text.Json.Serialization;

namespace androkat.maui.library.Models;

public class ContentResponse
{
    [JsonPropertyName("nid")]
    public Guid Nid { get; set; }

    [JsonPropertyName("datum")]
    public DateTime Datum { get; set; }

    [JsonPropertyName("cim")]
    public string Cim { get; set; }

    [JsonPropertyName("idezet")]
    public string Idezet { get; set; }

    [JsonPropertyName("img")]
    public string Image { get; set; }

    [JsonPropertyName("forras")]
    public string Forras { get; set; }

    [JsonPropertyName("kulsolink")]
    public string KulsoLink { get; set; }
}