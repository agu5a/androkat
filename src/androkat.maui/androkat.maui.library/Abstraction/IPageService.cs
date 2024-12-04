#nullable enable
using androkat.maui.library.Models.Entities;

namespace androkat.maui.library.Abstraction;

public interface IPageService
{
    Task<int> DeleteAllContentAndIma();
    Task<int> DeleteAllFavorite();
    Task<int> DeleteUserGyonas(bool jegyzet, bool bun);
    Task<int> DownloadAll();
    Task<ContentEntity> GetContentEntityByIdAsync(Guid id);
    Task<List<ContentEntity>> GetContentsAsync(string pageTypeId);
    Task<int> GetContentsCount();
    Task<List<FavoriteContentEntity>> GetFavoriteContentsAsync();
    Task<int> GetFavoriteCountAsync();
    Task<List<ImadsagEntity>> GetImaContents(int pageNumber, int pageSize);
    Task<ImadsagEntity> GetImadsagEntityByIdAsync(Guid id);
    int GetVersion();
    Task<GyonasiJegyzet?> GetGyonasiJegyzet();
    Task<int> InsertFavoriteContentAsync(FavoriteContentEntity favoriteContentEntity);
    Task<int> UpsertGyonasiJegyzet(string notes);
    Task<int> InsertBunok(Bunok entity);
    Task<int> DeleteBunokByIds(int bunId, int parancsId);
    Task<List<Bunok>> GetBunok();
}