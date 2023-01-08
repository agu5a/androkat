using androkat.application.Interfaces;
using androkat.domain.Enum;
using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Web;

namespace androkat.web.Pages;

public class DetailsModel : PageModel
{
	private readonly IContentService _contentService;

	public DetailsModel(IContentService contentService)
	{
		_contentService = contentService;
	}

	[BindProperty(SupportsGet = true)]
	public int Type { get; set; }

	[BindProperty(SupportsGet = true)]
    public string Nid { get; set; }

	public ContentModel ContentModel { get; set; }
	public string Image { get; set; }
	public string ForrasLink { get; set; }
	public string ForrasText { get; set; }
	public string ShareUrl { get; set; }
	public string EncodedUrl { get; set; }
	public string ShareTitle { get; set; }

	public IActionResult OnGet()
	{
        if (string.IsNullOrWhiteSpace(Nid))
            return Redirect("/Error");

        if (!Guid.TryParse(Nid, out var guid))
            return Redirect("/Error");

        var result = _contentService.GetContentDetailsModelByNid(guid, Type);
        if (result == null || result.MetaData == null)
			return Redirect("/Error");

		ShareUrl = $"https://androkat.hu/details/{Nid}/{Type}";
		EncodedUrl = HttpUtility.UrlEncode(ShareUrl);
		ShareTitle = HttpUtility.UrlEncode(result.ContentDetails.Cim);

		ContentModel = result;

		if (!string.IsNullOrWhiteSpace(ContentModel.ContentDetails.Img))
		{
			Image = "https://androkat.hu/images/";
			Image += ContentModel.MetaData.TipusId == Forras.maiszent ? "szentek/" : "ajanlatok/";
			Image += ContentModel.ContentDetails.Img;
		}

		ForrasLink = string.IsNullOrWhiteSpace(ContentModel.ContentDetails.Forras) ? ContentModel.MetaData.Link : ContentModel.ContentDetails.Forras;
		ForrasText = string.IsNullOrWhiteSpace(ContentModel.ContentDetails.Forras) ? ContentModel.MetaData.Forras : ContentModel.ContentDetails.Forras;

		return Page();
	}
}