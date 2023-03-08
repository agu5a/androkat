using androkat.hu.Models;

namespace androkat.hu.Data;

public interface IRepository
{
    void Init(bool tableCheck = false);

    Task<int> InsertContent(ContentDto dto);
    Task<int> DeleteContentByNid(Guid nid);

    Task<ContentDto> GetContentsByTypeName(string typeName);
    Task<ContentDto> GetElmelkedesContentById(Guid id);
    Task<List<ContentDto>> GetContentsWithoutBook();

    Task<List<ContentDto>> GetAjanlatokContents();
    Task<List<ContentDto>> GetAudioContents();
    Task<List<ContentDto>> GetBookContents();
    Task<List<ContentDto>> GetHumorContents();
    Task<List<ContentDto>> GetMaiszentContents();
    Task<List<ContentDto>> GetElmelkedesContents();    
    Task<List<ContentDto>> GetSzentekContents();
    Task<List<ContentDto>> GetBlogContents();
    Task<List<ContentDto>> GetNewsContents();
    
    Task<int> SetContentAsReadById(Guid nid);
    Task<List<FavoriteContentDto>> GetFavoriteContents();
    Task<int> InsertFavoriteContent(FavoriteContentDto dto);
    Task<int> GetFavoriteCount();
    Task<ImadsagDto> GetFirstImadsag();
    Task<int> DeleteImadsagByNid(Guid nid);
    Task<int> InsertImadsag(ImadsagDto dto);
    Task<List<ImadsagDto>> GetImaContents();
}