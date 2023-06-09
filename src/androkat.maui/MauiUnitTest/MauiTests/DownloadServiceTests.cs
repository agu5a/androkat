using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTest.MauiTests;

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
        /*androkatService.Setup(s => s.GetEgyebOlvasnivalo(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new List<EgyebOlvasnivalo>
        {
            new EgyebOlvasnivalo{ nid = Guid.NewGuid(), cim = "cim", kulsolink = "kulsolink", leiras = "leiras", time = "2023-01-01"}
        });
        androkatService.Setup(s => s.GetNapiElmelkedes(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<NapiElmelkedes>
        {
            new NapiElmelkedes{ nid = Guid.NewGuid(), cim = "cim", idezet = "idezet", img = "img", datum = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")}
        });

        repository.Setup(s => s.GetByTypeName(It.IsAny<string>())).ReturnsAsync(default(NapiElmelkedesDto));
        repository.Setup(s => s.GetWithoutBook()).ReturnsAsync(new List<NapiElmelkedesDto> { });

        helperSharedPreferences.Setup(s => s.getSharedPreferencesBoolean(It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

        sourceData.Setup(s => s.GetSourcesFromMemory(It.IsAny<int>())).Returns(new SourceData { GroupName = "groupname" });

        var service = new DownloadService(androkatService.Object, repository.Object,
        helperSharedPreferences.Object, sourceData.Object);

        var res = await service.DownloadAll();

        Assert.That(res, Is.EqualTo(21));*/
    }

    [Test]
    public async Task StartUpdate_Fokolare_Already_Exists()
    {
        /*androkatService.Setup(s => s.GetEgyebOlvasnivalo(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new List<EgyebOlvasnivalo>
        {
            new EgyebOlvasnivalo{ nid = Guid.NewGuid(), cim = "cim", kulsolink = "kulsolink", leiras = "leiras", time = "2023-01-01"}
        });
        androkatService.Setup(s => s.GetNapiElmelkedes(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<NapiElmelkedes>
        {
            new NapiElmelkedes{ nid = Guid.NewGuid(), cim = "cim", idezet = "idezet", img = "img", datum = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")}
        });

        repository.Setup(s => s.GetByTypeName(It.IsNotIn<string>(Activities.fokolare.ToString()))).ReturnsAsync(default(NapiElmelkedesDto));
        repository.Setup(s => s.GetByTypeName(It.IsIn<string>(Activities.fokolare.ToString())))
            .ReturnsAsync(GetNapiElmelkedesDto("7", Activities.fokolare.ToString()));
        repository.Setup(s => s.GetWithoutBook()).ReturnsAsync(new List<NapiElmelkedesDto> { });

        helperSharedPreferences.Setup(s => s.getSharedPreferencesBoolean(It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

        sourceData.Setup(s => s.GetSourcesFromMemory(It.IsAny<int>())).Returns(new SourceData { GroupName = "groupname" });

        var service = new DownloadService(androkatService.Object, repository.Object,
        helperSharedPreferences.Object, sourceData.Object);

        var res = await service.StartUpdate(Activities.fokolare);

        Assert.That(res, Is.EqualTo(0));*/
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
        /*androkatService.Setup(s => s.GetEgyebOlvasnivalo(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(new List<EgyebOlvasnivalo>
        {
            new EgyebOlvasnivalo{ nid = Guid.NewGuid(), cim = "cim", kulsolink = "kulsolink", leiras = "leiras", time = "2023-01-01"}
        });
        androkatService.Setup(s => s.GetNapiElmelkedes(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<NapiElmelkedes>
        {
            new NapiElmelkedes{ nid = Guid.NewGuid(), cim = "cim", idezet = "idezet", img = "img", datum = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")}
        });

        repository.Setup(s => s.GetByTypeName(It.IsAny<string>())).ReturnsAsync(default(NapiElmelkedesDto));
        repository.Setup(s => s.GetWithoutBook()).ReturnsAsync(new List<NapiElmelkedesDto> { });

        helperSharedPreferences.Setup(s => s.getSharedPreferencesBoolean(It.IsNotIn<string>(Activities.maiszent.ToString(),
            Activities.ajanlatweb.ToString(), Activities.humor.ToString()), true)).Returns(false);

        sourceData.Setup(s => s.GetSourcesFromMemory(It.IsAny<int>())).Returns(new SourceData { GroupName = "groupname" });

        var service = new DownloadService(androkatService.Object, repository.Object,
        helperSharedPreferences.Object, sourceData.Object);

        var res = await service.StartUpdate(activities);

        Assert.That(res, Is.EqualTo(expected));*/
    }

    /*private NapiElmelkedesDto GetNapiElmelkedesDto(string tipus, string typeName)
    {
        return new NapiElmelkedesDto
        {
            cim = "cim",
            datum = DateTime.Now,
            GroupName = "groupname",
            idezet = "idezet",
            forras = "forras",
            nid = Guid.NewGuid(),
            tipus = tipus,
            TypeName = typeName,
            img = "img",
            KulsoLink = "kulsolink"
        };
    }*/
}