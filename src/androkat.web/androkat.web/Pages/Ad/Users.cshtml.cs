using androkat.domain;
using androkat.domain.Model.AdminPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace androkat.web.Pages.Ad;

//[Authorize]
[BindProperties]
public class UsersModel : PageModel
{
    private readonly IAdminRepository _adminRepository;

    public UsersModel(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public List<AllUserResult> AllUserResult { get; set; }

    public void OnGet()
    {
        AllUserResult = _adminRepository.GetUsers();
    }
}