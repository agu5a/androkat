using androkat.domain.Model.AdminPage;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Model.SQLite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;

namespace androkat.web.Pages.Ad;

[Authorize()]
public class VideoModel : PageModel
{
    protected readonly ILogger<VideoModel> _logger;
    protected readonly AndrokatContext _ctx;

    [BindProperty]
    public VideoPageData VideoPageData { get; set; }

    public VideoModel(ILogger<VideoModel> logger, AndrokatContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    public void OnGet()
    {
        VideoPageData = new VideoPageData();
    }

    public void OnPost()
    {
        try
        {
            _ctx.VideoContent.Add(new VideoContent
            {
                Cim = VideoPageData.Cim,
                Img = VideoPageData.Img,
                VideoLink = VideoPageData.Idezet,
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                Forras = VideoPageData.Forras,
                ChannelId = VideoPageData.ChannelId,
                Inserted = DateTime.Now
            });
            _ctx.SaveChanges();
            VideoPageData.Error = "sikerült";
        }
        catch (Exception ex)
        {
            VideoPageData.Error = ex.Message;
            _logger.LogError(ex, "Exception: ");
        }

        VideoPageData = new VideoPageData();
    }
}
