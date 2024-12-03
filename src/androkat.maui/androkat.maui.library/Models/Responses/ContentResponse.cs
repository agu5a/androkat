using System.Text.Json.Serialization;

namespace androkat.maui.library.Models.Responses;

public class ContentResponse
{
    [JsonPropertyName("nid")]
    public Guid Nid { get; set; }

    [JsonPropertyName("datum")]
    public DateTime Datum { get; set; }

    [JsonPropertyName("cim")]
    public string Cim { get; set; } = string.Empty;

    [JsonPropertyName("idezet")]
    public string Idezet { get; set; } = string.Empty;

    [JsonPropertyName("img")]
    public string Image { get; set; } = string.Empty;

    [JsonPropertyName("forras")]
    public string Forras { get; set; } = string.Empty;

    [JsonPropertyName("kulsolink")]
    public string KulsoLink { get; set; } = string.Empty;
}