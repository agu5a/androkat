using androkat.application.Interfaces;
using androkat.domain.Enum;
using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace androkat.web.Pages;

public class HirekModel : PageModel
{
    private readonly IContentService _contentService;

    public HirekModel(IContentService contentService)
    {
        _contentService = contentService;
    }

    [BindProperty(SupportsGet = true)]
    public string Source { get; set; }

    public IReadOnlyCollection<ContentModel> ContentModels { get; set; }

    public void OnGet()
    {
        ViewData["source"] = Source;

        int tipus = Source switch
        {
            "keresztenyelet" => (int)Forras.keresztenyelet,
            "kurir" => (int)Forras.kurir,
            "bonumtv" => (int)Forras.bonumtv,
            _ => 0,
        };

        ContentModels = _contentService.GetHirek(tipus);
    }
}