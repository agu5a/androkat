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
/// android ima
/// </summary>
public class Prays : Endpoint<PrayRequest, ImaResponse>
{
    private readonly string _route;
    private readonly IApiService _apiService;
    private readonly ILogger<Prays> _logger;

    public Prays(IApiService apiService, ILogger<Prays> logger)
    {
        _route = "/v3/ima";
        _apiService = apiService;
        _logger = logger;
    }

    public override void Configure()
    {
        Get(_route);
        AllowAnonymous();

        Options(x => x.RequireRateLimiting("fixed-by-ip"));
    }

    public override async Task HandleAsync(PrayRequest request, CancellationToken ct)
    {
        try
        {
            var response = _apiService.GetImaByDate(request.Date, default);

            await Send.ResponseAsync(response, StatusCodes.Status200OK, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Failed to run {Name}", nameof(HandleAsync));

            await Send.ResponseAsync(null, StatusCodes.Status500InternalServerError, ct);
        }
    }
}
