using androkat.maui.library.Models.Responses;

namespace androkat.maui.library.Abstraction;

public interface IAndrokatService
{
    Task<List<ContentResponse>> GetContents(string tipus, string nid);
    Task<ImaResponse> GetImadsag(DateTime date);
    Task<List<ServerInfoResponse>> GetServerInfo();
}
