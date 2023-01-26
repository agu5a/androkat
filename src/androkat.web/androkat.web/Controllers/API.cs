using androkat.application.Interfaces;
using androkat.domain.Model.WebResponse;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace androkat.data.Controllers;

[ApiController]
public class Api : ControllerBase
{
    private readonly IApiService _apiService;
    private readonly IMapper _mapper;

    public Api(IApiService apiService, IMapper mapper)
    {
        _apiService = apiService;
        _mapper = mapper;
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
}