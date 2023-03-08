using androkat.hu.Data;
using androkat.hu.Models;
using androkat.hu.Models.Responses;
using MonkeyCache.FileStore;
using System.Net.Http.Json;
using System.Text.Json;

namespace androkat.hu.Services;

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
}
