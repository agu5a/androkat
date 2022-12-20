using androkat.application.Interfaces;
using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace androkat.web.Pages;

public class AjanlatModel : PageModel
{
    private readonly IMainPageService _mainPageService;

    public AjanlatModel(IMainPageService mainPageService)
    {
        _mainPageService = mainPageService;
    }

    public IEnumerable<ContentModel> ContentModels { get; set; }

    public void OnGet()
    {
        ContentModels = _mainPageService.GetAjanlat();
    }
}