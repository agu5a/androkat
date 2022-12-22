using androkat.application.Interfaces;
using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace androkat.web.Pages;

public class SzentekModel : PageModel
{
    private readonly IContentService _contentService;

    public SzentekModel(IContentService contentService)
    {
        _contentService = contentService;
    }

    public IEnumerable<ContentModel> ContentModels { get; set; }

    public void OnGet()
    {
        ContentModels = _contentService.GetSzentek();
    }
}