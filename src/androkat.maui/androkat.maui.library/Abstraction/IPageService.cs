using androkat.maui.library.Models.Entities;

namespace androkat.maui.library.Abstraction;

public interface IPageService
{
    Task<int> DeleteAllContentAndIma();
    Task<int> DownloadAll();
    Task<ContentEntity> GetContentEntityByIdAsync(Guid id);
    Task<List<ContentEntity>> GetContentsAsync(string pageTypeId);
    Task<int> GetContentsCount();
    Task<List<FavoriteContentEntity>> GetFavoriteContentsAsync();
    Task<int> GetFavoriteCountAsync();
    Task<List<ImadsagEntity>> GetImaContents();
    Task<int> InsertFavoriteContentAsync(FavoriteContentEntity favoriteContentEntity);
}