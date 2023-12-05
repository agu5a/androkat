using System;
using System.Text.Json.Serialization;

namespace androkat.domain.Model.WebResponse;

public class EgyebOlvasnivaloResponse
{
    [JsonPropertyName("nid")]
    public Guid Nid { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; } //"yyyy-MM-dd HH:mm:ss"

    [JsonPropertyName("cim")]
    public string Cim { get; set; }

    [JsonPropertyName("leiras")]
    public string Leiras { get; set; }

    [JsonPropertyName("kulsolink")]
    public string KulsoLink { get; set; }
}