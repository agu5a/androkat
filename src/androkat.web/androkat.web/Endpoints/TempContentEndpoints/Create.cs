using androkat.domain;
using androkat.domain.Configuration;
using androkat.web.Service;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace androkat.web.Endpoints.TempContentEndpoints;

public class Create : Endpoint<ContentDetailsModelRequest, ContentDetailsModelResponse>
{
    private readonly IApiRepository _apiRepository;
    private readonly ILogger<Create> _logger;
    private readonly string _route;
    private readonly IApiKeyValidator _apiKeyValidator;

    public Create(IApiRepository apiRepository, ILogger<Create> logger, IOptions<EndPointConfiguration> endPointsCongfig, IApiKeyValidator apiKeyValidator)
    {
        _apiRepository = apiRepository;
        _logger = logger;
        _route = endPointsCongfig.Value.SaveTempContentApiUrl;
        _apiKeyValidator = apiKeyValidator;
    }

    public override void Configure()
    {
        Post(_route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ContentDetailsModelRequest request, CancellationToken ct)
    {
        if (!_apiKeyValidator.IsValid(request.ApiKey))
        {
            await SendAsync(new ContentDetailsModelResponse(false), StatusCodes.Status401Unauthorized, ct);
            return;
        }

        try
        {
            var result = _apiRepository.AddTempContent(request.ContentDetailsModel);
            var response = new ContentDetailsModelResponse(result);
            await SendAsync(response, result ? StatusCodes.Status200OK : StatusCodes.Status409Conflict, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Failed to run {Name}", nameof(HandleAsync));

            await SendAsync(new ContentDetailsModelResponse(false), StatusCodes.Status500InternalServerError, ct);
        }
    }
}