using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Models;
using androkat.maui.library.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace androkat.maui.unittest.Services;

public class DownloadServiceTests
{
    private Mock<IAndrokatService> androkatService;
    private Mock<IRepository> repository;
    private Mock<IHelperSharedPreferences> helperSharedPreferences;
    private Mock<ISourceData> sourceData;

    public DownloadServiceTests()
    {
        androkatService = new Mock<IAndrokatService>();
        repository = new Mock<IRepository>();
        helperSharedPreferences = new Mock<IHelperSharedPreferences>();
        sourceData = new Mock<ISourceData>();
    }

    [Test]
    public async Task DownloadAll_All_Return_Test()
    {
        androkatService.Setup(s => s.GetContents(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<ContentResponse>
        {
            new ContentResponse{
                Nid = Guid.NewGuid(), 
                Cim = "cim", 
                Idezet = "idezet", 
                Image = "img", 
                KulsoLink = string.Empty,
                Datum = DateTime.Now.AddDays(1)}
        });

        repository.Setup(s => s.GetContentsByTypeName(It.IsAny<string>())).ReturnsAsync(default(ContentDto));
        repository.Setup(s => s.GetContentsWithoutBook()).ReturnsAsync(new List<ContentDto> { });

        helperSharedPreferences.Setup(s => s.getSharedPreferencesBoolean(It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

        sourceData.Setup(s => s.GetSourcesFromMemory(It.IsAny<int>())).Returns(new SourceData { GroupName = "groupname" });

        var service = new DownloadService(androkatService.Object, repository.Object,
        helperSharedPreferences.Object, sourceData.Object);

        var res = await service.DownloadAll();

        Assert.That(res, Is.EqualTo(40));
    }

    [Test]
    public async Task StartUpdate_Fokolare_Already_Exists()
    {
        androkatService.Setup(s => s.GetContents(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<ContentResponse>
        {
            new ContentResponse{
                Nid = Guid.NewGuid(),
                Cim = "cim",
                Idezet = "idezet",
                Image = "img",
                KulsoLink = string.Empty,
                Datum = DateTime.Now.AddDays(1)}
        });

        repository.Setup(s => s.GetContentsByTypeName(It.IsNotIn<string>(Activities.fokolare.ToString()))).ReturnsAsync(default(ContentDto));
        repository.Setup(s => s.GetContentsByTypeName(It.IsIn<string>(Activities.fokolare.ToString())))
            .ReturnsAsync(GetContentDto("7", Activities.fokolare.ToString()));
        repository.Setup(s => s.GetContentsWithoutBook()).ReturnsAsync(new List<ContentDto> { });

        helperSharedPreferences.Setup(s => s.getSharedPreferencesBoolean(It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

        sourceData.Setup(s => s.GetSourcesFromMemory(It.IsAny<int>())).Returns(new SourceData { GroupName = "groupname" });

        var service = new DownloadService(androkatService.Object, repository.Object,
        helperSharedPreferences.Object, sourceData.Object);

        var res = await service.StartUpdate(Activities.fokolare);

        Assert.That(res, Is.EqualTo(0));
    }

    [TestCase(Activities.papaitwitter, 0)]
    [TestCase(Activities.fokolare, 0)]
    [TestCase(Activities.kempis, 0)]
    [TestCase(Activities.maiszent, 1)]
    [TestCase(Activities.humor, 1)]
    [TestCase(Activities.audiohorvath, 0)]
    [TestCase(Activities.prayasyougo, 0)]
    [TestCase(Activities.audionapievangelium, 0)]
    [TestCase(Activities.audiobarsi, 0)]
    [TestCase(Activities.audiopalferi, 0)]
    [TestCase(Activities.audiotaize, 0)]
    [TestCase(Activities.book, 0)]
    [TestCase(Activities.ajanlatweb, 1)]
    [TestCase(Activities.kurir, 0)]
    [TestCase(Activities.bonumtv, 0)]
    [TestCase(Activities.b777, 0)]
    [TestCase(Activities.pio, 0)]
    [TestCase(Activities.kisterez, 0)]
    [TestCase(Activities.szentszalezi, 0)]
    [TestCase(Activities.sienaikatalin, 0)]
    public async Task StartUpdate_User_Dont_Download(Activities activities, int expected)
    {
        androkatService.Setup(s => s.GetContents(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<ContentResponse>
        {
            new ContentResponse{
                Nid = Guid.NewGuid(),
                Cim = "cim",
                Idezet = "idezet",
                Image = "img",
                KulsoLink = string.Empty,
                Datum = DateTime.Now.AddDays(1)}
        });

        repository.Setup(s => s.GetContentsByTypeName(It.IsAny<string>())).ReturnsAsync(default(ContentDto));
        repository.Setup(s => s.GetContentsWithoutBook()).ReturnsAsync(new List<ContentDto> { });

        helperSharedPreferences.Setup(s => s.getSharedPreferencesBoolean(It.IsNotIn<string>(Activities.maiszent.ToString(),
            Activities.ajanlatweb.ToString(), Activities.humor.ToString()), true)).Returns(false);

        sourceData.Setup(s => s.GetSourcesFromMemory(It.IsAny<int>())).Returns(new SourceData { GroupName = "groupname" });

        var service = new DownloadService(androkatService.Object, repository.Object,
        helperSharedPreferences.Object, sourceData.Object);

        var res = await service.StartUpdate(activities);

        Assert.That(res, Is.EqualTo(expected));
    }

    private static ContentDto GetContentDto(string tipus, string typeName)
    {
        return new ContentDto
        {
            Cim = "cim",
            Datum = DateTime.Now,
            GroupName = "groupname",
            Idezet = "idezet",
            Forras = "forras",
            Nid = Guid.NewGuid(),
            Tipus = tipus,
            TypeName = typeName,
            Image = "img",
            KulsoLink = "kulsolink"
        };
    }
}