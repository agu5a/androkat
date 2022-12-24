using androkat.application.Interfaces;
using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace androkat.web.Pages;

public class ImaModel : PageModel
{
    private readonly IContentService _contentService;

    public ImaModel(IContentService contentService)
    {
        _contentService = contentService;
    }

    [BindProperty(SupportsGet = true)]
    public string Csoport { get; set; }

    public IReadOnlyCollection<ContentModel> ContentModels { get; set; }

    public void OnGet()
    {
        ViewData["csoport"] = Csoport;

        ContentModels = _contentService.GetImaPage(Csoport);
    }
}