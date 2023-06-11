using androkat.maui.library.Models.Entities;

namespace androkat.maui.library.Data;

public interface IRepository
{
    void Init(bool tableCheck = false);

    Task<int> InsertContent(ContentEntity dto);
    Task<int> DeleteContentByNid(Guid nid);

    Task<ContentEntity> GetContentsByTypeName(string typeName);
    Task<ContentEntity> GetElmelkedesContentById(Guid id);
    Task<List<ContentEntity>> GetContentsWithoutBook();

    Task<List<ContentEntity>> GetAjanlatokContents();
    Task<List<ContentEntity>> GetAudioContents();
    Task<List<ContentEntity>> GetBookContents();
    Task<List<ContentEntity>> GetHumorContents();
    Task<List<ContentEntity>> GetMaiszentContents();
    Task<List<ContentEntity>> GetElmelkedesContents();
    Task<List<ContentEntity>> GetSzentekContents();
    Task<List<ContentEntity>> GetBlogContents();
    Task<List<ContentEntity>> GetNewsContents();

    Task<int> SetContentAsReadById(Guid nid);
    Task<List<FavoriteContentEntity>> GetFavoriteContents();
    Task<int> InsertFavoriteContent(FavoriteContentEntity dto);
    Task<int> GetFavoriteCount();
    Task<ImadsagEntity> GetFirstImadsag();
    Task<int> DeleteImadsagByNid(Guid nid);
    Task<int> InsertImadsag(ImadsagEntity dto);
    Task<List<ImadsagEntity>> GetImaContents();
}