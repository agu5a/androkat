using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using androkat.maui.library.Models.Responses;
using androkat.maui.library.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace androkat.maui.unittest.Services;

public class DownloadServiceTests
{
    private readonly Mock<IAndrokatService> androkatService;
    private readonly Mock<IRepository> repository;
    private readonly Mock<IHelperSharedPreferences> helperSharedPreferences;
    private readonly Mock<ISourceData> sourceData;

    public DownloadServiceTests()
    {
        androkatService = new Mock<IAndrokatService>();
        repository = new Mock<IRepository>();
        helperSharedPreferences = new Mock<IHelperSharedPreferences>();
        sourceData = new Mock<ISourceData>();
    }

    [Fact]
    public async Task DownloadAll_All_Return_Test()
    {
        androkatService.Setup(s => s.GetContents(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
        [
            new() {
                Nid = Guid.NewGuid(),
                Cim = "cim",
                Idezet = "idezet",
                Image = "img",
                KulsoLink = string.Empty,
                Datum = DateTime.Now.AddDays(-1)}
        ]);

        androkatService.SetupSequence(s => s.GetImadsag(It.IsAny<DateTime>())).ReturnsAsync(new ImaResponse
        {
            HasMore = true,
            Imak =
            [
                new() {
                    RecordDate = DateTime.Now.AddDays(-1),
                    Content = "Content",
                    Title = "Title",
                    Csoport = 1,
                    Nid = Guid.NewGuid()
                }
            ]
        }).ReturnsAsync(new ImaResponse
        {
            HasMore = false,
            Imak =
            [
                new() {
                    RecordDate = DateTime.Now.AddDays(-1),
                    Content = "Content",
                    Title = "Title",
                    Csoport = 1,
                    Nid = Guid.NewGuid()
                }
            ]
        });

        repository.Setup(s => s.GetContentsByTypeName(It.IsAny<string>())).ReturnsAsync(default(ContentEntity));
        repository.Setup(s => s.GetContentsWithoutBook()).ReturnsAsync([new() { }]);

        helperSharedPreferences.Setup(s => s.GetSharedPreferencesBoolean(It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

        sourceData.Setup(s => s.GetSourcesFromMemory(It.IsAny<int>())).Returns(new SourceData { GroupName = "groupname" });

        var service = new DownloadService(androkatService.Object, repository.Object,
        helperSharedPreferences.Object, sourceData.Object);

        var res = await service.DownloadAll();

        Assert.Equal(42, res);
    }

    [Fact]
    public async Task StartUpdate_Fokolare_Already_Exists()
    {
        androkatService.Setup(s => s.GetContents(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
        [
            new() {
                Nid = Guid.NewGuid(),
                Cim = "cim",
                Idezet = "idezet",
                Image = "img",
                KulsoLink = string.Empty,
                Datum = DateTime.Now.AddDays(1)}
        ]);

        repository.Setup(s => s.GetContentsByTypeName(It.IsNotIn<string>(Activities.fokolare.ToString()))).ReturnsAsync(default(ContentEntity));
        repository.Setup(s => s.GetContentsByTypeName(It.IsIn<string>(Activities.fokolare.ToString())))
            .ReturnsAsync(GetContentEntity("7", Activities.fokolare.ToString()));
        repository.Setup(s => s.GetContentsWithoutBook()).ReturnsAsync([]);

        helperSharedPreferences.Setup(s => s.GetSharedPreferencesBoolean(It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

        sourceData.Setup(s => s.GetSourcesFromMemory(It.IsAny<int>())).Returns(new SourceData { GroupName = "groupname" });

        var service = new DownloadService(androkatService.Object, repository.Object,
        helperSharedPreferences.Object, sourceData.Object);

        var res = await service.StartUpdate(Activities.fokolare);

        Assert.Equal(0, res);
    }

    [Theory]
    [InlineData(Activities.papaitwitter, 0)]
    [InlineData(Activities.fokolare, 0)]
    [InlineData(Activities.kempis, 0)]
    [InlineData(Activities.maiszent, 1)]
    [InlineData(Activities.humor, 1)]
    [InlineData(Activities.audiohorvath, 0)]
    [InlineData(Activities.prayasyougo, 0)]
    [InlineData(Activities.audionapievangelium, 0)]
    [InlineData(Activities.audiobarsi, 0)]
    [InlineData(Activities.audiopalferi, 0)]
    [InlineData(Activities.audiotaize, 0)]
    [InlineData(Activities.book, 0)]
    [InlineData(Activities.ajanlatweb, 1)]
    [InlineData(Activities.kurir, 0)]
    [InlineData(Activities.bonumtv, 0)]
    [InlineData(Activities.b777, 0)]
    [InlineData(Activities.pio, 0)]
    [InlineData(Activities.kisterez, 0)]
    [InlineData(Activities.szentszalezi, 0)]
    [InlineData(Activities.sienaikatalin, 0)]
    public async Task StartUpdate_User_Dont_Download_Based_On_SharedPreferences(Activities activities, int expected)
    {
        androkatService.Setup(s => s.GetContents(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
        [
            new() {
                Nid = Guid.NewGuid(),
                Cim = "cim",
                Idezet = "idezet",
                Image = "img",
                KulsoLink = string.Empty,
                Datum = DateTime.Now.AddDays(1)}
        ]);

        repository.Setup(s => s.GetContentsByTypeName(It.IsAny<string>())).ReturnsAsync(default(ContentEntity));
        repository.Setup(s => s.GetContentsWithoutBook()).ReturnsAsync([]);

        helperSharedPreferences.Setup(s => s.GetSharedPreferencesBoolean(It.IsNotIn<string>(Activities.maiszent.ToString(),
            Activities.ajanlatweb.ToString(), Activities.humor.ToString()), true)).Returns(false);

        sourceData.Setup(s => s.GetSourcesFromMemory(It.IsAny<int>())).Returns(new SourceData { GroupName = "groupname" });

        var service = new DownloadService(androkatService.Object, repository.Object,
        helperSharedPreferences.Object, sourceData.Object);

        var res = await service.StartUpdate(activities);

        Assert.Equal(res, expected);
    }

    private static ContentEntity GetContentEntity(string tipus, string typeName)
    {
        return new ContentEntity
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