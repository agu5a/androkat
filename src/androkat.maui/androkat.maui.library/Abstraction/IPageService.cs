using androkat.maui.library.Models.Entities;

namespace androkat.maui.library.Abstraction;

public interface IPageService
{
    Task<int> DownloadAll();
    Task<ContentEntity> GetContentDtoByIdAsync(Guid id);
    Task<List<ContentEntity>> GetContentsAsync(string pageTypeId);
    Task<List<FavoriteContentEntity>> GetFavoriteContentsAsync();
    Task<int> GetFavoriteCountAsync();
    Task<List<ImadsagEntity>> GetImaContents();
    Task<int> InsertFavoriteContentAsync(FavoriteContentEntity favoriteContentDto);
}