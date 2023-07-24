using System;
using System.Text.Json.Serialization;

namespace androkat.domain.Model.WebResponse;

public class ImaDetailsResponse
{
    [JsonPropertyName("nid")]
    public Guid Nid { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; } //"yyyy-MM-dd HH:mm:ss"

    [JsonPropertyName("cim")]
    public string Cim { get; set; }

    [JsonPropertyName("leiras")]
    public string Leiras { get; set; }

    [JsonPropertyName("csoport")]
    public int Csoport { get; set; }
}