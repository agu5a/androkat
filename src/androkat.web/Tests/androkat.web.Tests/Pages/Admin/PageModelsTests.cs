using androkat.domain;
using androkat.domain.Model.AdminPage;
using androkat.web.Pages.Ad;
using androkat.web.ViewModels;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace androkat.web.Tests.Pages.Admin;

public class PageModelsTests : BaseTest
{
    [Fact]
    public async Task LogoutModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var _logger = new Mock<ILogger<LogoutModel>>();

        var model = new LogoutModel(_logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        dynamic res = await model.OnGet();
        string url = res.Url;
        url.Should().Be("/");
    }

    [Fact]
    public void UsersModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();
        var _fixture = new Fixture();
        var users = _fixture.Create<List<AllUserResult>>();
        var adminRepository = new Mock<IAdminRepository>();
        adminRepository.Setup(s => s.GetUsers()).Returns(users);

        var model = new UsersModel(adminRepository.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        (model.AllUserResult).Should().Contain(users);
    }

    [Fact]
    public void UpdateRadioModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();

        var _fixture = new Fixture();
        var allRecordResult = _fixture.Create<List<AllRecordResult>>();

        allRecordResult.First().Csoport = "csoport";

        var _logger = new Mock<ILogger<UpdateRadioModel>>();
        var adminRepository = new Mock<IAdminRepository>();
        adminRepository.Setup(s => s.GetAllRadioResult()).Returns(allRecordResult);

        var model = new UpdateRadioModel(_logger.Object, adminRepository.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        model.OnGet();
        (model.AllRecordResult.First().Text).Should().Be("csoport");

        model.OnPostSearch();
        (model.AllRecordResult.First().Text).Should().Be("csoport");

        model.OnPostSave();
        (model.AllRecordResult.First().Text).Should().Be("csoport");

        model.OnPostDelete();
        (model.AllRecordResult.First().Text).Should().Be("csoport");
    }

    [Fact]
    public async Task UploadModelTest()
    {
        var (pageContext, tempData, actionContext) = GetPreStuff();
        var _fixture = new Fixture();
        var users = _fixture.Create<List<AllUserResult>>();
        var _logger = new Mock<ILogger<UploadModel>>();
        var adminRepository = new Mock<IAdminRepository>();
        adminRepository.Setup(s => s.GetUsers()).Returns(users);

        var model = new UploadModel(_logger.Object)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext),
            FileNameReplace = "yt5s.com - igazi fájl név (128 kbps).mp3"
        };
        model.OnPostReplace();
        (model.Result).Should().Contain("igazi_fajl_nev.mp3");

        model.FileUpload2 = new BufferedSingleFileUploadPhysical
        {
            FormFile = GetFormFile().Object
        };

        var onPostUploadImagesAsync = await model.OnPostUploadImagesAsync();
        (model.Result).Should().Contain("hiba");
        (model.ModelState.Values.First().Errors[0].ErrorMessage).Should().Be("(test.jpeg) is empty.");
        (model.ModelState.Values.First().Errors[1].ErrorMessage).Should().Be("(test.jpeg) file type isn't permitted or the file's signature doesn't match the file's extension.");

        model.OnPostReplace();
        (model.Result).Should().Be("igazi_fajl_nev.mp3");

        model.FileUpload1 = new BufferedSingleFileUploadPhysical
        {
            FormFile = GetFormFile().Object
        };

        var onPostUploadAsync = await model.OnPostUploadAsync();
        (model.Result).Should().Be("hiba");
        (model.ModelState.Values.First().Errors[0].ErrorMessage).Should().Be("(test.jpeg) is empty.");
        (model.ModelState.Values.First().Errors[1].ErrorMessage).Should().Be("(test.jpeg) file type isn't permitted or the file's signature doesn't match the file's extension.");
    }

    private static Mock<IFormFile> GetFormFile()
    {
        var fileMock = new Mock<IFormFile>();
        var content = "Hello World from a Fake File";
        var fileName = "test.jpeg";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Name).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);

        return fileMock;
    }
}