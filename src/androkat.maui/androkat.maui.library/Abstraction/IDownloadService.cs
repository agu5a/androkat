using androkat.maui.library.Models;

namespace androkat.maui.library.Abstraction;

public interface IDownloadService
{
    Task<int> DownloadAll();
    Task<int> StartUpdate(Activities act);
}