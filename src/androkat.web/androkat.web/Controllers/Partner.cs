using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Model;
using androkat.web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace androkat.web.Controllers;

[EnableRateLimiting("fixed-by-ip")]
[Route("partner")]
public class Partner : Controller
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IClock _iClock;

    public Partner(IClock iClock, IPartnerRepository partnerRepository)
    {
        _iClock = iClock;
        _partnerRepository = partnerRepository;
    }

    [Route("ad")]
    [HttpPost]
    public ActionResult<bool> Ad([FromForm] InsertData model)
    {
        var res = _partnerRepository.InsertTempContent(new ContentDetailsModel(Guid.NewGuid(),
            DateTime.Parse(_iClock.Now.DateTime.ToString("yyyy-") + model.Datum, CultureInfo.CreateSpecificCulture("hu-HU")),
            model.Cim, model.Idezet, model.Tipus,
        _iClock.Now.DateTime, string.Empty, string.Empty, string.Empty, string.Empty)
        );
        return Ok(res);
    }

    [Route("delete")]
    [HttpPost]
    public ActionResult<bool> Delete([FromForm] DeleteData model)
    {
        var res = _partnerRepository.DeleteTempContentByNid(model.Nid);
        return Ok(res);
    }

    [Route("logout")]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
}
