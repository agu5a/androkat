using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using SQLite;

namespace androkat.maui.library.Data;

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

        SQLiteOpenFlags flags =
        // open the database in read/write mode
        SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLiteOpenFlags.SharedCache;

        //https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/database-sqlite?view=net-maui-8.0
        //Database.EnableWriteAheadLoggingAsync()
        conn = new SQLiteAsyncConnection(_dbPath, flags);

        if (tableCheck)
        {
            conn.CreateTableAsync<ContentEntity>();
            conn.CreateTableAsync<FavoriteContentEntity>();
            conn.CreateTableAsync<ImadsagEntity>();
            conn.CreateTableAsync<VideoEntity>();
            conn.CreateTableAsync<Bunok>();
            conn.CreateTableAsync<GyonasiJegyzet>();
        }
    }

    public async Task<ContentEntity> GetContentById(Guid id)
    {
        try
        {
            Init();

            return await conn.Table<ContentEntity>().Where(w => w.Nid == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new ContentEntity();
    }

    public async Task<ImadsagEntity> GetImadsagEntityById(Guid id)
    {
        try
        {
            Init();

            return await conn.Table<ImadsagEntity>().Where(w => w.Nid == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new ImadsagEntity();
    }

    public async Task<int> GetFavoriteCount()
    {
        try
        {
            Init();

            return await conn.Table<FavoriteContentEntity>().CountAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return 0;
    }

    public async Task<List<FavoriteContentEntity>> GetFavoriteContents()
    {
        try
        {
            Init();

            return await conn.Table<FavoriteContentEntity>().OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ContentEntity>> GetContents()
    {
        try
        {
            Init();

            return await conn.Table<ContentEntity>()
                .Where(w => w.GroupName == "group_napiolvaso").OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<int> InsertContent(ContentEntity entity)
    {
        try
        {
            Init();
            var exists = await conn.Table<ContentEntity>()
                .Where(w => w.Nid == entity.Nid && w.Tipus == entity.Tipus).FirstOrDefaultAsync();
            if (exists is null)
                return await conn.InsertAsync(entity);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** InsertContent EXCEPTION! {ex}");
        }
        return -1;
    }

    public async Task<int> InsertImadsag(ImadsagEntity entity)
    {
        try
        {
            Init();
            var exists = await conn.Table<ImadsagEntity>()
                .Where(w => w.Nid == entity.Nid).FirstOrDefaultAsync();
            if (exists is null)
                return await conn.InsertAsync(entity);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** InsertImadsag EXCEPTION! {ex}");
        }
        return -1;
    }

    public async Task<int> InsertFavoriteContent(FavoriteContentEntity entity)
    {
        try
        {
            Init();
            var exists = await conn.Table<FavoriteContentEntity>()
                .Where(w => w.Nid == entity.Nid && w.Tipus == entity.Tipus).FirstOrDefaultAsync();
            if (exists is null)
                return await conn.InsertAsync(entity);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return -1;
    }

    public async Task<ImadsagEntity> GetFirstImadsag()
    {
        try
        {
            Init();
            return await conn.Table<ImadsagEntity>().OrderByDescending(o => o.Datum).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return null;
    }

    public async Task<ContentEntity> GetContentsByTypeName(string typeName)
    {
        try
        {
            Init();
            return await conn.Table<ContentEntity>().Where(w => w.TypeName == typeName).OrderByDescending(o => o.Datum).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return null;
    }

    public async Task<int> GetContentsCount()
    {
        try
        {
            Init();
            return await conn.Table<ContentEntity>().CountAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** GetContentsCount EXCEPTION! {ex}");
        }

        return 0;
    }

    public async Task<int> DeleteContentByNid(Guid nid)
    {
        try
        {
            Init();
            return await conn.Table<ContentEntity>().DeleteAsync(d => d.Nid == nid);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** DeleteContentByNid EXCEPTION! {ex}");
        }
        return -1;
    }

    public async Task<int> DeleteAllContent()
    {
        try
        {
            Init();
            return await conn.Table<ContentEntity>().DeleteAsync(d => d.Nid != Guid.Empty || d.Nid == Guid.Empty);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** DeleteAllContent EXCEPTION! {ex}");
        }
        return -1;
    }

    public async Task<int> DeleteAllFavorite()
    {
        try
        {
            Init();
            return await conn.Table<FavoriteContentEntity>().DeleteAsync(d => d.Nid != Guid.Empty || d.Nid == Guid.Empty);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** DeleteAllFavorite EXCEPTION! {ex}");
        }
        return -1;
    }

    public async Task<int> DeleteAllImadsag()
    {
        try
        {
            Init();
            return await conn.Table<ImadsagEntity>().DeleteAsync(d => d.Csoport != 5);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** DeleteAllImadsag EXCEPTION! {ex}");
        }
        return -1;
    }

    public async Task<int> DeleteImadsagByNid(Guid nid)
    {
        try
        {
            Init();
            return await conn.Table<ImadsagEntity>().DeleteAsync(d => d.Nid == nid);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** DeleteImadsagByNid EXCEPTION! {ex}");
        }
        return -1;
    }

    public async Task<int> DeleteUserGyonas(bool jegyzet, bool bun)
    {
        try
        {
            Init();
            var res = 0;
            if (bun)
            {
                res = await conn.Table<Bunok>().DeleteAsync();
            }

            if (jegyzet)
            {
                res += await conn.Table<GyonasiJegyzet>().DeleteAsync();
            }

            return res;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** DeleteUserGyonas EXCEPTION! {ex}");
        }
        return -1;
    }

    public virtual async Task<List<ContentEntity>> GetAjanlatokContents()
    {
        try
        {
            Init();
            var id = ((int)Activities.ajanlatweb).ToString();
            return await conn.Table<ContentEntity>().Where(w => w.Tipus == id).OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ImadsagEntity>> GetImaContents(int pageNumber, int pageSize)
    {
        try
        {
            Init();
            return await conn.Table<ImadsagEntity>()
                .Where(w => !w.IsHided).OrderBy(o => o.Cim)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ContentEntity>> GetAudioContents()
    {
        try
        {
            Init();
            return await conn.Table<ContentEntity>()
                .Where(w => w.GroupName == "group_audio").OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ContentEntity>> GetBookContents()
    {
        try
        {
            Init();
            var bookId = ((int)Activities.book).ToString();
            return await conn.Table<ContentEntity>().Where(w => w.Tipus == bookId).OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ContentEntity>> GetHumorContents()
    {
        try
        {
            Init();
            return await conn.Table<ContentEntity>().Where(w => w.GroupName == "group_humor").OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ContentEntity>> GetMaiszentContents()
    {
        try
        {
            Init();
            var id = ((int)Activities.maiszent).ToString();
            return await conn.Table<ContentEntity>().Where(w => w.Tipus == id).OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ContentEntity>> GetSzentekContents()
    {
        try
        {
            Init();
            var type = Activities.group_szentek.ToString();
            return await conn.Table<ContentEntity>()
                .Where(w => w.GroupName == type).OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ContentEntity>> GetBlogContents()
    {
        try
        {
            Init();
            return await conn.Table<ContentEntity>().Where(w => w.GroupName == "group_blog").OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ContentEntity>> GetNewsContents()
    {
        try
        {
            Init();
            return await conn.Table<ContentEntity>().Where(w => w.GroupName == "group_news").OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ContentEntity>> GetContentsWithoutBook()
    {
        try
        {
            Init();
            return await conn.Table<ContentEntity>().Where(w => w.TypeName != "book")
                .OrderBy(o => o.Datum)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<int> SetContentAsReadById(Guid nid)
    {
        try
        {
            Init();
            var res = await conn.Table<ContentEntity>().Where(w => w.Nid == nid && !w.IsRead).ToListAsync();
            if (res != null && res.Count != 0)
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
