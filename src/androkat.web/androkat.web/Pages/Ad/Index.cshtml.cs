using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Model;
using androkat.domain.Model.AdminPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace androkat.web.Pages.Ad;

[Authorize]
[BindProperties]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IAdminRepository _adminRepository;
    private readonly IAuthService _authService;
    private readonly ICronService _cronService;
    private readonly string _adminEmail;

    public IndexModel(
        ILogger<IndexModel> logger,
        IAdminRepository adminRepository,
        IAuthService authService,
        ICronService cronService,
        IOptions<GeneralConfiguration> generalConfig)
    {
        _logger = logger;
        _adminRepository = adminRepository;
        _authService = authService;
        _cronService = cronService;
        _adminEmail = generalConfig.Value.Admin;
    }

    public AdminResult AdminResult { get; set; }
    public AdminAResult AdminAResult { get; set; }
    public AdminBResult AdminBResult { get; set; }
    public string NewsInfo { get; set; }
    public string BlogInfo { get; set; }

    public bool IsAdvent { get; set; }
    public bool IsNagyBojt { get; set; }
    public bool ShowToast { get; set; }
    public string ToastMessage { get; set; }

    public IActionResult OnGet()
    {
        if (!_authService.IsAuthenticated(_adminEmail))
        {
            return Redirect("/");
        }

        _logger.LogInformation("Login RemoteIpAddress {IP}", Request.HttpContext.Connection.RemoteIpAddress?.ToString());
        _adminRepository.LogInUser(_adminEmail);
        GetData();

        return Page();
    }

    private void GetData()
    {
        try
        {
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
            Redirect("/");
        }

        AdminResult = _adminRepository.GetAdminResult();
        AdminAResult = _adminRepository.GetAdminAResult();
        AdminBResult = _adminRepository.GetAdminBResult();
        NewsInfo = _adminRepository.GetNewsInfo();
        BlogInfo = _adminRepository.GetBlogInfo();
    }


    public IActionResult OnPostDeleteOld()
    {
        try
        {
            if (!_authService.IsAuthenticated(_adminEmail))
            {
                return Redirect("/");
            }

            var deletedCount = _cronService.Start();
            _cronService.DeleteCaches();

            ShowToast = true;
            ToastMessage = $"Successfully deleted {deletedCount} old items and cleared caches.";

            GetData();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in OnPostDeleteOld: ");

            ShowToast = true;
            ToastMessage = "An error occurred while deleting old items.";

            // Still try to load the data
            AdminResult = _adminRepository.GetAdminResult();
            AdminAResult = _adminRepository.GetAdminAResult();
            AdminBResult = _adminRepository.GetAdminBResult();
            NewsInfo = _adminRepository.GetNewsInfo();
            BlogInfo = _adminRepository.GetBlogInfo();

            return Page();
        }
    }
}