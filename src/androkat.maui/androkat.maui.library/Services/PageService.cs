using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Responses;
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
        //var showResponse = await TryGetAsync<ShowResponse>($"shows/{id}");

        //return showResponse == null
        //    ? null
        //    : GetShow(showResponse);
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

    public async Task<IEnumerable<Show>> GetShowsByCategoryAsync(Guid idCategory)
    {
        return GetShows();
        /*var result = new List<Show>();
        var showsResponse = await TryGetAsync<IEnumerable<ShowResponse>>($"shows?limit=20&categoryId={idCategory}");

        if (showsResponse == null)
            return result;
        else
        {
            foreach(var show in showsResponse)
            {
                result.Add(GetShow(show));
            }

            return result;
        }*/
    }

    public async Task<IEnumerable<Show>> SearchShowsAsync(Guid idCategory, string term)
    {
        return new List<Show>();
        //var showsResponse = await TryGetAsync<IEnumerable<ShowResponse>>($"shows?limit=20&categoryId={idCategory}&term={term}");

        //return showsResponse?.Select(response => GetShow(response));
    }

    public async Task<IEnumerable<Show>> SearchShowsAsync(string term)
    {
        return GetShows();
        //var showsResponse = await TryGetAsync<IEnumerable<ShowResponse>>($"shows?limit=20&term={term}");

        //return showsResponse?.Select(response => GetShow(response));
    }

    private Show GetShow(ShowResponse response)
    {
        return new Show(response/*, listenLaterService*/);
    }

    private Task<T> TryGetAsync<T>(string path)
    {
        if (firstLoad)
        {
            firstLoad = false;

            // On first load, it takes a significant amount of time to initialize
            // the ShowsService. For example, Connectivity.NetworkAccess, Barrel.Current.Get,
            // and HttpClient all take time to initialize.
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

    private static List<Show> GetShows()
    {
        return new List<Show>
        {
            new Show{ Author ="Author", Description = "Description", Id = Guid.NewGuid(), Title = "Title", IsFeatured = true, Image = new Uri("https://d3wo5wojvuv7l.cloudfront.net/t_rss_itunes_square_1400/images.spreaker.com/original/d609b60f7cc16bfd0e6416ce8d5596ec.jpg"),
            Episodes = new List<Episode>
            {
                new Episode { Id = Guid.NewGuid(), Description = "Description", Title = "Title", Duration = "1", Published = DateTime.Now, Url = new Uri("http://aa.hu")}
            }},
            new Show{ Author ="Author2", Description = "Description2", Id = Guid.NewGuid(), Title = "Title2", IsFeatured = true, Image = new Uri("https://d3wo5wojvuv7l.cloudfront.net/t_rss_itunes_square_1400/images.spreaker.com/original/d609b60f7cc16bfd0e6416ce8d5596ec.jpg"),
            Episodes = new List<Episode>
            {
                new Episode { Id = Guid.NewGuid(), Description = "Description", Title = "Title", Duration = "1", Published = DateTime.Now, Url = new Uri("http://aa.hu")}
            }},
            new Show{ Author ="Author3", Description = "Description3", Id = Guid.NewGuid(), Title = "Title3", IsFeatured = false, Image = new Uri("https://d3wo5wojvuv7l.cloudfront.net/t_rss_itunes_square_1400/images.spreaker.com/original/d609b60f7cc16bfd0e6416ce8d5596ec.jpg"),
            Episodes = new List<Episode>
            {
                new Episode { Id = Guid.NewGuid(), Description = "Description", Title = "Title", Duration = "1", Published = DateTime.Now, Url = new Uri("http://aa.hu")}
            }},
            new Show{ Author ="Author4", Description = "Description4", Id = Guid.NewGuid(), Title = "Title4", IsFeatured = true, Image = new Uri("https://d3wo5wojvuv7l.cloudfront.net/t_rss_itunes_square_1400/images.spreaker.com/original/d609b60f7cc16bfd0e6416ce8d5596ec.jpg"),
            Episodes = new List<Episode>
            {
                new Episode { Id = Guid.NewGuid(), Description = "Description", Title = "Title", Duration = "1", Published = DateTime.Now, Url = new Uri("http://aa.hu")}
            }},
            new Show{ Author ="Author5", Description = "Description5", Id = Guid.NewGuid(), Title = "Title5", IsFeatured = false, Image = new Uri("https://d3wo5wojvuv7l.cloudfront.net/t_rss_itunes_square_1400/images.spreaker.com/original/d609b60f7cc16bfd0e6416ce8d5596ec.jpg"),
            Episodes = new List<Episode>
            {
                new Episode { Id = Guid.NewGuid(), Description = "Description", Title = "Title", Duration = "1", Published = DateTime.Now, Url = new Uri("http://aa.hu")}
            }},
            new Show{ Author ="Author6", Description = "Description6", Id = Guid.NewGuid(), Title = "Title6", IsFeatured = true, Image = new Uri("https://d3wo5wojvuv7l.cloudfront.net/t_rss_itunes_square_1400/images.spreaker.com/original/d609b60f7cc16bfd0e6416ce8d5596ec.jpg"),
            Episodes = new List<Episode>
            {
                new Episode { Id = Guid.NewGuid(), Description = "Description", Title = "Title", Duration = "1", Published = DateTime.Now, Url = new Uri("http://aa.hu")}
            }}
        };
    }
}
