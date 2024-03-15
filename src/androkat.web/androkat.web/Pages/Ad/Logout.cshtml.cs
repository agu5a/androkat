using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace androkat.web.Pages.Ad;

[Authorize]
[BindProperties]
public class LogoutModel : PageModel
{
    protected readonly ILogger<LogoutModel> _logger;

    public LogoutModel(ILogger<LogoutModel> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        try
        {
            _logger.LogInformation("Logout RemoteIpAddress {IP}", Request.HttpContext.Connection.RemoteIpAddress?.ToString());
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        return Redirect("/");
    }
}