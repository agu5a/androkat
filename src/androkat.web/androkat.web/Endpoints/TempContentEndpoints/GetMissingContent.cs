using androkat.domain;
using androkat.domain.Configuration;
using androkat.web.Service;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace androkat.web.Endpoints.TempContentEndpoints;

public class GetMissingContent : Endpoint<MissingContentRequest, MissingContentResponse>
{
    private readonly IAdminRepository _adminRepository;
    private readonly string _route;
    private readonly IApiKeyValidator _apiKeyValidator;

    public GetMissingContent(IAdminRepository adminRepository,
    IOptions<EndPointConfiguration> endPointsCongfig,
    IApiKeyValidator apiKeyValidator)
    {
        _adminRepository = adminRepository;
        _route = endPointsCongfig.Value.MissingContentUrl;
        _apiKeyValidator = apiKeyValidator;
    }

    public override void Configure()
    {
        Get(_route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(MissingContentRequest request, CancellationToken ct)
    {
        if (!_apiKeyValidator.IsValid(request.ApiKey))
        {
            await SendAsync(new MissingContentResponse("Invalid API Key"), StatusCodes.Status401Unauthorized, ct);
            return;
        }

        var result = _adminRepository.GetMaiHianyzok();
        await SendAsync(new MissingContentResponse(result), StatusCodes.Status200OK, ct);
    }
}
