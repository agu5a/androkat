using androkat.hu.Models;
using Org.Json;

namespace androkat.hu.Services;

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

    private Dictionary<int, SourceData> GetFromSources()
    {
        Dictionary<int, SourceData> sources = new Dictionary<int, SourceData>();

        try
        {
            using var stream = FileSystem.OpenAppPackageFileAsync("sources.json").Result;
            using var reader = new StreamReader(stream);
            var contents = reader.ReadToEnd();

            JSONObject obj = new JSONObject(contents);
            JSONArray jsonArray = obj.GetJSONArray("sources");

            for (int i = 0; i < jsonArray.Length(); i++)
            {
                SourceData data = new SourceData();
                JSONObject jsonObj = jsonArray.GetJSONObject(i);
                data.Title = jsonObj.GetString("title");
                data.Forras = jsonObj.GetString("sourcelink");
                data.Forrasszoveg = jsonObj.GetString("source");
                data.GroupName = jsonObj.GetString("group");
                //int img = mContext.Resources.GetIdentifier(jsonObj.GetString("img"), "drawable", mContext.PackageName);
                data.Img = jsonObj.GetString("img");
                sources.Add(jsonObj.GetInt("id"), data);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** GetFromSources EXCEPTION! {ex}");
        }

        return sources;
    }
}
