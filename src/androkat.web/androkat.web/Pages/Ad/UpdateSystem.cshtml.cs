using androkat.domain;
using androkat.domain.Model.AdminPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Pages.Ad;

[Authorize]
[BindProperties]
public class UpdateSystemModel : PageModel
{
	private readonly ILogger<UpdateSystemModel> _logger;
	private readonly IAdminRepository _adminRepository;

	public UpdateSystemModel(ILogger<UpdateSystemModel> logger, IAdminRepository adminRepository)
	{
		_logger = logger;
		_adminRepository = adminRepository;
	}

	public string Value { get; set; }
	public string Key { get; set; }
	public string Nid { get; set; }
	public string Error { get; set; }
	public List<SelectListItem> AllRecordResult { get; set; }

	public void OnGet()
	{
		try
		{
			var all = _adminRepository.GetAllSystemInfo().ToList();
			AllRecordResult = all.Select(s => new SelectListItem { Text = s.Key, Value = s.Id.ToString() }).ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception: ");
		}
	}

	public void OnPostSearch()
	{
		try
		{
			var all = _adminRepository.GetAllSystemInfo().ToList();
			AllRecordResult = all.Select(s => new SelectListItem { Text = s.Key, Value = s.Id.ToString() }).ToList();

			if (string.IsNullOrWhiteSpace(Nid))
			{
				return;
			}

			var id = int.Parse(Nid);
			var obj = all.Find(f => f.Id == id);
			Value = obj.Value;
			Key = obj.Key;
			Nid = obj.Id.ToString();
		}
		catch (Exception ex)
		{
			Error = ex.Message;
			_logger.LogError(ex, "Exception: ");
		}
	}

	public void OnPostSave()
	{
		if (string.IsNullOrWhiteSpace(Nid) || string.IsNullOrWhiteSpace(Value))
        {
            return;
        }

        var all = _adminRepository.GetAllSystemInfo().ToList();
		AllRecordResult = all.Select(s => new SelectListItem { Text = s.Key, Value = s.Id.ToString() }).ToList();

		var res = _adminRepository.UpdateSystemInfo(new SystemInfoData { Id = int.Parse(Nid), Value = Value });

		Error = res ? "siker" : "vmi rossz volt";
	}	
}