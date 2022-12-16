using androkat.businesslayer.Service;
using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace androkat.core.Pages;

public class IndexModel : PageModel
{
    private readonly IMainPageService _mainPageService;

    public IndexModel(IMainPageService mainPageService)
    {
        _mainPageService = mainPageService;
    }

    public List<MainViewModel> MainViewModels { get; set; }

    public void OnGet()
    {
        MainViewModels = _mainPageService.GetHome();
    }
}