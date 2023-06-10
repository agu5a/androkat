using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Models;
using MonkeyCache.FileStore;
using System.Net.Http.Json;
using System.Text.Json;

namespace androkat.maui.library.Services;

public class PageService
{
    private readonly HttpClient httpClient;
    private readonly IAndrokatService _androkatService;
    private readonly ISourceData _sourceData;
    private bool firstLoad = true;
    private readonly DownloadService _downloadService;
    private readonly IRepository _repository;

    public PageService(IAndrokatService androkatService, ISourceData sourceData, DownloadService downloadService, IRepository repository)
    {
        _androkatService = androkatService;
        _sourceData = sourceData;
        _downloadService = downloadService;
        _repository = repository;
    }

    public async Task<ContentDto> GetContentDtoByIdAsync(Guid id)
    {
        var temp = await _repository.GetElmelkedesContentById(id);
        return temp;
    }

    public async Task<int> InsertFavoriteContentAsync(FavoriteContentDto favoriteContentDto)
    {
        var temp = await _repository.InsertFavoriteContent(favoriteContentDto);
        return temp;
    }

    public async Task<int> DownloadAll()
    {
        return await _downloadService.DownloadAll();
    }

    public async Task<List<FavoriteContentDto>> GetFavoriteContentsAsync()
    {
        return await _repository.GetFavoriteContents();
    }

    public async Task<List<ImadsagDto>> GetImaContents()
    {
        return await _repository.GetImaContents();
    }

    public async Task<int> GetFavoriteCountAsync()
    {
        return await _repository.GetFavoriteCount();
    }

    public async Task<List<ContentDto>> GetContentsAsync(string pageTypeId)
    {
        return pageTypeId switch
        {
            "1" => await _repository.GetAjanlatokContents(),
            "2" => await _repository.GetMaiszentContents(),
            "3" => await _repository.GetSzentekContents(),
            "4" => await _repository.GetNewsContents(),
            "5" => await _repository.GetBlogContents(),
            "6" => await _repository.GetHumorContents(),
            //"7" => await _repository.GetHumorContents(), ima
            "8" => await _repository.GetHumorContents(),   //hanganyag         
            "11" => await _repository.GetBookContents(),
            _ => await _repository.GetElmelkedesContents(),
        };
    }


    private Task<T> TryGetAsync<T>(string path)
    {
        if (firstLoad)
        {
            firstLoad = false;

            // On first load, it takes a significant amount of time to initialize
            // For example, Connectivity.NetworkAccess, Barrel.Current.Get, and HttpClient all take time to initialize.
            //
            // Don't block the UI thread while doing this initialization, so the app starts faster.
            // Instead, run the first TryGet in a background thread to unblock the UI during startup.
            return Task.Run(() => TryGetImplementationAsync<T>(path));
        }

        return TryGetImplementationAsync<T>(path);
    }

    private async Task<T> TryGetImplementationAsync<T>(string path)
    {
        var json = string.Empty;

#if !MACCATALYST
        if (Connectivity.NetworkAccess == NetworkAccess.None)
            json = Barrel.Current.Get<string>(path);
#endif
        if (!Barrel.Current.IsExpired(path))
            json = Barrel.Current.Get<string>(path);

        T responseData = default;
        try
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                var response = await httpClient.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadFromJsonAsync<T>();
                }
            }
            else
            {
                responseData = JsonSerializer.Deserialize<T>(json);
            }

            if (responseData != null)
                Barrel.Current.Add(path, responseData, TimeSpan.FromMinutes(10));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }

        return responseData;
    }
}