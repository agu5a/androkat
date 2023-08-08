using androkat.application.Interfaces;
using androkat.domain.Enum;
using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace androkat.web.Pages;

public class BlogModel : PageModel
{
    private readonly IContentService _contentService;

    public BlogModel(IContentService contentService)
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
            "bzarandokma" => (int)Forras.bzarandokma,
            "b777" => (int)Forras.b777,
            "jezsuitablog" => (int)Forras.jezsuitablog,
            _ => 0,
        };

        ContentModels = _contentService.GetBlog(tipus);
    }
}