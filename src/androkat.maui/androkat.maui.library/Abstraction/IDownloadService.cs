using androkat.maui.library.Models;

namespace androkat.maui.library.Abstraction;

public interface IDownloadService
{
    Task<int> DownloadAll();
    Task<(Guid nid, bool exists)> HasTodayData(Activities type);
    void InsertIma();
    Task<int> StartUpdate(Activities act);
}