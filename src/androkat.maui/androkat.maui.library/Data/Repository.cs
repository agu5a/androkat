#nullable enable
using System.Diagnostics;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using SQLite;

namespace androkat.maui.library.Data;

public class Repository : IRepository
{
    private readonly string _dbPath;
    private SQLiteAsyncConnection? _conn;

    public Repository(string dbPath)
    {
        _dbPath = dbPath;
        Init(true);
    }

    public void Init(bool tableCheck = false)
    {
        if (_conn != null)
            return;

        const SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite |
                                      // create the database if it doesn't exist
                                      SQLiteOpenFlags.Create |
                                      // enable multi-threaded database access
                                      SQLiteOpenFlags.SharedCache;

        //https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/database-sqlite?view=net-maui-8.0
        //Database.EnableWriteAheadLoggingAsync()
        _conn = new SQLiteAsyncConnection(_dbPath, flags);

        if (tableCheck)
        {
            _conn.CreateTableAsync<ContentEntity>();
            _conn.CreateTableAsync<FavoriteContentEntity>();
            _conn.CreateTableAsync<ImadsagEntity>();
            _conn.CreateTableAsync<VideoEntity>();
            _conn.CreateTableAsync<Bunok>();
            _conn.CreateTableAsync<GyonasiJegyzet>();
        }
    }

    public async Task<ContentEntity> GetContentById(Guid id)
    {
        try
        {
            Init();

            return await _conn!.Table<ContentEntity>().Where(w => w.Nid == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new ContentEntity();
    }

    public async Task<ImadsagEntity> GetImadsagEntityById(Guid id)
    {
        try
        {
            Init();

            return await _conn!.Table<ImadsagEntity>().Where(w => w.Nid == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new ImadsagEntity();
    }

    public async Task<int> GetFavoriteCount()
    {
        try
        {
            Init();

            return await _conn!.Table<FavoriteContentEntity>().CountAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return 0;
    }

    public async Task<List<FavoriteContentEntity>> GetFavoriteContents()
    {
        try
        {
            Init();

            return await _conn!.Table<FavoriteContentEntity>().OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<ImadsagEntity?> GetFirstImadsag()
    {
        try
        {
            Init();
            return await _conn!.Table<ImadsagEntity>().OrderByDescending(o => o.Datum).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return null;
    }

    public async Task<ContentEntity?> GetContentsByTypeName(string typeName)
    {
        try
        {
            Init();
            return await _conn!.Table<ContentEntity>().Where(w => w.TypeName == typeName).OrderByDescending(o => o.Datum).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return null;
    }

    public async Task<int> GetContentsCount()
    {
        try
        {
            Init();
            return await _conn!.Table<ContentEntity>().CountAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** GetContentsCount EXCEPTION! {ex}");
        }

        return 0;
    }

    public async Task<GyonasiJegyzet?> GetGyonasiJegyzet()
    {
        try
        {
            Init();
            return await _conn!.Table<GyonasiJegyzet>().FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return new();
    }

    public async Task<List<ContentEntity>> GetContentsByTypeId(string typeId)
    {
        try
        {
            Init();
            return await _conn!.Table<ContentEntity>().Where(w => w.Tipus == typeId).OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ImadsagEntity>> GetImaContents(int pageNumber, int pageSize)
    {
        try
        {
            Init();
            return await _conn!.Table<ImadsagEntity>()
                .Where(w => !w.IsHided).OrderBy(o => o.Cim)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ContentEntity>> GetContentsByGroupName(string groupName)
    {
        try
        {
            Init();
            return await _conn!.Table<ContentEntity>()
                .Where(w => w.GroupName == groupName).OrderByDescending(o => o.Datum).ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository GetContentsByGroupName EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ContentEntity>> GetContentsWithoutBook()
    {
        try
        {
            Init();
            return await _conn!.Table<ContentEntity>().Where(w => w.TypeName != "book")
                .OrderBy(o => o.Datum)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<int> InsertContent(ContentEntity entity)
    {
        try
        {
            Init();
            var exists = await _conn!.Table<ContentEntity>()
                .Where(w => w.Nid == entity.Nid && w.Tipus == entity.Tipus).FirstOrDefaultAsync();
            if (exists is null)
                return await _conn.InsertAsync(entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** InsertContent EXCEPTION! {ex}");
        }

        return -1;
    }
        
    public async Task<int> InsertImadsag(ImadsagEntity entity)
    {
        try
        {
            Init();
            var exists = await _conn!.Table<ImadsagEntity>()
                .Where(w => w.Nid == entity.Nid).FirstOrDefaultAsync();
            if (exists is null)
                return await _conn.InsertAsync(entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** InsertImadsag EXCEPTION! {ex}");
        }

        return -1;
    }

    public async Task<int> InsertFavoriteContent(FavoriteContentEntity entity)
    {
        try
        {
            Init();
            var exists = await _conn!.Table<FavoriteContentEntity>()
                .Where(w => w.Nid == entity.Nid && w.Tipus == entity.Tipus).FirstOrDefaultAsync();
            if (exists is null)
                return await _conn.InsertAsync(entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return -1;
    }
    
    public async Task<int> DeleteContentByNid(Guid nid)
    {
        try
        {
            Init();
            return await _conn!.Table<ContentEntity>().DeleteAsync(d => d.Nid == nid);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** DeleteContentByNid EXCEPTION! {ex}");
        }

        return -1;
    }

    public async Task<int> DeleteAllContent()
    {
        try
        {
            Init();
            return await _conn!.Table<ContentEntity>().DeleteAsync(d => d.Nid != Guid.Empty || d.Nid == Guid.Empty);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** DeleteAllContent EXCEPTION! {ex}");
        }

        return -1;
    }

    public async Task<int> DeleteAllFavorite()
    {
        try
        {
            Init();
            return await _conn!.Table<FavoriteContentEntity>().DeleteAsync(d => d.Nid != Guid.Empty || d.Nid == Guid.Empty);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** DeleteAllFavorite EXCEPTION! {ex}");
        }

        return -1;
    }

    public async Task<int> DeleteAllImadsag()
    {
        try
        {
            Init();
            return await _conn!.Table<ImadsagEntity>().DeleteAsync(d => d.Csoport != 5);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** DeleteAllImadsag EXCEPTION! {ex}");
        }

        return -1;
    }

    public async Task<int> DeleteImadsagByNid(Guid nid)
    {
        try
        {
            Init();
            return await _conn!.Table<ImadsagEntity>().DeleteAsync(d => d.Nid == nid);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** DeleteImadsagByNid EXCEPTION! {ex}");
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
                res = await _conn!.Table<Bunok>().DeleteAsync(d => d.ParancsId > -1);
            }

            if (jegyzet)
            {
                res += await _conn!.Table<GyonasiJegyzet>().DeleteAsync(d => d.Jegyzet != null);
            }

            return res;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** DeleteUserGyonas EXCEPTION! {ex}");
        }

        return -1;
    }    

    public async Task<int> SetContentAsReadById(Guid nid)
    {
        try
        {
            Init();
            var res = await _conn!.Table<ContentEntity>().Where(w => w.Nid == nid && !w.IsRead).ToListAsync();
            if (res != null && res.Count != 0)
            {
                res[0].IsRead = true;
                return await _conn.UpdateAsync(res[0]);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return -1;
    }

    public async Task<int> UpsertGyonasiJegyzet(string notes)
    {
        try
        {
            Init();
            var res = await _conn!.Table<GyonasiJegyzet>().FirstOrDefaultAsync();
            if (res != null)
            {
                res.Jegyzet = notes;
                return await _conn.UpdateAsync(res);
            }

            return await _conn.InsertAsync(new GyonasiJegyzet { Jegyzet = notes });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository EXCEPTION! {ex}");
        }

        return -1;
    }
}