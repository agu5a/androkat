using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using androkat.application.Contants;
using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;

namespace androkat.web.Controllers;

[EnableRateLimiting("fixed-by-ip")]
[Route(ContantValues.Cron)]
[ApiController]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S6960:Controllers should not have mixed responsibilities", Justification = "<Pending>")]
public class Cron : ControllerBase
{
	private readonly ILogger<Cron> _logger;
	private readonly IApiRepository _apiRepository;
	private readonly IClock _iClock;
	private readonly ICronService _cronService;

	public Cron(ILogger<Cron> logger,
		IApiRepository apiRepository,
		IClock iClock,
		ICronService cronService)
	{
		_logger = logger;
		_apiRepository = apiRepository;
		_iClock = iClock;
		_cronService = cronService;
	}

	[Route("cron")]
	[HttpGet]
	public ActionResult RunCron()
	{
		_cronService.Start();
		_cronService.DeleteCaches();
		return Ok();
	}

	//csak update Db-ben
	[Route("vatikanradio")]
	[HttpGet]
	public ActionResult VatikanRadio()
	{
		try
		{
			var yesterday = _iClock.Now.DateTime.AddDays(-1).ToString("ddMMyy");
			var res = _apiRepository.GetSystemInfoModels().Where(w => w.Key == "radio").Select(s => s.Value).First();
			var web = JsonSerializer.Deserialize<Dictionary<string, string>>(res)!;
			var newDic = web.ToDictionary(item => item.Key, item => item.Key == "vatikan" ? $"https://media.vaticannews.va/media/audio/program/1900/ungherese_{yesterday}.mp3" : item.Value);
			_apiRepository.UpdateRadioSystemInfo(JsonSerializer.Serialize(newDic));
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception: Failed to run {Name}", nameof(VatikanRadio));
			return BadRequest($"failed to run {nameof(VatikanRadio)}");
		}
	}

	[Route("getKeresztenyelet")]
	[HttpGet]
    public ActionResult<List<string>> GetKeresztenyelet()
	{
		try
		{
			var inDb = _apiRepository.GetContentDetailsModels().Where(w => w.Tipus == (int)Forras.keresztenyelet).Select(s => s.Cim).ToList();
			return Ok(inDb);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception: Failed to run GetKeresztenyelet");
			return BadRequest($"failed to run {nameof(GetKeresztenyelet)}");
		}
	}

	[Route("getLastContentByTipus")]
	[HttpGet]
	public ActionResult<DateTime> GetLastContentByTipus(int tipus)
	{
		try
		{
			var inDb = _apiRepository.GetContentDetailsModels().Where(w => w.Tipus == tipus).OrderByDescending(o => o.Fulldatum).Select(s => s.Fulldatum).FirstOrDefault();
			return Ok(inDb);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception: Failed to run GetLastContentByTipus");
			return BadRequest($"failed to run {nameof(GetLastContentByTipus)}");
		}
	}

	[Route("getSzentbernat")]
	[HttpGet]
	public ActionResult<DateTime> GetSzentbernat()
	{
		try
		{
			var inDb = _apiRepository.GetContentDetailsModels().Where(w => w.Tipus == 50).OrderByDescending(o => o.Fulldatum).Select(s => s.Fulldatum).FirstOrDefault();
			return Ok(inDb);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception: Failed to run GetSzentbernat");
			return BadRequest($"failed to run {nameof(GetSzentbernat)}");
		}
	}

	[Route("hasNapiolvasoByDate")]
	[HttpGet]
	public ActionResult<bool> HasNapiolvasoByDate(int tipus, string date)
	{
		try
		{
			var exist = _apiRepository.GetContentDetailsModels().FirstOrDefault(w => w.Tipus == tipus && w.Fulldatum.ToString("yyyy-MM-dd").StartsWith(date));
			return Ok(exist is not null);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception: Failed to run HasNapiolvasoByDate");
			return BadRequest($"failed to run {nameof(HasNapiolvasoByDate)}");
		}
	}

	[Route("deleteCaches")]
	[HttpPost]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public ActionResult DeleteCaches(CacheFromMegaApp cacheFromMegaApp)
	{
		_cronService.DeleteCaches();
		return Ok();
	}

	[Route("savevideo")]
	[HttpPost]
	public ActionResult SaveVideo(VideoModel videoModel)
	{
		try
		{
			if (!_apiRepository.AddVideo(videoModel))
            {
				return Conflict();
            }

			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception: Failed to run savevideo");
			return BadRequest($"failed to run {nameof(SaveVideo)}");
		}
	}	
}