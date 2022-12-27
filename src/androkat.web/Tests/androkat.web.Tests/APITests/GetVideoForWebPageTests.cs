using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace androkat.web.Tests.APITests;

public class GetVideoForWebPageTests : BaseTest
{
    [TestCase("UCRn003-qzC5GOQVPyJSkgnA", -1)]
    [TestCase("UCRn003-qzC5GOQVPyJSkgnA", 51)]
    public void API_GetVideoForWebPage_V1_BadRequest(string f, int offset)
    {
        var apiV1 = new data.Controllers.Api();
        var resV1 = apiV1.GetVideoForWebPage(f, offset);
        dynamic s = resV1.Result;
        string result = s.Value;
        result.Should().Be("Hiba");
    }

    [Test]
    public void API_GetVideoForWebPage_V1()
    {
        var apiV1 = new data.Controllers.Api();
        ActionResult<string> resV1 = apiV1.GetVideoForWebPage("", 0);
        dynamic sV1 = resV1.Result;

        var actual = sV1.Value.ToString();
        Assert.That(actual, Is.EqualTo("<div class='col mb-1'><div class=\"videoBox p-3 border bg-light\" style=\"border-radius: 0.25rem;\"><h5><a href=\"https://www.youtube.com/watch?v=aLQ9Ed89z-A\" target =\"_blank\">AndroKat új verzió: Olvasottak elrejthetősége a lista oldalon</a></h5><div><strong>Forrás</strong>: AndroKat</div><div>2022-07-31</div><div class=\"video-container\" ><iframe src=\"https://www.youtube.com/embed/aLQ9Ed89z-A\" width =\"310\" height =\"233\" title=\"YouTube video player\" frameborder =\"0\" allow=\"accelerometer;\"></iframe></div></div></div>"));
    }
}