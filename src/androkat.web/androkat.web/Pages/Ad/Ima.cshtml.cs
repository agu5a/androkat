using androkat.application.Interfaces;
using androkat.domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace androkat.web.Pages.Ad;

[Authorize]
[BindProperties]
public class ImaModel : PageModel
{
	private readonly IClock _iClock;
	private readonly IAdminRepository _adminRepository;

	public ImaModel(IClock iClock, IAdminRepository adminRepository)
	{
		_iClock = iClock;
		_adminRepository = adminRepository;
	}

	public string Cim { get; set; }
	public string Idezet { get; set; }
	public string Csoport { get; set; }
	public string Error { get; set; }

	public void OnGet()
	{
		//nothing here
	}

	public void OnPost()
	{
		if (string.IsNullOrWhiteSpace(Cim) || string.IsNullOrWhiteSpace(Idezet) || string.IsNullOrWhiteSpace(Csoport))
		{
			Error = "valami hiányzik";
			return;
		}

		var res = _adminRepository.InsertIma(new domain.Model.ImaModel(Guid.NewGuid(), _iClock.Now.Date, Cim, Csoport, Idezet));

		Error = res ? "siker" : "vmi rossz volt";
	}
}