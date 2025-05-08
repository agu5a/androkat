using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Model.AdminPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace androkat.web.Pages.Ad;

[Authorize]
[BindProperties]
public class UpdateMaiSzentModel : PageModel
{
    private readonly ILogger<UpdateMaiSzentModel> _logger;
    private readonly IClock _iClock;
    private readonly IAdminRepository _adminRepository;
    private readonly IOptions<AndrokatConfiguration> _androkatConfiguration;

    public UpdateMaiSzentModel(ILogger<UpdateMaiSzentModel> logger, IClock iClock, IAdminRepository adminRepository, IOptions<AndrokatConfiguration> androkatConfiguration)
    {
        _logger = logger;
        _iClock = iClock;
        _adminRepository = adminRepository;
        _androkatConfiguration = androkatConfiguration;
    }

    public string CurrentMonth { get; set; }
    public string Cim { get; set; }
    public string Idezet { get; set; }
    public string Datum { get; set; }
    public string Inserted { get; set; }
    public string Img { get; set; }
    public string Tipus { get; set; }
    public string Nid { get; set; }
    public string Error { get; set; }
    public bool ShowToast { get; set; }
    public List<SelectListItem> Months { get; set; }
    public List<SelectListItem> AllRecordResult { get; set; }
    public List<SelectListItem> Tipusok { get; set; }
    public string Today { get; set; }

    public void OnGet()
    {
        GetDropDownData();

        try
        {
            // Set default month to current month
            CurrentMonth = _iClock.Now.ToString("MM");
            
            // Always set type to 21
            Tipus = "21";
            
            // Automatically populate the third dropdown with data for the current month
            var all = _adminRepository.GetAllMaiSzentByMonthResult(CurrentMonth).ToList();
            AllRecordResult = all.Select(s => new SelectListItem { Text = s.Datum, Value = s.Nid.ToString() }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            Error = ex.Message;
            ShowToast = true;
        }
    }

    public void OnPostSearch()
    {
        GetDropDownData();

        try
        {
            if (string.IsNullOrWhiteSpace(CurrentMonth))
            {
                CurrentMonth = _iClock.Now.ToString("MM");
            }

            // Always set Tipus to 21
            Tipus = "21";

            var all = _adminRepository.GetAllMaiSzentByMonthResult(CurrentMonth).ToList();
            AllRecordResult = all.Select(s => new SelectListItem { Text = s.Datum, Value = s.Nid.ToString() }).ToList();

            if (string.IsNullOrWhiteSpace(Nid))
            {
                return;
            }
            
            var obj = _adminRepository.LoadMaiSzentByNid(Nid);
            Cim = obj?.Cim;
            Datum = obj?.FullDatum;
            Idezet = obj?.Idezet;
            Img = obj?.Img;
            Inserted = obj?.Inserted;
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            ShowToast = true;
            _logger.LogError(ex, "Exception: ");
        }
    }

    public void OnPostSave()
    {
        GetDropDownData();

        if (string.IsNullOrWhiteSpace(Nid) || string.IsNullOrWhiteSpace(Cim)
            || string.IsNullOrWhiteSpace(Datum) || string.IsNullOrWhiteSpace(Idezet))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(CurrentMonth))
        {
            CurrentMonth = _iClock.Now.ToString("MM");
        }

        var all = _adminRepository.GetAllMaiSzentByMonthResult(CurrentMonth).ToList();
        AllRecordResult = all.Select(s => new SelectListItem { Text = s.Datum, Value = s.Nid.ToString() }).ToList();

        var res = _adminRepository.UpdateMaiSzent(new MaiSzentModel
        {
            Nid = Guid.Parse(Nid),
            Cim = Cim,
            Datum = Datum,
            Idezet = Idezet,
            Img = Img ?? "",
            Inserted = DateTime.Parse(Inserted, CultureInfo.CreateSpecificCulture("hu-HU"))
        });

        Error = res ? "A mentés sikerült" : "Valamilyen hiba történt";
        ShowToast = true;
    }

    private void GetDropDownData()
    {
        try
        {
            Today = _iClock.Now.ToString("MM-dd");
            var months = new List<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            Months = months.Select(s => new SelectListItem { Value = s, Text = s }).ToList();

            var result = new List<AllTipusResult>();
            var list = new List<int> { 21 };
            foreach (var item in list)
            {
                var label = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(item).TipusNev;
                if (item == 14)
                {
                    label = "audió " + label;
                }

                result.Add(new AllTipusResult
                {
                    Tipus = item,
                    Label = label
                });
            }

            Tipusok = result.Select(s => new SelectListItem { Value = s.Tipus.ToString(), Text = s.Label }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }
    }
}