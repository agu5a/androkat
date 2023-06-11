using System.Text.Json.Serialization;

namespace androkat.maui.library.Models;

public class SourcesImport
{
    [JsonPropertyName("sources")]
    public List<SourceData> Sources { get; set; }
}
