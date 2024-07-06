using androkat.domain;
using androkat.domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;

namespace androkat.web.Pages.Ad;

[Authorize]
[BindProperties]
public class MedjugorjeModel : PageModel
{
    private readonly IAdminRepository _adminRepository;

    public MedjugorjeModel(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public string Cim { get; set; }
    public string Datum { get; set; }
    public string Idezet { get; set; }
    public string Error { get; set; }

    public void OnGet()
    {
        //nothing here
    }

    public void OnPost()
    {
        if (string.IsNullOrWhiteSpace(Cim) || string.IsNullOrWhiteSpace(Idezet) || string.IsNullOrWhiteSpace(Datum))
        {
            Error = "valami hiányzik";
            return;
        }

        if (!DateTime.TryParseExact(Datum, "MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            Error = "nem jó a dátum";
            return;
        }

        var (isSuccess, message) = _adminRepository.InsertFixContent(Cim, Idezet, (int)Forras.medjugorje, Datum);
        Error = isSuccess ? "siker" : message;
    }
}