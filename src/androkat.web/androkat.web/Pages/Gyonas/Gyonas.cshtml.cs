using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Collections.Generic;

namespace androkat.web.Pages.Gyonas;

public class GyonasModel : PageModel
{
    private readonly ILogger<GyonasModel> _logger;

    public GyonasModel(ILogger<GyonasModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public string Parancsok { get; set; }

    public void OnGet()
    {
        Parancsok = "<div id=\"sins\">";

        var conf = HttpContext.Session.GetString("conf");
        if (!string.IsNullOrWhiteSpace(conf))
        {
            try
            {
                var dic = JsonSerializer.Deserialize<Dictionary<string, bool>>(conf);
                ElsoHarom(dic);
                MasodikHarom(dic);
                HarmadikHarom(dic);
                Tizedik(dic);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "GyonasModel");
            }
        }

        Parancsok += "</div>";
    }

    private void Tizedik(Dictionary<string, bool> dic)
    {
        if (dic["cb101"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p101) + "</strong></li>";
        if (dic["cb102"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p102) + "</strong></li>";
        if (dic["cb103"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p103) + "</strong></li>";
        if (dic["cb104"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p104) + "</strong></li>";
    }

    private void HarmadikHarom(Dictionary<string, bool> dic)
    {
        if (dic["cb71"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p71) + "</strong></li>";
        if (dic["cb72"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p72) + "</strong></li>";
        if (dic["cb73"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p73) + "</strong></li>";
        if (dic["cb74"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p74) + "</strong></li>";
        if (dic["cb75"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p75) + "</strong></li>";

        if (dic["cb81"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p81) + "</strong></li>";
        if (dic["cb82"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p82) + "</strong></li>";
        if (dic["cb83"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p83) + "</strong></li>";
        if (dic["cb84"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p84) + "</strong></li>";
        if (dic["cb85"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p85) + "</strong></li>";

        if (dic["cb91"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p91) + "</strong></li>";
        if (dic["cb92"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p92) + "</strong></li>";
        if (dic["cb93"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p93) + "</strong></li>";
    }

    private void MasodikHarom(Dictionary<string, bool> dic)
    {
        if (dic["cb41"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p41) + "</strong></li>";
        if (dic["cb42"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p42) + "</strong></li>";
        if (dic["cb43"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p43) + "</strong></li>";
        if (dic["cb44"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p44) + "</strong></li>";

        if (dic["cb51"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p51) + "</strong></li>";
        if (dic["cb52"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p52) + "</strong></li>";
        if (dic["cb53"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p53) + "</strong></li>";
        if (dic["cb54"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p54) + "</strong></li>";

        if (dic["cb61"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p61) + "</strong></li>";
        if (dic["cb62"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p62) + "</strong></li>";
        if (dic["cb63"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p63) + "</strong></li>";
        if (dic["cb64"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p64) + "</strong></li>";
    }

    private void ElsoHarom(Dictionary<string, bool> dic)
    {
        if (dic["cb11"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p11) + "</strong></li>";
        if (dic["cb12"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p12) + "</strong></li>";
        if (dic["cb13"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p13) + "</strong></li>";
        if (dic["cb14"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p14) + "</strong></li>";
        if (dic["cb15"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p15) + "</strong></li>";

        if (dic["cb21"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p21) + "</strong></li>";
        if (dic["cb22"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p22) + "</strong></li>";
        if (dic["cb23"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p23) + "</strong></li>";
        if (dic["cb24"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p24) + "</strong></li>";
        if (dic["cb25"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p25) + "</strong></li>";

        if (dic["cb31"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p31) + "</strong></li>";
        if (dic["cb32"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p32) + "</strong></li>";
        if (dic["cb33"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p33) + "</strong></li>";
        if (dic["cb34"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p34) + "</strong></li>";
        if (dic["cb35"]) Parancsok += "<li><strong>" + GetSzoveg(Parancs.p35) + "</strong></li>";
    }

    private static string GetSzoveg(string input)
    {
        return input.Replace("?", ".").Replace("-e ", " ");
    }
}