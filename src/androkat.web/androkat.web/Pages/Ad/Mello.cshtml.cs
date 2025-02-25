﻿using androkat.domain;
using androkat.domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;

namespace androkat.web.Pages.Ad;

[Authorize]
[BindProperties]
public class MelloModel : PageModel
{
    private readonly IAdminRepository _adminRepository;

    public MelloModel(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public string Cim { get; set; }
    public string Datum { get; set; }
    public string Idezet { get; set; }
    public string Error { get; set; }
    public bool ShowToast { get; set; }

    public void OnGet()
    {
        //nothing here
    }

    public void OnPost()
    {
        if (string.IsNullOrWhiteSpace(Cim) || string.IsNullOrWhiteSpace(Idezet) || string.IsNullOrWhiteSpace(Datum))
        {
            Error = "Hiányzik kötelező adat";
            ShowToast = true;
            return;
        }

        if (!DateTime.TryParseExact(Datum, "MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            Error = "Nem megfelelő a dátum";
            ShowToast = true;
            return;
        }

        var (isSuccess, message) = _adminRepository.InsertFixContent(Cim, Idezet, (int)Forras.mello, Datum);
        Error = isSuccess ? "A mentés sikerült" : message;
        ShowToast = true;
    }
}