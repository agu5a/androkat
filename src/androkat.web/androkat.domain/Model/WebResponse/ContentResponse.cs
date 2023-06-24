using System;
using System.Text.Json.Serialization;

namespace androkat.domain.Model.WebResponse;

public class ContentResponse
{
    [JsonPropertyName("nid")]
    public Guid Nid { get; set; }

    [JsonPropertyName("datum")]
    public string Datum { get; set; } //"yyyy-MM-dd HH:mm:ss"

    [JsonPropertyName("cim")]
    public string Cim { get; set; }

    [JsonPropertyName("idezet")]
    public string Idezet { get; set; }

    [JsonPropertyName("img")]
    public string Img { get; set; }

    [JsonPropertyName("forras")]
    public string Forras { get; set; }

    [JsonPropertyName("kulsolink")]
    public string KulsoLink { get; set; }
}