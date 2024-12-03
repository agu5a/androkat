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
    } = string.Empty;

    [JsonPropertyName("source")]
    public string Forrasszoveg
    {
        get; set;
    } = string.Empty;

    [JsonPropertyName("img")]
    public string Img
    {
        get; set;
    } = string.Empty;

    [JsonPropertyName("title")]
    public string Title
    {
        get; set;
    } = string.Empty;

    [JsonPropertyName("group")]
    public string GroupName
    {
        get; set;
    } = string.Empty;
}
