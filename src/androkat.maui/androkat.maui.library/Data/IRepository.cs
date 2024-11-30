using androkat.maui.library.Models.Entities;

namespace androkat.maui.library.Data;

public interface IRepository
{
    void Init(bool tableCheck = false);

    Task<int> InsertContent(ContentEntity entity);
    Task<int> DeleteContentByNid(Guid nid);

    Task<ContentEntity> GetContentsByTypeName(string typeName);
    Task<ContentEntity> GetContentById(Guid id);
    Task<List<ContentEntity>> GetContentsWithoutBook();

    Task<List<ContentEntity>> GetAjanlatokContents();
    Task<List<ContentEntity>> GetAudioContents();
    Task<List<ContentEntity>> GetBookContents();
    Task<List<ContentEntity>> GetHumorContents();
    Task<List<ContentEntity>> GetMaiszentContents();
    Task<List<ContentEntity>> GetContents();
    Task<List<ContentEntity>> GetSzentekContents();
    Task<List<ContentEntity>> GetBlogContents();
    Task<List<ContentEntity>> GetNewsContents();

    Task<int> SetContentAsReadById(Guid nid);
    Task<List<FavoriteContentEntity>> GetFavoriteContents();
    Task<int> InsertFavoriteContent(FavoriteContentEntity entity);
    Task<int> GetFavoriteCount();
    Task<ImadsagEntity> GetFirstImadsag();
    Task<int> DeleteImadsagByNid(Guid nid);
    Task<int> InsertImadsag(ImadsagEntity entity);
    Task<List<ImadsagEntity>> GetImaContents(int pageNumber, int pageSize);
    Task<int> GetContentsCount();
    Task<int> DeleteAllContent();
    Task<int> DeleteAllImadsag();
    Task<int> DeleteAllFavorite();
    Task<ImadsagEntity> GetImadsagEntityById(Guid id);
    Task<int> DeleteUserGyonas(bool jegyzet, bool bun);
}