using androkat.application.Contants;
using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Model;
using androkat.domain.Model.AdminPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;

namespace androkat.web.Pages.Ad;

[Authorize]
[BindProperties]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IAdminRepository _adminRepository;
    private readonly IAuthService _authService;

    public IndexModel(ILogger<IndexModel> logger, IAdminRepository adminRepository, IAuthService authService)
    {
        _logger = logger;
        _adminRepository = adminRepository;
        _authService = authService;
    }

    public AdminResult AdminResult { get; set; }
    public AdminAResult AdminAResult { get; set; }
    public AdminBResult AdminBResult { get; set; }

    public bool IsAdvent { get; set; }
    public bool IsNagyBojt { get; set; }

    public IActionResult OnGet()
    {
        try
        {
            if (!_authService.IsAuthenticated(ContantValues.Admin))
            {
                return Redirect("/");
            }

            _logger.LogInformation("Login RemoteIpAddress {IP}", Request.HttpContext.Connection.RemoteIpAddress?.ToString());
            _adminRepository.LogInUser(ContantValues.Admin);

            var res = _adminRepository.GetIsAdventAndNagybojt();
            res.ForEach(x =>
            {
                switch (x.Key)
                {
                    case Constants.IsAdvent:
                        IsAdvent = Convert.ToBoolean(x.Value);
                        break;
                    case Constants.IsNagyBojt:
                        IsNagyBojt = Convert.ToBoolean(x.Value);
                        break;
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            return Redirect("/");
        }

        AdminResult = _adminRepository.GetAdminResult();
        AdminAResult = _adminRepository.GetAdminAResult(IsAdvent, IsNagyBojt);
        AdminBResult = _adminRepository.GetAdminBResult();

        return Page();
    }
}