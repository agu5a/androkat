using androkat.application.Interfaces;
using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace androkat.web.Pages.Ima;

public class IndexModel : PageModel
{
    private readonly IContentService _contentService;

    public IndexModel(IContentService contentService)
    {
        _contentService = contentService;
    }

    [BindProperty(SupportsGet = true)]
    public string Csoport { get; set; }
    public Dictionary<string, string> ImaCsoportok { get; set; }

    public IReadOnlyCollection<ContentModel> ContentModels { get; set; }

    public void OnGet()
    {
        ImaCsoportok = new Dictionary<string, string> {{ "Alapimák", "11" }, {"Napi imák","9" }, {"Kérő és felajánló imák","12" },
            {"Hála és dicsőítő imák","7"}, {"Litániák","4"}, {"Szentmise","3"},
            {"Szűz Mária","10"}, {"Rózsafüzér","2"}, {"Szentek imái","1"},
            {"Zsoltár","0" }}; //5->saját ima az android appban

        ContentModels = _contentService.GetImaPage(Csoport);
    }
}