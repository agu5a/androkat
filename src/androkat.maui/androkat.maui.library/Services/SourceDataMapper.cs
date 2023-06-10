using androkat.maui.library.Abstraction;
using androkat.maui.library.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace androkat.maui.library.Services;

public class SourcesImport
{
    [JsonPropertyName("sources")]
    public List<SourceData> Sources { get; set; }
}

public class SourceDataMapper : ISourceData
{
    private readonly Dictionary<int, SourceData> _sources = new();

    public SourceDataMapper()
    {
        _sources = GetFromSources();
    }

    public SourceData GetSourcesFromMemory(int index)
    {
        return _sources[index];
    }

    private static Dictionary<int, SourceData> GetFromSources()
    {
        try
        {
            using var stream = FileSystem.OpenAppPackageFileAsync("sources.json").Result;
            using var reader = new StreamReader(stream);

            var data = new SourceData();
            var contents = reader.ReadToEnd();

            var result = JsonSerializer.Deserialize<SourcesImport>(contents);
            return result.Sources.ToDictionary(d => d.Id);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** GetFromSources EXCEPTION! {ex}");
        }

        return new Dictionary<int, SourceData>();
    }
}
