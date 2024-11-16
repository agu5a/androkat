using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace androkat.web.Pages.Partner;

[Authorize]
[BindProperties]
public class IndexModel : PageModel
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IClock _iClock;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, IPartnerRepository partnerRepository, IClock iClock)
    {
        _logger = logger;
        _partnerRepository = partnerRepository;
        _iClock = iClock;
    }

    public IEnumerable<ContentDetailsModel> Items { get; set; }
    public string Szoveg { get; set; }
    public string Datum { get; set; }
    public string Cim { get; set; }
    public string Idezet { get; set; }
    public int Tipus { get; set; }

    public IActionResult OnGet(int tipus, string nid)
    {
        try
        {
            var email = ((ClaimsIdentity)HttpContext.User.Identity)!.Claims
               .FirstOrDefault(s => s.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

            if (email is null || string.IsNullOrEmpty(email.Value))
            {
                return Redirect("/");
            }

            if (email.Value != "alkotunk@szentgellertkiado.hu" && email.Value != "arnoldgulyas@gmail.com" && email.Value != "szendike8@gmail.com")
            {
                return Redirect("/");
            }

            _partnerRepository.LogInUser(email.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Partner");
            return Redirect("/");
        }

        var tipusEdit = 58;
        Szoveg = "AJÁNDÉKOZZ KÖNYVET";

        if (tipus == 24)
        {
            tipusEdit = 24;
            Szoveg = "Szeretet-újság";
        }

        Datum = _iClock.Now.DateTime.ToString("MM") + "-";
        Cim = "";
        Idezet = "";

        if (!string.IsNullOrWhiteSpace(nid))
        {
            var res = _partnerRepository.GetTempContentByNid(nid);
            if (res is not null)
            {
                Datum = res.Fulldatum.ToString("yyyy-MM-dd HH:mm:ss");
                Cim = res.Cim;
                Idezet = res.Idezet;
            }
        }

        Items = _partnerRepository.GetTempContentByTipus(tipusEdit);
        Tipus = tipusEdit;

        return Page();
    }
}