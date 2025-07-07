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
using System.Collections.Generic;
using System.Globalization;

namespace androkat.web.Pages.Ad;

[Authorize]
[BindProperties]
public class PufferModel : PageModel
{
    private readonly ILogger<PufferModel> _logger;
    private readonly IClock _iClock;
    private readonly IAdminRepository _adminRepository;
    private readonly IOptions<AndrokatConfiguration> _androkatConfiguration;

    public PufferModel(ILogger<PufferModel> logger, IClock iClock, IAdminRepository adminRepository, IOptions<AndrokatConfiguration> androkatConfiguration)
    {
        _logger = logger;
        _iClock = iClock;
        _adminRepository = adminRepository;
        _androkatConfiguration = androkatConfiguration;
    }

    public IEnumerable<AllTodayResult> AllTodayContent { get; set; }
    public LastTodayResult LastTodayResult { get; set; }
    public string TipusNev { get; set; }
    public string Cim { get; set; }
    public string Idezet { get; set; }
    public string FullDatum { get; set; }
    public string Forras { get; set; }
    public string Image { get; set; }
    public string FileUrl { get; set; }
    public int? TipusId { get; set; }
    public string Nid { get; set; }
    public string Today { get; set; }
    public bool IsNewItem { get; set; }
    public bool ShowToast { get; set; }
    public string Error { get; set; }

    public void OnGet(int? tipus, string id, string newitem)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(newitem) && newitem == "1")
            {
                IsNewItem = true;
            }

            var obj = string.IsNullOrWhiteSpace(id) || IsNewItem ? new domain.Model.AdminPage.ContentResult() : _adminRepository.LoadPufferTodayContentByNid(id);

            TipusId = tipus;
            AllTodayContent = _adminRepository.LoadAllTodayResult();
            LastTodayResult = tipus is null ? new LastTodayResult() : _adminRepository.GetLastTodayContentByTipus(tipus.Value);

            if (obj?.Def is null && tipus.HasValue)
            {
                TipusNev = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(tipus.Value).TipusNev;
            }
            else
            {
                TipusNev = obj?.Def;
            }

            Forras = obj?.Forras;
            Idezet = obj?.Idezet;
            FileUrl = obj?.FileUrl;
            Cim = obj?.Cim;
            if (tipus.HasValue)
            {
                SetForras(tipus.Value);
                SetIdezet(tipus.Value);
                SetFileUrl(tipus.Value);
                SetCim(tipus.Value);
            }

            FullDatum = obj?.FullDatum;
            Image = obj?.Img;
            Nid = obj?.Nid;
            Today = _iClock.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("hu-HU"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            Error = ex.Message;
            ShowToast = true;
            ResetForm();
        }
    }

    public IActionResult OnPostSave()
    {
        AllTodayContent = _adminRepository.LoadAllTodayResult();

        if (!TipusId.HasValue)
        {
            Error = "Hiányzik a tipus!";
            ShowToast = true;
            return Page();
        }

        if (string.IsNullOrWhiteSpace(Cim) || string.IsNullOrWhiteSpace(FullDatum) || (string.IsNullOrWhiteSpace(Idezet) && string.IsNullOrWhiteSpace(FileUrl)))
        {
            Error = "Hiányzik kötelező adat: Cím/Dátum/Idézet vagy fájl url";
            ShowToast = true;
            return Page();
        }

        LastTodayResult = _adminRepository.GetLastTodayContentByTipus(TipusId.Value);

        // Check for duplicate content if TipusId is laciatya
        if (TipusId.Value == (int)domain.Enum.Forras.laciatya)
        {
            var hasDuplicate = _adminRepository.HasDuplicateContentByIdezet(Idezet ?? "", TipusId.Value);
            if (hasDuplicate)
            {
                Error = "Ez az idézet már létezik az adatbázisban!";
                ShowToast = true;
                return Page();
            }
        }

        // Check if FullDatum already has a time component
        var dateToUse = FullDatum;
        if (!FullDatum.Contains(':')) // If there's no time component (no colon character)
        {
            dateToUse += DateTime.Now.ToString(" HH:mm:ss");
        }

        var newContent = new ContentDetailsModel(Guid.Empty, DateTime.Parse(dateToUse, CultureInfo.CreateSpecificCulture("hu-HU")),
            Cim, Idezet ?? "", TipusId.Value,
        _iClock.Now.Date, string.Empty, Image ?? "", FileUrl ?? "", Forras ?? "");

        var res = _adminRepository.InsertContent(newContent);

        if (IsNewItem)
        {
            ResetForm();
        }

        Error = res ? "A mentés sikerült" : "Valamilyen hiba történt";
        ShowToast = true;
        return Page();
    }

    public IActionResult OnPostDelete()
    {
        AllTodayContent = _adminRepository.LoadAllTodayResult();
        LastTodayResult = new LastTodayResult();

        if (string.IsNullOrWhiteSpace(Nid))
        {
            return Page();
        }

        var res = _adminRepository.DeleteTempContentByNid(Nid);
        if (res)
        {
            ResetForm();
            Error = "A törlés sikerült";
            ShowToast = true;
            return Page();
        }

        Error = "Valamilyen hiba történt!";
        ShowToast = true;
        return Page();
    }

    private void ResetForm()
    {
        Cim = "";
        Idezet = "";
        FileUrl = "";
        Forras = "";
        Image = "";
        Nid = "";
        FullDatum = "";
        TipusId = null;
        TipusNev = "";
        LastTodayResult = new LastTodayResult();
        AllTodayContent = _adminRepository.LoadAllTodayResult();
    }

    private void SetForras(int tipus)
    {
        if (!string.IsNullOrEmpty(Forras))
        {
            return;
        }

        Forras = tipus switch
        {
            24 => //szeretet újság
                "https://www.szentgellertkiado.hu/szeretet-ujsag",
            58 => //ajándékozz könyvet
                "https://www.szentgellertkiado.hu",
            (int)domain.Enum.Forras.advent or (int)domain.Enum.Forras.nagybojt => "https://orszagutiferencesek.hu",
            _ => Forras
        };
    }

    private void SetFileUrl(int tipus)
    {
        if (!string.IsNullOrEmpty(FileUrl))
        {
            return;
        }

        FileUrl = tipus switch
        {
            (int)domain.Enum.Forras.prayasyougo => string.IsNullOrWhiteSpace(FileUrl) ? $"https://androkat.hu/download/{DateTime.Now:MM_dd}.mp3" : FileUrl,
            (int)domain.Enum.Forras.audiobarsi => string.IsNullOrWhiteSpace(FileUrl) ? $"https://androkat.hu/download/" : FileUrl,
            (int)domain.Enum.Forras.audiopalferi => string.IsNullOrWhiteSpace(FileUrl) ? $"https://androkat.hu/download/" : FileUrl,
            _ => FileUrl
        };
    }

    private void SetIdezet(int tipus)
    {
        switch (tipus)
        {
            case (int)domain.Enum.Forras.advent:
            case (int)domain.Enum.Forras.nagybojt:
                var nextdate = _iClock.Now.DateTime.ToString("yyyy-MM-dd");
                Idezet = "<p><strong>Olvasd</strong>!<br>" +
                    "<a target=\"_blank\" href=\"https://igenaptar.katolikus.hu/nap/index.php?holnap=" + nextdate + "\">IDE</a></p>" +
                    "<p><strong>Elmélkedj</strong>!<br>IDE</p><p><strong>Cselekedj</strong>!<br>IDE</p>";
                break;
        }
    }

    private void SetCim(int tipus)
    {
        switch (tipus)
        {
            case (int)domain.Enum.Forras.prayasyougo:
                var res = _adminRepository.GetLastTodayContentByTipus((int)domain.Enum.Forras.maievangelium);
                Cim = res.Cim.Replace(" (Napi Ige)", "");
                break;
            case (int)domain.Enum.Forras.laciatya:
                Cim = $"{DateTime.Now:yyyy-MM-dd} {DayReplace(DateTime.Now.DayOfWeek.ToString())}";
                break;
        }
    }

    private static string DayReplace(string day)
    {
        return day
        .Replace("Monday", "Hétfő")
        .Replace("Tuesday", "Kedd")
        .Replace("Wednesday", "Szerda")
        .Replace("Thursday", "Csütörtök")
        .Replace("Friday", "Péntek")
        .Replace("Saturday", "Szombat")
        .Replace("Sunday", "Vasárnap");
    }
}