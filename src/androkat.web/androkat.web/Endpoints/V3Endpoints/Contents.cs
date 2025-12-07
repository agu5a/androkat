using androkat.application.Interfaces;
using androkat.domain.Configuration;
using androkat.domain.Model.WebResponse;
using androkat.infrastructure.Model;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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

        Options(x => x.RequireRateLimiting("fixed-by-ip"));
    }

    public override async Task HandleAsync(ContentRequest request, CancellationToken ct)
    {
        try
        {
            Guid.TryParse(request.Id, out var guid);

            // Get response (this will cache it and the ETag if not already cached)
            var response = _apiService.GetContentByTipusAndId(request.Tipus, guid, default, default);

            // Get pre-computed ETag from cache (should be available now)
            var etag = _apiService.GetETag(request.Tipus, guid);

            // Check If-None-Match header
            var clientETag = HttpContext.Request.Headers.IfNoneMatch.FirstOrDefault();

            if (!string.IsNullOrEmpty(clientETag) && !string.IsNullOrEmpty(etag) && clientETag == etag)
            {
                // Content hasn't changed - return 304 Not Modified
                HttpContext.Response.Headers.ETag = etag;
                await Send.ResponseAsync(null, StatusCodes.Status304NotModified, ct);
                return;
            }

            // Content changed or first request - return 200 OK with ETag
            if (!string.IsNullOrEmpty(etag))
            {
                HttpContext.Response.Headers.ETag = etag;
            }

            HttpContext.Response.Headers.CacheControl = "private, max-age=300"; // 5 minutes
            await Send.ResponseAsync(response, StatusCodes.Status200OK, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Failed to run {Name}", nameof(HandleAsync));

            await Send.ResponseAsync([], StatusCodes.Status500InternalServerError, ct);
        }
    }
}
