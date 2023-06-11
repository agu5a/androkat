using androkat.maui.library.Data;
using androkat.maui.library.Models;
using SQLite;

namespace androkat.hu.Data;

public class Repository : IRepository
{
    private readonly string _dbPath;
    private SQLiteAsyncConnection conn;

    public Repository(string dbPath)
    {
        _dbPath = dbPath;
        Init(true);
    }

    public void Init(bool tableCheck = false)
    {
        if (conn != null)
            return;

        conn = new SQLiteAsyncConnection(_dbPath);

        if (tableCheck)
        {
            conn.CreateTableAsync<ContentDto>();
            conn.CreateTableAsync<FavoriteContentDto>();
            conn.CreateTableAsync<ImadsagDto>();
            conn.CreateTableAsync<VideoDto>();
        }
    }

    public async Task<ContentDto> GetElmelkedesContentById(Guid id)
    {
        try
        {
            Init();

            var res = await conn.Table<ContentDto>().Where(w => w.Nid == id).FirstOrDefaultAsync();

            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new ContentDto();
    }

    public async Task<int> GetFavoriteCount()
    {
        try
        {
            Init();

            var res = await conn.Table<FavoriteContentDto>().CountAsync();

            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return 0;
    }

    public async Task<List<FavoriteContentDto>> GetFavoriteContents()
    {
        try
        {
            Init();

            var res = await conn.Table<FavoriteContentDto>().OrderByDescending(o => o.Datum).ToListAsync();

            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<FavoriteContentDto>();
    }

    public async Task<List<ContentDto>> GetElmelkedesContents()
    {
        try
        {
            Init();

            var res = await conn.Table<ContentDto>()
                .Where(w => w.GroupName == "group_napiolvaso").OrderByDescending(o => o.Datum).ToListAsync();

            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<ContentDto>();
    }

    public async Task<int> InsertContent(ContentDto dto)
    {
        try
        {
            Init();
            var exists = await conn.Table<ContentDto>()
                .Where(w => w.Nid == dto.Nid && w.Tipus == dto.Tipus).FirstOrDefaultAsync();
            if (exists is null)
                return await conn.InsertAsync(dto);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** InsertContent EXCEPTION! {ex}");
        }
        return -1;
    }

    public async Task<int> InsertImadsag(ImadsagDto dto)
    {
        try
        {
            Init();
            var exists = await conn.Table<ImadsagDto>()
                .Where(w => w.Nid == dto.Nid).FirstOrDefaultAsync();
            if (exists is null)
                return await conn.InsertAsync(dto);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** InsertImadsag EXCEPTION! {ex}");
        }
        return -1;
    }

    public async Task<int> InsertFavoriteContent(FavoriteContentDto dto)
    {
        try
        {
            Init();
            var exists = await conn.Table<FavoriteContentDto>()
                .Where(w => w.Nid == dto.Nid && w.Tipus == dto.Tipus).FirstOrDefaultAsync();
            if (exists is null)
                return await conn.InsertAsync(dto);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return -1;
    }

    public async Task<ImadsagDto> GetFirstImadsag()
    {
        try
        {
            Init();
            var res = await conn.Table<ImadsagDto>().OrderByDescending(o => o.Datum).FirstOrDefaultAsync();
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return null;
    }

    public async Task<ContentDto> GetContentsByTypeName(string typeName)
    {
        try
        {
            Init();
            var res = await conn.Table<ContentDto>().Where(w => w.TypeName == typeName).OrderByDescending(o => o.Datum).FirstOrDefaultAsync();
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return null;
    }

    public async Task<int> DeleteContentByNid(Guid nid)
    {
        try
        {
            Init();
            var res = await conn.Table<ContentDto>().DeleteAsync(d => d.Nid == nid);
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** DeleteContentByNid EXCEPTION! {ex}");
        }
        return -1;
    }

    public async Task<int> DeleteImadsagByNid(Guid nid)
    {
        try
        {
            Init();
            var res = await conn.Table<ImadsagDto>().DeleteAsync(d => d.Nid == nid);
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** DeleteImadsagByNid EXCEPTION! {ex}");
        }
        return -1;
    }

    public virtual async Task<List<ContentDto>> GetAjanlatokContents()
    {
        try
        {
            Init();
            var id = ((int)Activities.ajanlatweb).ToString();
            var res = await conn.Table<ContentDto>().Where(w => w.Tipus == id).OrderByDescending(o => o.Datum).ToListAsync();
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<ContentDto>();
    }

    public async Task<List<ImadsagDto>> GetImaContents()
    {
        try
        {
            Init();
            var res = await conn.Table<ImadsagDto>()
                .Where(w => w.IsHided == false).OrderBy(o => o.Cim).ToListAsync();

            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<ImadsagDto>();
    }

    public async Task<List<ContentDto>> GetAudioContents()
    {
        try
        {
            Init();
            var res = await conn.Table<ContentDto>()
                .Where(w => w.GroupName == "group_audio").OrderByDescending(o => o.Datum).ToListAsync();

            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<ContentDto>();
    }

    public async Task<List<ContentDto>> GetBookContents()
    {
        try
        {
            Init();
            var bookId = ((int)Activities.book).ToString();
            var res = await conn.Table<ContentDto>().Where(w => w.Tipus == bookId).OrderByDescending(o => o.Datum).ToListAsync();
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<ContentDto>();
    }

    public async Task<List<ContentDto>> GetHumorContents()
    {
        try
        {
            Init();
            var res = await conn.Table<ContentDto>().Where(w => w.GroupName == "group_humor").OrderByDescending(o => o.Datum).ToListAsync();
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<ContentDto>();
    }

    public async Task<List<ContentDto>> GetMaiszentContents()
    {
        try
        {
            Init();
            var id = ((int)Activities.maiszent).ToString();
            var res = await conn.Table<ContentDto>().Where(w => w.Tipus == id).OrderByDescending(o => o.Datum).ToListAsync();
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<ContentDto>();
    }

    public async Task<List<ContentDto>> GetSzentekContents()
    {
        try
        {
            Init();
            var type = Activities.group_szentek.ToString();
            var res = await conn.Table<ContentDto>()
                .Where(w => w.GroupName == type).OrderByDescending(o => o.Datum).ToListAsync();
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<ContentDto>();
    }

    public async Task<List<ContentDto>> GetBlogContents()
    {
        try
        {
            Init();
            var res = await conn.Table<ContentDto>().Where(w => w.GroupName == "group_blog").OrderByDescending(o => o.Datum).ToListAsync();
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<ContentDto>();
    }

    public async Task<List<ContentDto>> GetNewsContents()
    {
        try
        {
            Init();
            var res = await conn.Table<ContentDto>().Where(w => w.GroupName == "group_news").OrderByDescending(o => o.Datum).ToListAsync();
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<ContentDto>();
    }

    public async Task<List<ContentDto>> GetContentsWithoutBook()
    {
        try
        {
            Init();
            var res = await conn.Table<ContentDto>().Where(w => w.TypeName != "book")
                .OrderBy(o => o.Datum)
                .ToListAsync();
            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new List<ContentDto>();
    }

    public async Task<int> SetContentAsReadById(Guid nid)
    {
        try
        {
            Init();
            var res = await conn.Table<ContentDto>().Where(w => w.Nid == nid && !w.IsRead).ToListAsync();
            if (res != null && res.Any())
            {
                res[0].IsRead = true;
                return await conn.UpdateAsync(res[0]);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return -1;
    }
}
