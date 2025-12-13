using androkat.application.Interfaces;
using androkat.domain.Model.WebResponse;
using androkat.web.Endpoints.V3Endpoints.Model;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace androkat.web.Endpoints.V3Endpoints;

/// <summary>
/// android video
/// </summary>
public class Radios : Endpoint<RadioRequest, IEnumerable<RadioMusorResponse>>
{
    private readonly string _route;
    private readonly IApiService _apiService;
    private readonly ILogger<Radios> _logger;

    public Radios(IApiService apiService, ILogger<Radios> logger)
    {
        _route = "/v3/radio";
        _apiService = apiService;
        _logger = logger;
    }

    public override void Configure()
    {
        Get(_route);
        AllowAnonymous();

        Options(x => x.RequireRateLimiting("fixed-by-ip"));
    }

    public override async Task HandleAsync(RadioRequest request, CancellationToken ct)
    {
        try
        {
            var response = _apiService.GetRadioBySource(request.Radio, default);

            await Send.ResponseAsync(response, StatusCodes.Status200OK, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Failed to run {Name}", nameof(HandleAsync));

            await Send.ResponseAsync([], StatusCodes.Status500InternalServerError, ct);
        }
    }
}
