using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace androkat.web.Pages;

public class ErrorModel : PageModel
{
    private readonly ILogger<ErrorModel> _logger;

    public ErrorModel(ILogger<ErrorModel> logger) => _logger = logger;

    public void OnGet(int? statusCode)
    {
        if (statusCode != null && statusCode != 0)
        {
            HandleStatusCodeCase(statusCode);
            return;
        }

        HandleStatusCodeLessCase();
    }

    private void HandleStatusCodeLessCase()
    {
        try
        {
            var exceptionMessage = string.Empty;
            string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                exceptionMessage = "The file was not found.";

            if (!string.IsNullOrWhiteSpace(exceptionHandlerPathFeature?.Path))
                exceptionMessage += $" {exceptionHandlerPathFeature?.Path}";

            if (!string.IsNullOrWhiteSpace(exceptionMessage))
                _logger.LogError("Error: {exceptionMessage}", exceptionMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: error");
        }
    }

    private void HandleStatusCodeCase(int? statusCode)
    {
        try
        {
            var iStatusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            if (statusCode != 404 && !string.IsNullOrWhiteSpace(iStatusCodeReExecuteFeature?.OriginalPath) && !iStatusCodeReExecuteFeature.OriginalPath.Contains("/sys/"))
            {
                var httpRequestFeature = HttpContext.Features.Get<IHttpRequestFeature>();
                _logger.LogError("Error - Path: {OriginalPath}, Query: {OriginalQueryString}, Code: {statusCode}, Method: {Method}, Scheme: {Scheme}, Protocol: {Protocol}",
                    iStatusCodeReExecuteFeature.OriginalPath, iStatusCodeReExecuteFeature.OriginalQueryString, statusCode, httpRequestFeature.Method,
                    httpRequestFeature.Scheme, httpRequestFeature.Protocol);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: error");
        }
    }
}