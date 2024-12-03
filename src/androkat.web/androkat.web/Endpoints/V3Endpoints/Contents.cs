using androkat.application.Interfaces;
using androkat.domain.Configuration;
using androkat.domain.Model.WebResponse;
using androkat.infrastructure.Model;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace androkat.web.Endpoints.V3Endpoints;

public class Contents : Endpoint<ContentRequest, IEnumerable<ContentResponse>>
{
    private readonly string _route;
    private readonly IApiService _apiService;
    private readonly ILogger<Contents> _logger;

    public Contents(IOptions<EndPointConfiguration> endPointsCongfig, IApiService apiService, ILogger<Contents> logger)
    {
        _route = endPointsCongfig.Value.GetContentsApiUrl;
        _apiService = apiService;
        _logger = logger;
    }

    public override void Configure()
    {
        Get(_route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ContentRequest request, CancellationToken ct)
    {
        try
        {
            Guid.TryParse(request.Id, out var guid);

            var response = _apiService.GetContentByTipusAndId(request.Tipus, guid, default, default);
            await SendAsync(response, StatusCodes.Status200OK, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Failed to run {Name}", nameof(HandleAsync));

            await SendAsync([], StatusCodes.Status500InternalServerError, ct);
        }
    }
}
