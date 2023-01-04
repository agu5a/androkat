using androkat.application.Interfaces;
using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Web;

namespace androkat.web.Pages.Ima;

public class DetailsModel : PageModel
{
    private readonly IContentService _contentService;

    public DetailsModel(IContentService contentService)
    {
        _contentService = contentService;
    }

    [BindProperty(SupportsGet = true)]
    public Guid Nid { get; set; }

    public ImaModel ImaModel { get; set; }
    public string ShareUrl { get; set; }
    public string EncodedUrl { get; set; }
    public string ShareTitle { get; set; }
    public Dictionary<string, string> ImaCsoportok { get; set; }

    public IActionResult OnGet()
    {
        var result = _contentService.GetImaById(Nid);
        if (result == null)
            return Redirect("/Error");

        ImaCsoportok = new Dictionary<string, string> {{ "Alapimák", "11" }, {"Napi imák","9" }, {"Kérő és felajánló imák","12" },
            {"Hála és dicsőítő imák","7"}, {"Litániák","4"}, {"Szentmise","3"},
            {"Szűz Mária","10"}, {"Rózsafüzér","2"}, {"Szentek imái","1"},
            {"Zsoltár","0" }}; //5->saját ima az android appban

        ShareUrl = $"https://androkat.hu/ima/details/{Nid}";
        EncodedUrl = HttpUtility.UrlEncode(ShareUrl);
        ShareTitle = HttpUtility.UrlEncode(result.Cim);

        ImaModel = result;

        return Page();
    }
}