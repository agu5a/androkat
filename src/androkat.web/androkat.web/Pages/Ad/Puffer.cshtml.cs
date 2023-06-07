using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Model;
using androkat.domain.Model.AdminPage;
using androkat.infrastructure.Model.SQLite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace androkat.web.Pages.Ad;

//[Authorize()]
[BindProperties]
public class PufferModel : PageModel
{
    protected readonly ILogger<PufferModel> _logger;
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
    public string Error { get; set; }
    public string Today { get; set; }
    public bool IsNewItem { get; set; }

    public void OnGet(int? tipus, string id, string newitem)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(newitem) && newitem == "1")
                IsNewItem = true;

            var obj = string.IsNullOrWhiteSpace(id) || IsNewItem ? new domain.Model.AdminPage.ContentResult() : _adminRepository.LoadPufferNapiByNid(id);

            TipusId = tipus;
            AllTodayContent = _adminRepository.LoadAllTodayResult();
            LastTodayResult = tipus is null ? new LastTodayResult() : _adminRepository.GetLastTodayContentByTipus(tipus.Value);

            if (obj?.Def is null && tipus.HasValue)
            {
                TipusNev = _androkatConfiguration.Value.GetContentMetaDataModelByTipus(tipus.Value).TipusNev;
            }
            else
                TipusNev = obj?.Def;

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
            Today = _iClock.Now.ToString("yyyy-MM-dd");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            AllTodayContent = _adminRepository.LoadAllTodayResult();
            LastTodayResult = tipus is null ? new LastTodayResult() : _adminRepository.GetLastTodayContentByTipus(tipus.Value);
        }
    }

    public IActionResult OnPostSave()
    {
        AllTodayContent = _adminRepository.LoadAllTodayResult();

        if (!TipusId.HasValue)
        {
            Error = "hiányzik a tipus";
            return Page();
        }

        if (string.IsNullOrWhiteSpace(Cim) || string.IsNullOrWhiteSpace(FullDatum) || (string.IsNullOrWhiteSpace(Idezet) && string.IsNullOrWhiteSpace(FileUrl)))
        {
            Error = "hiányzik";
            return Page();
        }

        LastTodayResult = _adminRepository.GetLastTodayContentByTipus(TipusId.Value);
        var newNapiolvaso = new ContentDetailsModel(Guid.Empty, DateTime.Parse(FullDatum + DateTime.Now.ToString(" HH:mm:ss")), 
            Cim, Idezet ?? "", TipusId.Value,
        _iClock.Now.Date, string.Empty, Image ?? "", FileUrl ?? "", Forras ?? "");

        var res = _adminRepository.InsertContent(newNapiolvaso);
        Error = res ? "siker" : "vmi rossz volt";

        if (IsNewItem)
            return Redirect("/Ad/Puffer");

        return Page();
    }

    public IActionResult OnPostDelete()
    {
        AllTodayContent = _adminRepository.LoadAllTodayResult();
        LastTodayResult = new LastTodayResult();

        if (string.IsNullOrWhiteSpace(Nid))
            return Page();

        var res = _adminRepository.DeleteTempContentByNid(Nid);
        if (res)
            return Redirect("/Ad/Puffer");

        Error = "vmi rossz volt";
        return Page();
    }

    private void SetForras(int tipus)
    {
        if (string.IsNullOrEmpty(Forras))
            switch (tipus)
            {
                case 24: //szeretet újság
                    Forras = "https://www.szentgellertkiado.hu/szeretet-ujsag";
                    break;
                case 58: //ajándékozz könyvet
                    Forras = "https://www.szentgellertkiado.hu";
                    break;
                case (int)domain.Enum.Forras.advent:
                case (int)domain.Enum.Forras.nagybojt:
                    Forras = "https://orszagutiferencesek.hu";
                    break;
            }
    }

    private void SetFileUrl(int tipus)
    {
        if (string.IsNullOrEmpty(FileUrl))
            switch (tipus)
            {
                case 15: // Napi útra való
                    FileUrl = string.IsNullOrWhiteSpace(FileUrl) ? $"https://androkat.hu/download/{DateTime.Now.ToString("MM_dd")}.mp3" : FileUrl;
                    break;
                case 38: //barsi audio
                    FileUrl = string.IsNullOrWhiteSpace(FileUrl) ? "https://androkat.hu/download/" : FileUrl;
                    break;
            }
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
                Cim = res.Cim;
                break;
            case (int)domain.Enum.Forras.laciatya:                            
                Cim = $"{DateTime.Now.ToString("yyyy-MM-dd")} {DayReplace(DateTime.Now.DayOfWeek.ToString())}";
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