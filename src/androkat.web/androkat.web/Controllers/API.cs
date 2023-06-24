using androkat.application.Interfaces;
using androkat.domain.Model.WebResponse;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace androkat.data.Controllers;

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
    [Route("api/video")]
    [HttpPost]
    [ProducesResponseType(200)]
    public ActionResult<List<VideoResponse>> GetVideoByOffset([FromForm] int offset)
    {
        if (offset < 0 || offset > 50)
            return BadRequest("Hiba");

        var result = _apiService.GetVideoByOffset(offset, default);
        return Ok(result);
    }

    /// <summary>
    /// android radio
    /// </summary>
    [Route("api/radio")]
    [HttpPost]
    [ProducesResponseType(200)]
    public ActionResult<IEnumerable<RadioMusorResponse>> GetRadioBySource([FromForm] string s)
    {
        if (string.IsNullOrWhiteSpace(s) || (s.ToLower() != "katolikushu" && s.ToLower() != "mariaradio" && s.ToLower() != "szentistvan"))
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
        if (offset < 0 || offset > 50)
            return BadRequest("Hiba");

        if (!string.IsNullOrWhiteSpace(f) && (f.Length > 30 || f.Length < 20))
            return BadRequest("Hiba");

        var sb = _apiService.GetVideoForWebPage(f, offset, default);
        return Ok(sb);
    }
}