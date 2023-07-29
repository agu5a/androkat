using androkat.application.Interfaces;
using androkat.domain.Model.WebResponse;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace androkat.data.Controllers.V2;

[Route("v2")]
[ApiController]
public class Api : ControllerBase
{
    private readonly IApiService _apiService;
#pragma warning disable S4487 // Unread "private" fields should be removed
#pragma warning disable IDE0052 // Remove unread private members
    private readonly IMapper _mapper;
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning restore S4487 // Unread "private" fields should be removed

    public Api(IApiService apiService, IMapper mapper)
    {
        _apiService = apiService;
        _mapper = mapper;
    }

    /// <summary>
    /// android video
    /// </summary>
    [Route("video")]
    [HttpGet]
    [ProducesResponseType(200)]
    public ActionResult<List<VideoResponse>> GetVideoByOffsetV2(int offset)
    {
        if (offset < 0 || offset > 50)
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
}