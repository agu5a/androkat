using androkat.application.Interfaces;
using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace androkat.web.Pages;

public class IndexModel : PageModel
{
    private readonly IContentService _contentService;

    public IndexModel(IContentService contentService)
    {
        _contentService = contentService;
    }

    public IReadOnlyCollection<ContentModel> ContentModels { get; set; }

    public void OnGet()
    {
        ContentModels = _contentService.GetHome();
    }
}