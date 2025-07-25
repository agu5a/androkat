﻿using androkat.application.Interfaces;
using androkat.domain;
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
public class UpdateImaModel : PageModel
{
    private readonly ILogger<UpdateImaModel> _logger;
    private readonly IAdminRepository _adminRepository;
    private readonly IClock _iClock;

    public UpdateImaModel(ILogger<UpdateImaModel> logger, IAdminRepository adminRepository, IClock iClock)
    {
        _logger = logger;
        _adminRepository = adminRepository;
        _iClock = iClock;
    }

    public string Cim { get; set; }
    public string Idezet { get; set; }
    public string Datum { get; set; }
    public string Inserted { get; set; }
    public string Img { get; set; }
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
                Tipus = "11";
            }

            var all = _adminRepository.GetAllImaByCsoportResult(Tipus).ToList();
            AllRecordResult = all.Select(s => new SelectListItem { Text = s.Csoport.ToString(), Value = s.Nid.ToString() }).ToList();

            if (string.IsNullOrWhiteSpace(Nid))
            {
                // Reset form fields when Tipus changes but no Nid is selected
                ClearFormFields();
                return;
            }
            
            var obj = _adminRepository.LoadImaByNid(Nid);
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

        if (string.IsNullOrWhiteSpace(Tipus))
        {
            Tipus = "11";
        }

        var all = _adminRepository.GetAllImaByCsoportResult(Tipus).ToList();
        AllRecordResult = all.Select(s => new SelectListItem { Text = s.Csoport.ToString(), Value = s.Nid.ToString() }).ToList();

        var res = _adminRepository.UpdateIma(new domain.Model.ImaModel(Guid.Parse(Nid), DateTime.Parse(Datum, CultureInfo.CreateSpecificCulture("hu-HU")), Cim, Tipus, Idezet));

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

        var res = _adminRepository.DeleteIma(Nid);

        if (string.IsNullOrWhiteSpace(Tipus))
        {
            Tipus = "11";
        }

        var all = _adminRepository.GetAllImaByCsoportResult(Tipus).ToList();
        AllRecordResult = all.Select(s => new SelectListItem { Text = s.Csoport.ToString(), Value = s.Nid.ToString() }).ToList();
        
        // Clear form fields if deletion was successful
        if (res)
        {
            Cim = string.Empty;
            Idezet = string.Empty;
            Datum = string.Empty;
            Inserted = string.Empty;
            Img = string.Empty;
            Nid = string.Empty; // Clear the Nid selection
        }

        Error = res ? "A törlés sikerült" : "Valamilyen hiba történt";
        ShowToast = true;
    }
    private void GetDropDownData()
    {
        try
        {
            Today = _iClock.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));

            var imacsoportok = new Dictionary<string, string> {{ "Alapimák", "11" }, {"Napi imák","9" }, {"Kérő és felajánló imák","12" },
            {"Hála és dicsőítő imák","7"}, {"Litániák","4"}, {"Szentmise","3"},
            {"Szűz Mária","10"}, {"Rózsafüzér","2"}, {"Szentek imái","1"},
            {"Zsoltár","0" }}; //5->saját ima az android appban

            Tipusok = [];
            foreach (var item in imacsoportok)
            {
                Tipusok.Add(new SelectListItem { Text = item.Key, Value = item.Value });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }
    }

    private void ClearFormFields()
    {
        Cim = string.Empty;
        Idezet = string.Empty;
        Datum = string.Empty;
        Inserted = string.Empty;
        Img = string.Empty;
        Nid = string.Empty;
    }
}