using System.Text.Json.Serialization;

namespace androkat.maui.library.Models;

public class SourceData
{
    [JsonPropertyName("id")]
    public int Id
    {
        get; set;
    }

    [JsonPropertyName("sourcelink")]
    public string Forras
    {
        get; set;
    }

    [JsonPropertyName("source")]
    public string Forrasszoveg
    {
        get; set;
    }

    [JsonPropertyName("img")]
    public string Img
    {
        get; set;
    }

    [JsonPropertyName("title")]
    public string Title
    {
        get; set;
    }

    [JsonPropertyName("group")]
    public string GroupName
    {
        get; set;
    }
}
