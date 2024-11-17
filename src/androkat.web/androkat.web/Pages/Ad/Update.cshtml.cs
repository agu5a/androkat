using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Model;
using androkat.domain.Model.AdminPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace androkat.web.Pages.Ad;

[Authorize]
[BindProperties]
public class UpdateModel : PageModel
{
    private readonly ILogger<UpdateModel> _logger;
    private readonly IClock _iClock;
    private readonly IAdminRepository _adminRepository;

    public UpdateModel(ILogger<UpdateModel> logger, IClock iClock, IAdminRepository adminRepository)
    {
        _logger = logger;
        _iClock = iClock;
        _adminRepository = adminRepository;
    }

    public string CurrentMonth { get; set; }
    public string Cim { get; set; }
    public string Idezet { get; set; }
    public string Forras { get; set; }
    public string Inserted { get; set; }
    public string Fulldatum { get; set; }
    public string Img { get; set; }
    public string FileUrl { get; set; }
    public string Tipus { get; set; }
    public string Nid { get; set; }
    public string Error { get; set; }
    public bool ShowToast { get; set; }
    public List<SelectListItem> AllRecordResult { get; set; }
    public List<SelectListItem> Tipusok { get; set; }
    public string Today { get; set; }

    public void OnGet()
    {
        GetDropDownData();

        try
        {
            AllRecordResult = [];
            Tipus = "";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }
    }

    public void OnPostSearch()
    {
        GetDropDownData();

        try
        {
            if (string.IsNullOrWhiteSpace(Tipus))
            {
                Tipus = ((int)domain.Enum.Forras.papaitwitter).ToString();
            }

            var all = _adminRepository.GetAllContentByTipus(int.Parse(Tipus)).ToList();
            AllRecordResult = all.Select(s => new SelectListItem { Text = s.Datum, Value = s.Nid.ToString() }).ToList();

            if (string.IsNullOrWhiteSpace(Nid))
            {
                return;
            }
            
            var obj = _adminRepository.LoadTodayContentByNid(Nid);
            Cim = obj?.Cim;
            Forras = obj?.Forras;
            Idezet = obj?.Idezet;
            Img = obj?.Img;
            FileUrl = obj?.FileUrl;
            Fulldatum = obj?.FullDatum;
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
            || string.IsNullOrWhiteSpace(Fulldatum) || (string.IsNullOrWhiteSpace(Idezet) && string.IsNullOrWhiteSpace(FileUrl)))
        {
            return;
        }

        var res = _adminRepository.UpdateContent(new ContentDetailsModel(Guid.Parse(Nid), DateTime.Parse(Fulldatum, CultureInfo.CreateSpecificCulture("hu-HU")), Cim, Idezet ?? "", default,
        DateTime.Parse(Inserted, CultureInfo.CreateSpecificCulture("hu-HU")), string.Empty, Img ?? "", FileUrl ?? "", Forras ?? "")
        );
        Error = res ? "A mentés sikerült" : "Valamilyen hiba történt";
        ShowToast = true;
    }

    public void OnPostDelete()
    {
        GetDropDownData();

        if (string.IsNullOrWhiteSpace(Nid))
        {
            return;
        }

        var res = _adminRepository.DeleteContent(Nid);

        Error = res ? "A mentés sikerült" : "Valamilyen hiba történt";
        ShowToast = true;
    }

    private void GetDropDownData()
    {
        try
        {
            Today = _iClock.Now.ToString("MM-dd");

            var result = new List<AllTipusResult>();
            foreach (var item in _adminRepository.GetAllContentTipusFromDb())
            {
                var label = item.Value;

                if (item.Key is (int)domain.Enum.Forras.audiohorvath 
                    or (int)domain.Enum.Forras.audiobarsi 
                    or (int)domain.Enum.Forras.audionapievangelium 
                    or (int)domain.Enum.Forras.audiopalferi 
                    or (int)domain.Enum.Forras.audiotaize)
                {
                    label = "audió " + label;
                }

                result.Add(new AllTipusResult
                {
                    Tipus = item.Key,
                    Label = label
                });
            }

            Tipusok = result.OrderBy(o => o.Label).Select(s => new SelectListItem { Value = s.Tipus.ToString(), Text = s.Label }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }
    }
}