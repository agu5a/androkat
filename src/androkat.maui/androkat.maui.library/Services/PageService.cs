using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using MonkeyCache.FileStore;
using System.Net.Http.Json;
using System.Text.Json;

namespace androkat.maui.library.Services;

public class PageService : IPageService
{
    private HttpClient client;
    private bool firstLoad = true;
    private readonly IDownloadService _downloadService;
    private readonly IRepository _repository;

    public PageService(IDownloadService downloadService, IRepository repository)
    {
        _downloadService = downloadService;
        _repository = repository;
    }

    public async Task<ContentEntity> GetContentEntityByIdAsync(Guid id)
    {
        var temp = await _repository.GetContentById(id);
        return temp;
    }

    public async Task<ImadsagEntity> GetImadsagEntityByIdAsync(Guid id)
    {
        return await _repository.GetImadsagEntityById(id);
    }

    public async Task<int> InsertFavoriteContentAsync(FavoriteContentEntity favoriteContentEntity)
    {
        var temp = await _repository.InsertFavoriteContent(favoriteContentEntity);
        return temp;
    }

    public async Task<int> DownloadAll()
    {
        return await _downloadService.DownloadAll();
    }

    public async Task<List<FavoriteContentEntity>> GetFavoriteContentsAsync()
    {
        return await _repository.GetFavoriteContents();
    }

    public async Task<int> GetContentsCount()
    {
        return await _repository.GetContentsCount();
    }

    public async Task<int> DeleteAllContentAndIma()
    {
        var res = await _repository.DeleteAllContent();
        res += await _repository.DeleteAllImadsag();
        return res;
    }

    public async Task<int> DeleteAllFavorite()
    {
        return await _repository.DeleteAllFavorite();
    }

    public async Task<List<ImadsagEntity>> GetImaContents()
    {
        return await _repository.GetImaContents();
    }

    public async Task<int> GetFavoriteCountAsync()
    {
        return await _repository.GetFavoriteCount();
    }

    public async Task<List<ContentEntity>> GetContentsAsync(string pageTypeId)
    {
        return pageTypeId switch
        {
            "1" => await _repository.GetAjanlatokContents(),
            "2" => await _repository.GetMaiszentContents(),
            "3" => await _repository.GetSzentekContents(),
            "4" => await _repository.GetNewsContents(),
            "5" => await _repository.GetBlogContents(),
            "6" => await _repository.GetHumorContents(),
            //"7" => ima
            "8" => await _repository.GetAudioContents(),
            "11" => await _repository.GetBookContents(),
            _ => await _repository.GetContents(),
        };
    }

#pragma warning disable S1144 // Unused private types or members should be removed
    private Task<T> TryGetAsync<T>(string path)
#pragma warning restore S1144 // Unused private types or members should be removed
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
                var response = await GetClient().GetAsync(path);
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

    private HttpClient GetClient()
    {
        if (client != null)
            return client;

        client = new HttpClient { BaseAddress = new Uri(ConsValues.ApiUrl) };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        return client;
    }
}