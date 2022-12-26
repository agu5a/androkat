using androkat.domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace androkat.data.Controllers;

[ApiController]
public class Api : ControllerBase
{
    [Route("api/video")]
    [HttpPost]
    [ProducesResponseType(200)]
    public ActionResult<string> GetVideoForWebPage([FromForm] string f, [FromForm] int offset)
    {
        if (offset < 0 || offset > 50)
            return BadRequest("Hiba");

        return Ok(GetVideo(f, offset));
    }

    public static string GetVideo(string f, int offset)
    {
        StringBuilder sb = new StringBuilder();

        var res = new List<VideoModel>
        {
            new VideoModel
            {
                Cim = "AndroKat új verzió: Olvasottak elrejthetősége a lista oldalon",
                Date = "2022-07-31",
                Forras = "AndroKat",
                VideoLink = "https://www.youtube.com/watch?v=aLQ9Ed89z-A"
            }
        };

        foreach (var item in res.OrderByDescending(o => o.Date).Skip(offset).Take(4))
        {
            GetVideoHtml(sb, item);
        }

        return sb.ToString();
    }

    private static void GetVideoHtml(StringBuilder sb, VideoModel item)
    {
        sb.Append("<div class='col mb-1'>");
        sb.Append("<div class=\"videoBox p-3 border bg-light\" style=\"border-radius: 0.25rem;\"><h5><a href=\"" + item.VideoLink + "\" target =\"_blank\">" + item.Cim + "</a></h5>");
        sb.Append("<div><strong>Forrás</strong>: " + item.Forras + "</div>");
        sb.Append("<div>" + item.Date + "</div>");
        sb.Append("<div class=\"video-container\" ><iframe src=\"" + item.VideoLink.Replace("watch?v=", "embed/") + "\" width =\"310\" height =\"233\" title=\"YouTube video player\" " +
            "frameborder =\"0\" allow=\"accelerometer;\"></iframe></div>");
        sb.Append("</div></div>");
    }
}