using androkat.application.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace androkat.web.Pages;

public class RadioModel : PageModel
{
    private readonly IContentService _contentService;

    public RadioModel(IContentService contentService)
    {
        _contentService = contentService;
    }

    public IReadOnlyCollection<domain.Model.RadioModel> RadioViewModels { get; set; }

    public void OnGet()
    {
        RadioViewModels = _contentService.GetRadioPage();
    }
}