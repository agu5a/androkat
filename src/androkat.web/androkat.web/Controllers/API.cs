using androkat.application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    /// web page hivja a video fullhoz
    /// </summary>
    [Route("api/video")]
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