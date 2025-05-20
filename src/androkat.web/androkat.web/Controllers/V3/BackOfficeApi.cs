using androkat.domain;
using androkat.domain.Model.WebResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace androkat.web.Controllers.V3;

[EnableRateLimiting("fixed-by-ip")]
[Route("v3")]
[ApiController]
public class BackOfficeApi : ControllerBase
{
    private readonly IAdminRepository _apiService;

    public BackOfficeApi(IAdminRepository apiService)
    {
        _apiService = apiService;
    }

    [Route("errors")]
    [HttpPost]
    [ProducesResponseType(typeof(bool), 200)]
    public ActionResult<bool> PostError([FromBody] ErrorRequest error)
    {
        if (error is null || string.IsNullOrWhiteSpace(error.Error))
        {
            return BadRequest("Hiba");
        }

        var result = _apiService.InsertError(error);
        return Ok(result);
    }
}