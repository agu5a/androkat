namespace androkat.web.Service;

public interface IApiKeyValidator
{
    bool IsValid(string apiKey);
}