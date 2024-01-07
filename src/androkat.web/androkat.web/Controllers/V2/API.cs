using System;
using System.Collections.Generic;
using System.Globalization;
using androkat.application.Interfaces;
using androkat.domain.Model.WebResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace androkat.web.Controllers.V2;

[EnableRateLimiting("fixed-by-ip")]
[Route("v2")]
[ApiController]
public class Api : ControllerBase
{
    private readonly IApiService _apiService;

    public Api(IApiService apiService)
    {
        _apiService = apiService;
    }

    /// <summary>
    /// android video
    /// </summary>
    [Route("video")]
    [HttpGet]
    [ProducesResponseType(200)]
    public ActionResult<List<VideoResponse>> GetVideoByOffsetV2(int offset)
    {
        if (offset is < 0 or > 50)
            return BadRequest("Hiba");

        var result = _apiService.GetVideoByOffset(offset, default);
        return Ok(result);
    }

    /// <summary>
    /// android ima
    /// </summary>
    [Route("ima")]
    [HttpGet]
    [ProducesResponseType(typeof(ImaResponse), 200)]
    public ActionResult<ImaResponse> GetImaByDateV2(string date)
    {
        if (string.IsNullOrWhiteSpace(date) || !DateTime.TryParse(date, CultureInfo.CreateSpecificCulture("hu-HU"), out _))
            return BadRequest("Hiba");

        var result = _apiService.GetImaByDate(date, default);
        return Ok(result);
    }

    /// <summary>
    /// android radio url, verziók, website url
    /// </summary>
    [Route("ser")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SystemDataResponse>), 200)]
    public ActionResult<IEnumerable<SystemDataResponse>> GetSystemDataV2()
    {
        IEnumerable<SystemDataResponse> result = _apiService.GetSystemData(default);
        return Ok(result);
    }

    /// <summary>
    /// android radio
    /// </summary>
    [Route("radio")]
    [HttpGet]
    [ProducesResponseType(200)]
    public ActionResult<IEnumerable<RadioMusorResponse>> GetRadioBySourceV2(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return BadRequest("Hiba");

        s = s.ToLower();
        if (s != "katolikushu" && s != "mariaradio" && s != "szentistvan")
            return BadRequest("Hiba");

        var result = _apiService.GetRadioBySource(s, default);
        return Ok(result);
    }  

    /// <summary>
    /// web page hivja a video fullhoz
    /// </summary>
    [Route("video")]
    [HttpPost]
    [ProducesResponseType(200)]
    public ActionResult<string> GetVideoForWebPage([FromForm] string f, [FromForm] int offset)
    {
        if (offset is < 0 or > 50)
            return BadRequest("Hiba");

        if (!string.IsNullOrWhiteSpace(f) && f.Length is > 30 or < 20)
            return BadRequest("Hiba");

        var sb = _apiService.GetVideoForWebPage(f, offset, default);
        return Ok(sb);
    }  
}