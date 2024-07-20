using androkat.maui.library.Abstraction;

namespace androkat.maui.library.Services;

public class ResourceDataService : IResourceData
{
    public async Task<string> GetResourceAsString(string filename)
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
            using var reader = new StreamReader(stream);

            var contents = await reader.ReadToEndAsync();

            return contents;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** GetResourceAsString EXCEPTION! {ex}");
        }

        return string.Empty;
    }
}
