#nullable enable
using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using MonkeyCache.FileStore;
using System.Net.Http.Json;
using System.Text.Json;

namespace androkat.maui.library.Services;

public class PageService(IDownloadService downloadService, IRepository repository) : IPageService
{
    private HttpClient? client = null;
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3604:Member initializer values should not be redundant", Justification = "<Pending>")]
    private bool firstLoad = true;

    public async Task<int> UpsertGyonasiJegyzet(string notes)
    {
        return await repository.UpsertGyonasiJegyzet(notes);
    }

    public async Task<List<Bunok>> GetBunok()
    {
        return await repository.GetBunok();
    }
    public async Task<GyonasiJegyzet?> GetGyonasiJegyzet()
    {
        return await repository.GetGyonasiJegyzet();
    }

    public async Task<ContentEntity> GetContentEntityByIdAsync(Guid id)
    {
        var temp = await repository.GetContentById(id);
        return temp;
    }

    public async Task<ImadsagEntity> GetImadsagEntityByIdAsync(Guid id)
    {
        return await repository.GetImadsagEntityById(id);
    }

    public async Task<int> InsertFavoriteContentAsync(FavoriteContentEntity favoriteContentEntity)
    {
        var temp = await repository.InsertFavoriteContent(favoriteContentEntity);
        return temp;
    }

    public async Task<int> InsertBunok(Bunok entity)
    {
        return await repository.InsertBunok(entity);
    }

    public async Task<int> DownloadAll()
    {
        return await downloadService.DownloadAll();
    }

    public async Task<List<FavoriteContentEntity>> GetFavoriteContentsAsync()
    {
        return await repository.GetFavoriteContents();
    }

    public async Task<int> GetContentsCount()
    {
        return await repository.GetContentsCount();
    }

    public async Task<int> DeleteAllContentAndIma()
    {
        var res = await repository.DeleteAllContent();
        res += await repository.DeleteAllImadsag();
        return res;
    }

    public async Task<int> DeleteUserGyonas(bool jegyzet, bool bun)
    {
        return await repository.DeleteUserGyonas(jegyzet, bun);
    }

    public async Task<int> DeleteAllFavorite()
    {
        return await repository.DeleteAllFavorite();
    }

    public async Task<int> DeleteFavoriteContentByNid(Guid nid)
    {
        return await repository.DeleteFavoriteContentByNid(nid);
    }

    public async Task<int> DeleteBunokByIds(int bunId, int parancsId)
    {
        return await repository.DeleteBunokByIds(bunId, parancsId);
    }

    public async Task<int> SaveCustomPrayerAsync(ImadsagEntity customPrayer)
    {
        return await repository.InsertImadsag(customPrayer);
    }

    public async Task<List<ImadsagEntity>> GetImaContents(int offset, int pageSize, int? categoryId = null)
    {
        return await repository.GetImaContents(offset, pageSize, categoryId);
    }

    public async Task<int> GetFavoriteCountAsync()
    {
        return await repository.GetFavoriteCount();
    }

    public async Task<List<ContentEntity>> GetContentsAsync(string pageTypeId, bool returnVisited = true, List<string>? enabledSources = null)
    {
        return pageTypeId switch
        {
            "1" => await repository.GetContentsByTypeId(((int)Activities.ajanlatweb).ToString(), returnVisited, enabledSources),
            "2" => await repository.GetContentsByTypeId(((int)Activities.maiszent).ToString(), returnVisited, enabledSources),
            "3" => await repository.GetContentsByGroupName("group_szentek", returnVisited, enabledSources),
            "4" => await repository.GetContentsByGroupName("group_news", returnVisited, enabledSources),
            "5" => await repository.GetContentsByGroupName("group_blog", returnVisited, enabledSources),
            "6" => await repository.GetContentsByGroupName("group_humor", returnVisited, enabledSources),
            // "7" => Prayers handled by ImaListPage, not through ContentListPage
            "8" => await repository.GetContentsByGroupName("group_audio", returnVisited, enabledSources),
            "11" => await repository.GetContentsByTypeId(((int)Activities.book).ToString(), returnVisited, enabledSources),
            "34" => await repository.GetContentsByTypeId(((int)Activities.gyonas).ToString(), returnVisited, enabledSources),
            /*0*/
            _ => await repository.GetContentsByGroupName("group_napiolvaso", returnVisited, enabledSources),
        };
    }

    public int GetVersion()
    {
        return AppInfo.Version.Minor;
    }

#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable IDE0051 // Remove unused private members
    private Task<T?> TryGetAsync<T>(string path)
#pragma warning restore IDE0051 // Remove unused private members
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

    private async Task<T?> TryGetImplementationAsync<T>(string path)
    {
        var json = string.Empty;

#if !MACCATALYST
        if (Connectivity.NetworkAccess == NetworkAccess.None)
            json = Barrel.Current.Get<string>(path);
#endif
        if (!Barrel.Current.IsExpired(path))
            json = Barrel.Current.Get<string>(path);

        T? responseData = default;
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

            if (!Equals(responseData, default(T)))
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