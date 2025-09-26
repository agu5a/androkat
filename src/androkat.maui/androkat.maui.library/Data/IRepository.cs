#nullable enable
using androkat.maui.library.Models.Entities;

namespace androkat.maui.library.Data;

public interface IRepository
{
    void Init(bool tableCheck = false);

    Task<(int, string?)> InsertContent(ContentEntity entity);
    Task<int> DeleteContentByNid(Guid nid);

    Task<ContentEntity?> GetContentsByTypeName(string typeName);
    Task<ContentEntity> GetContentById(Guid id);
    Task<List<ContentEntity>> GetContentsWithoutBook();

    Task<List<ContentEntity>> GetContentsByTypeId(string typeId, bool returnVisited = true, List<string>? enabledSources = null);
    Task<List<ContentEntity>> GetContentsByGroupName(string groupName, bool returnVisited = true, List<string>? enabledSources = null);

    Task<int> SetContentAsReadById(Guid nid);
    Task<List<FavoriteContentEntity>> GetFavoriteContents();
    Task<int> InsertFavoriteContent(FavoriteContentEntity entity);
    Task<int> DeleteFavoriteContentByNid(Guid nid);
    Task<int> GetFavoriteCount();
    Task<ImadsagEntity?> GetFirstImadsag();
    Task<int> DeleteImadsagByNid(Guid nid);
    Task<int> InsertImadsag(ImadsagEntity entity);
    Task<List<ImadsagEntity>> GetImaContents(int pageNumber, int pageSize, int? categoryId = null);
    Task<int> GetContentsCount();
    Task<int> DeleteAllContent();
    Task<int> DeleteAllImadsag();
    Task<int> DeleteAllFavorite();
    Task<ImadsagEntity> GetImadsagEntityById(Guid id);
    Task<int> DeleteUserGyonas(bool jegyzet, bool bun);
    Task<GyonasiJegyzet?> GetGyonasiJegyzet();
    Task<int> UpsertGyonasiJegyzet(string notes);
    Task<int> InsertBunok(Bunok entity);
    Task<int> DeleteBunokByIds(int bunId, int parancsId);
    Task<List<Bunok>> GetBunok();
}