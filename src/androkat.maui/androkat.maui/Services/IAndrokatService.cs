using androkat.hu.Models;

namespace androkat.hu.Services;

public interface IAndrokatService
{
    Task<List<ContentResponse>> GetContents(string f, string n);
}
