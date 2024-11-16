using androkat.domain;
using androkat.domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Pages.Ad;

[Authorize]
[BindProperties]
public class UpdateRadioModel : PageModel
{
    private readonly ILogger<UpdateRadioModel> _logger;
    private readonly IAdminRepository _adminRepository;

    public UpdateRadioModel(ILogger<UpdateRadioModel> logger, IAdminRepository adminRepository)
    {
        _logger = logger;
        _adminRepository = adminRepository;
    }

    public string Inserted { get; set; }
    public string Tipus { get; set; }
    public string Musor { get; set; }
    public string Source { get; set; }
    public string Nid { get; set; }
    public string Error { get; set; }
    public List<SelectListItem> AllRecordResult { get; set; }

    public void OnGet()
    {
        try
        {
            var all = _adminRepository.GetAllRadioResult().ToList();
            AllRecordResult = all.Select(s => new SelectListItem { Text = s.Csoport.ToString(), Value = s.Nid.ToString() }).ToList();
            Tipus = "";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }
    }

    public void OnPostSearch()
    {
        try
        {
            var all = _adminRepository.GetAllRadioResult().ToList();
            AllRecordResult = all.Select(s => new SelectListItem { Text = s.Csoport.ToString(), Value = s.Nid.ToString() }).ToList();

            if (string.IsNullOrWhiteSpace(Nid))
            {
                return;
            }
            
            var obj = _adminRepository.LoadRadioByNid(Nid);
            Musor = obj?.Musor;
            Source = obj?.Source;
            Nid = obj?.Nid;
            Inserted = obj?.Inserted;
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            _logger.LogError(ex, "Exception: ");
        }
    }

    public void OnPostSave()
    {
        if (string.IsNullOrWhiteSpace(Nid) || string.IsNullOrWhiteSpace(Source)
           || string.IsNullOrWhiteSpace(Inserted) || string.IsNullOrWhiteSpace(Musor))
        {
            return;
        }

        var all = _adminRepository.GetAllRadioResult().ToList();
        AllRecordResult = all.Select(s => new SelectListItem { Text = s.Csoport.ToString(), Value = s.Nid.ToString() }).ToList();

        var res = _adminRepository.UpdateRadio(new RadioMusorModel(Guid.Parse(Nid), Source, Musor, Inserted));

        Error = res ? "siker" : "vmi rossz volt";
    }

    public void OnPostDelete()
    {
        if (string.IsNullOrWhiteSpace(Nid))
        {
            return;
        }

        var res = _adminRepository.DeleteRadio(Nid);

        var all = _adminRepository.GetAllRadioResult().ToList();
        AllRecordResult = all.Select(s => new SelectListItem { Text = s.Csoport.ToString(), Value = s.Nid.ToString() }).ToList();

        Error = res ? "siker" : "vmi rossz volt";
    }
}