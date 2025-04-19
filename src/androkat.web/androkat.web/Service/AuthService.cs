using androkat.application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;

namespace androkat.web.Service;

[ExcludeFromCodeCoverage]
public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _context;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IHttpContextAccessor context, ILogger<AuthService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public bool IsAuthenticated(string email)
    {
        var claim = ((ClaimsIdentity)_context.HttpContext!.User.Identity!).Claims
               .FirstOrDefault(s => s.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

        if (string.IsNullOrEmpty(claim?.Value))
        {
            _logger.LogError("Exception: no auth email");
            return false;
        }

        if (claim.Value == email)
        {
            return true;
        }
        
        _logger.LogError("Exception: wrong auth email {Email}", claim.Value);
        return false;
    }
}