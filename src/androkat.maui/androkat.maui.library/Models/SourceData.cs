using System.Text.Json.Serialization;

namespace androkat.maui.library.Models;

public class SourceData
{
    [JsonPropertyName("TipusId")]
    public int Id
    {
        get; set;
    }

    [JsonPropertyName("Link")]
    public string Forras
    {
        get; set;
    } = string.Empty;

    [JsonPropertyName("Forras")]
    public string Forrasszoveg
    {
        get; set;
    } = string.Empty;

    [JsonPropertyName("Image")]
    public string Img
    {
        get; set;
    } = string.Empty;

    [JsonPropertyName("TipusNev")]
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
