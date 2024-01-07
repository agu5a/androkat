using androkat.domain.Model.AdminPage;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Model.SQLite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;

namespace androkat.web.Pages.Ad;

//[Authorize]
public class VideoModel : PageModel
{
    private readonly ILogger<VideoModel> _logger;
    private readonly AndrokatContext _ctx;

    [BindProperty]
    public VideoData VideoPageData { get; set; }

    public VideoModel(ILogger<VideoModel> logger, AndrokatContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    public void OnGet()
    {
        VideoPageData = new VideoData();
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
            VideoPageData = new VideoData { Error = "siker�lt" };
        }
        catch (Exception ex)
        {
            VideoPageData = new VideoData { Error = ex.Message };
            _logger.LogError(ex, "Exception: ");
        }
    }
}
