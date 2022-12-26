using androkat.application.Interfaces;
using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace androkat.web.Pages;

public class VideoModel : PageModel
{
    private readonly IContentService _contentService;

    public VideoModel(IContentService contentService)
    {
        _contentService = contentService;
    }

    [BindProperty(SupportsGet = true)]
    public string F { get; set; }

    public IReadOnlyCollection<VideoSourceModel> VideoSourceViewModels { get; set; }

    public void OnGet()
    {
        ViewData["source"] = F ?? string.Empty;
        VideoSourceViewModels = _contentService.GetVideoSourcePage();
    }
}