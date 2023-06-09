using androkat.maui.library.Models;

namespace androkat.maui.library.Abstraction;

public interface IAndrokatService
{
    Task<List<ContentResponse>> GetContents(string f, string n);
    Task<ImaResponse> GetImadsag(DateTime date);
}
