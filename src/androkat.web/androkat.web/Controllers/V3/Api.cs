using androkat.application.Interfaces;
using androkat.domain.Model.WebResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Collections.Generic;

namespace androkat.web.Controllers.V3;

[EnableRateLimiting("fixed-by-ip")]
[Route("v3")]
[ApiController]
public class Api : ControllerBase
{
    private readonly IApiService _apiService;

    public Api(IApiService apiService)
    {
        _apiService = apiService;
    }

    [Route("contents")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContentResponse>), 200)]
    public ActionResult<IEnumerable<ContentResponse>> GetContentsByTipusAndId(int tipus, string id)
    {
        if (tipus is < 1 or > 100)
            return BadRequest("Hiba");

        var guid = Guid.Empty;
        if (!string.IsNullOrWhiteSpace(id) && !Guid.TryParse(id, out guid))
            return BadRequest("Hiba");

        var result = _apiService.GetContentByTipusAndId(tipus, guid, default, default);
        return Ok(result);
    }
}