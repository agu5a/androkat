using androkat.domain.Configuration;
using Microsoft.Extensions.Options;

namespace androkat.web.Service;

public class ApiKeyValidator : IApiKeyValidator
{
    private readonly string _validApiKey;

    public ApiKeyValidator(IOptions<CredentialConfiguration> credentials)
    {
        _validApiKey = credentials.Value.CronApiKey;
    }

    public bool IsValid(string apiKey)
    {
        return _validApiKey == apiKey;
    }
}
