﻿#nullable enable
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
            await SetContentAsReadById(id);
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

    public async Task<List<Bunok>> GetBunok()
    {
        try
        {
            Init();

            return await _conn!.Table<Bunok>().ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** GetBunok EXCEPTION! {ex}");
        }

        return [];
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

    public async Task<List<ContentEntity>> GetContentsByTypeId(string typeId, bool returnVisited = true, List<string>? enabledSources = null)
    {
        try
        {
            Init();
            var query = _conn!.Table<ContentEntity>()
                .Where(w => w.Tipus == typeId);

            var results = await query.OrderByDescending(o => o.Datum).ToListAsync();

            // Apply combined filtering logic
            return ApplyFilters(results, returnVisited, enabledSources);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** Repository GetContentsByTypeId EXCEPTION! {ex}");
        }

        return [];
    }

    public async Task<List<ImadsagEntity>> GetImaContents(int pageNumber, int pageSize, int? categoryId = null)
    {
        try
        {
            Init();
            var query = _conn!.Table<ImadsagEntity>()
                .Where(w => !w.IsHided);

            if (categoryId.HasValue && categoryId.Value > -1)
            {
                query = query.Where(w => w.Csoport == categoryId.Value);
            }

            return await query.OrderBy(o => o.Cim)
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

    public async Task<List<ContentEntity>> GetContentsByGroupName(string groupName, bool returnVisited = true, List<string>? enabledSources = null)
    {
        try
        {
            Init();
            var query = _conn!.Table<ContentEntity>()
                .Where(w => w.GroupName == groupName);

            var results = await query.OrderByDescending(o => o.Datum).ToListAsync();

            // Apply combined filtering logic
            return ApplyFilters(results, returnVisited, enabledSources);
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

    public async Task<int> InsertBunok(Bunok entity)
    {
        try
        {
            Init();
            var exists = await _conn!.Table<Bunok>()
                .Where(w => w.BunId == entity.BunId && w.ParancsId == entity.ParancsId).FirstOrDefaultAsync();
            if (exists is null)
            {
                entity.Nid = Guid.NewGuid();
                return await _conn.InsertAsync(entity);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** InsertBunok EXCEPTION! {ex}");
        }

        return -1;
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

    public async Task<int> DeleteFavoriteContentByNid(Guid nid)
    {
        try
        {
            Init();
            return await _conn!.Table<FavoriteContentEntity>().DeleteAsync(d => d.Nid == nid);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** DeleteFavoriteContentByNid EXCEPTION! {ex}");
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

    public async Task<int> DeleteBunokByIds(int bunId, int parancsId)
    {
        try
        {
            Init();
            return await _conn!.Table<Bunok>().DeleteAsync(d => d.BunId == bunId && d.ParancsId == parancsId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"********************************** DeleteBunokByIds EXCEPTION! {ex}");
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

    private static List<ContentEntity> ApplyFilters(List<ContentEntity> results, bool returnVisited, List<string>? enabledSources)
    {
        var filteredResults = results;

        // Debug logging to see what data we're working with
        Debug.WriteLine($"ApplyFilters called with {results.Count} results, returnVisited: {returnVisited}");
        Debug.WriteLine(enabledSources != null
            ? $"EnabledSources: [{string.Join(", ", enabledSources)}]"
            : "EnabledSources: null");

        // Log some sample TypeName values to see what's in the database
        var sampleEntries = results.Take(20).Select(r => new { r.TypeName, r.Tipus, r.GroupName }).ToList();
        Debug.WriteLine($"=== DATABASE CONTENT ANALYSIS ===");
        Debug.WriteLine($"Total results: {results.Count}");
        Debug.WriteLine($"Sample database entries (first 20):");
        foreach (var entry in sampleEntries)
        {
            Debug.WriteLine($"  TypeName: '{entry.TypeName}', Tipus: '{entry.Tipus}', GroupName: '{entry.GroupName}'");
        }

        // Show unique TypeName values
        var uniqueTypeNames = results.Select(r => r.TypeName).Distinct().OrderBy(x => x).ToList();
        Debug.WriteLine($"All unique TypeName values in results ({uniqueTypeNames.Count} total):");
        Debug.WriteLine($"  [{string.Join(", ", uniqueTypeNames.Select(x => $"'{x}'"))}]");
        Debug.WriteLine($"=== END DATABASE ANALYSIS ===");

        // Apply source filtering first (if enabled sources are specified)
        // If enabledSources is provided but empty, show nothing
        // If enabledSources is null, it means no filtering should be applied (show all sources)
        if (enabledSources != null)
        {
            if (enabledSources.Count == 0)
            {
                // No sources enabled, return empty list
                Debug.WriteLine("No sources enabled, returning empty list");
                return new List<ContentEntity>();
            }

            // Filter to only enabled sources
            var beforeCount = filteredResults.Count;

            // Debug: Log detailed comparison information
            Debug.WriteLine("=== DETAILED SOURCE FILTERING DEBUG ===");
            foreach (var source in enabledSources)
            {
                var matchingByTipus = filteredResults.Where(content =>
                    content.Tipus.Equals(source, StringComparison.OrdinalIgnoreCase)).ToList();

                Debug.WriteLine($"Source '{source}' matches {matchingByTipus.Count} items by Tipus");

                if (matchingByTipus.Count > 0)
                {
                    Debug.WriteLine($"  Tipus matches: {string.Join(", ", matchingByTipus.Take(3).Select(m => $"'{m.Tipus}'"))}");
                }
            }

            Debug.WriteLine("=== END DETAILED DEBUG ===");

            // Filter by Tipus field (contains integer activity IDs)
            filteredResults = filteredResults.Where(content =>
                enabledSources.Any(source =>
                    content.Tipus.Equals(source, StringComparison.OrdinalIgnoreCase))).ToList();
            Debug.WriteLine($"Source filtering: {beforeCount} -> {filteredResults.Count}");
        }

        // Then apply read/unread filtering
        // If returnVisited is false, hide read items (show only unread)
        if (!returnVisited)
        {
            var beforeCount = filteredResults.Count;
            filteredResults = filteredResults.Where(content => !content.IsRead).ToList();
            Debug.WriteLine($"Read/unread filtering: {beforeCount} -> {filteredResults.Count}");
        }

        Debug.WriteLine($"Final result count: {filteredResults.Count}");
        return filteredResults;
    }
}