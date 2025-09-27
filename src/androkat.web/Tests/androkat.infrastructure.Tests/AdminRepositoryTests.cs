using androkat.application.Interfaces;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.domain.Model.WebResponse;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Model.SQLite;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.infrastructure.Tests;

public class AdminRepositoryTests : BaseTest
{
    [Fact]
    public void LoadPufferTodayContentByNid_Happy()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

        var loggerRepo = new Mock<ILogger<AdminRepository>>();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { GetContentMetaDataModel(Forras.mello) } });

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        var _fixture = new Fixture();
        var tempContent = _fixture.Create<TempContent>();
        tempContent.Nid = guid;
        tempContent.Tipus = 1;
        tempContent.Inserted = DateTime.Now;

        context.TempContent.Add(tempContent);
        context.SaveChanges();

        var repo = new AdminRepository(context, loggerRepo.Object, clock.Object, idezetData, null);
        var result = repo.LoadPufferTodayContentByNid(guid.ToString());
        result.Img.Should().Be(tempContent.Img);
        result.Inserted.Should().Contain(tempContent.Inserted.ToString("yyyy-MM-dd"));
    }

    [Fact]
    public void InsertError_Happy()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

        var loggerRepo = new Mock<ILogger<AdminRepository>>();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { GetContentMetaDataModel(Forras.mello) } });

        using var context = new AndrokatContext(GetDbContextOptions());

        var repo = new AdminRepository(context, loggerRepo.Object, clock.Object, idezetData, null);
        var result = repo.InsertError(new ErrorRequest
        {
            Error = "hiba"
        });
        result.Should().BeTrue();
    }

    [Fact]
    public void LogInUser_Happy()
    {
        var loggerRepo = new Mock<ILogger<AdminRepository>>();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { GetContentMetaDataModel(Forras.mello) } });

        using var context = new AndrokatContext(GetDbContextOptions());

        var repo = new AdminRepository(context, loggerRepo.Object, GetToday().Object, idezetData, null);
        var result = repo.LogInUser("aa@aa.hu");

        result.Should().BeTrue();
        context.Admin.FirstOrDefault().Email.Should().Be("aa@aa.hu");
        context.Admin.FirstOrDefault().LastLogin.ToString("yyyy-MM-ddTHH:mm:ss").Should().Be(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06");
    }

    [Fact]
    public void LogInUser_Exists_Happy()
    {
        var loggerRepo = new Mock<ILogger<AdminRepository>>();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { GetContentMetaDataModel(Forras.mello) } });

        using var context = new AndrokatContext(GetDbContextOptions());
        context.Admin.Add(new Admin { Email = "aa@aa.hu", Id = 1, LastLogin = GetToday().Object.Now.AddHours(-1).DateTime });
        context.SaveChanges();

        var repo = new AdminRepository(context, loggerRepo.Object, GetToday().Object, idezetData, null);
        var result = repo.LogInUser("aa@aa.hu");

        result.Should().BeTrue();
        context.Admin.FirstOrDefault().LastLogin.ToString("yyyy-MM-ddTHH:mm:ss").Should().Be(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06");
    }

    [Fact]
    public void LoadAllTodayResult_Happy()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

        var loggerRepo = new Mock<ILogger<AdminRepository>>();

        var idezetData = Options.Create(GetAndrokatConfiguration());

        using var context = new AndrokatContext(GetDbContextOptions());

        var guid = Guid.NewGuid();
        var _fixture = new Fixture();
        var tempContent = _fixture.Create<TempContent>();
        tempContent.Tipus = 6;
        tempContent.Fulldatum = DateTime.Now.ToString("yyyy") + "-11-02";
        tempContent.Nid = guid;

        context.TempContent.Add(tempContent);
        context.SaveChanges();

        var repo = new AdminRepository(context, loggerRepo.Object, clock.Object, idezetData, null);
        var result = repo.LoadAllTodayResult();

        result.ToList().FirstOrDefault(f => f.Tipus == 6).Tipus.Should().Be(6);
        result.ToList().FirstOrDefault(f => f.Tipus == 6).Datum.Should().Be(DateTime.Now.ToString("yyyy") + "-11-02");
        result.ToList().FirstOrDefault(f => f.Tipus == 6).TipusNev.Should().Be("Horváth");
        result.ToList().FirstOrDefault(f => f.Tipus == 6).Nid.Should().Be(guid.ToString());
        result.Count().Should().Be(20);

        var res = result.ToList().Where(f => f.Tipus != 6).Select(s => s.Datum);
        res.Any(a => !string.IsNullOrWhiteSpace(a)).Should().BeFalse();

        res = result.ToList().Where(f => f.Tipus != 6).Select(s => s.Nid);
        res.Any(a => !string.IsNullOrWhiteSpace(a)).Should().BeFalse();
    }

    private static AndrokatConfiguration GetAndrokatConfiguration()
    {
        return new AndrokatConfiguration
        {
            ContentMetaDataList = new List<ContentMetaDataModel>
            {
                GetContentMetaDataModel(Forras.audiohorvath, tipusNev: "Horváth"),
                GetContentMetaDataModel(Forras.fokolare),
                GetContentMetaDataModel(Forras.papaitwitter),
                GetContentMetaDataModel(Forras.advent),
                GetContentMetaDataModel(Forras.maievangelium),
                GetContentMetaDataModel(Forras.ignac),
                GetContentMetaDataModel(Forras.barsi),
                GetContentMetaDataModel(Forras.laciatya),
                GetContentMetaDataModel(Forras.horvath),
                GetContentMetaDataModel(Forras.prayasyougo),
                GetContentMetaDataModel(Forras.nagybojt),
                GetContentMetaDataModel(Forras.szeretetujsag),
                GetContentMetaDataModel(Forras.audionapievangelium),
                GetContentMetaDataModel(Forras.audiobarsi),
                GetContentMetaDataModel(Forras.audiopalferi),
                GetContentMetaDataModel(Forras.regnum),
                GetContentMetaDataModel(Forras.taize),
                GetContentMetaDataModel(Forras.szentbernat),
                GetContentMetaDataModel(Forras.ajanlatweb),
                GetContentMetaDataModel(Forras.audiotaize),
            }
        };
    }

    [Fact]
    public void GetNewsInfo_WithData_Happy()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

        var loggerRepo = new Mock<ILogger<AdminRepository>>();

        var idezetData = Options.Create(new AndrokatConfiguration
        {
            ContentMetaDataList = new List<ContentMetaDataModel>
            {
                GetContentMetaDataModel(Forras.keresztenyelet, tipusNev: "Keresztény élet"),
                GetContentMetaDataModel(Forras.kurir, tipusNev: "Kurír"),
                GetContentMetaDataModel(Forras.bonumtv, tipusNev: "Bonum TV")
            }
        });

        using var context = new AndrokatContext(GetDbContextOptions());

        var content1 = new Content
        {
            Cim = "kurir cím",
            Tipus = (int)Forras.kurir,
            Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03 04:05:06",
            Nid = Guid.NewGuid()
        };

        var content2 = new Content
        {
            Cim = "keresztény élet cím",
            Tipus = (int)Forras.keresztenyelet,
            Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03 04:05:06",
            Nid = Guid.NewGuid()
        };

        var content3 = new Content
        {
            Cim = "bonumtv cím",
            Tipus = (int)Forras.bonumtv,
            Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03 04:05:06",
            Nid = Guid.NewGuid()
        };

        context.Content.Add(content1);
        context.Content.Add(content2);
        context.Content.Add(content3);
        context.SaveChanges();

        var repo = new AdminRepository(context, loggerRepo.Object, clock.Object, idezetData, null);
        var result = repo.GetNewsInfo();

        result.Should().Contain("összes mai hírek: 3<br>");
        result.Should().Contain("Kurír (" + DateTime.Now.ToString("yyyy") + "-02-03 04:05:06)<br>");
        result.Should().Contain("Keresztény élet (" + DateTime.Now.ToString("yyyy") + "-02-03 04:05:06)<br>");
        result.Should().Contain("Bonum TV (" + DateTime.Now.ToString("yyyy") + "-02-03 04:05:06)<br>");
    }

    [Fact]
    public void GetNewsInfo_NoData_Happy()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

        var loggerRepo = new Mock<ILogger<AdminRepository>>();

        var idezetData = Options.Create(new AndrokatConfiguration
        {
            ContentMetaDataList = new List<ContentMetaDataModel>
            {
                GetContentMetaDataModel(Forras.keresztenyelet, tipusNev: "Keresztény élet"),
                GetContentMetaDataModel(Forras.kurir, tipusNev: "Kurír"),
                GetContentMetaDataModel(Forras.bonumtv, tipusNev: "Bonum TV")
            }
        });

        using var context = new AndrokatContext(GetDbContextOptions());

        var repo = new AdminRepository(context, loggerRepo.Object, clock.Object, idezetData, null);
        var result = repo.GetNewsInfo();

        result.Should().Be("<span style='color:red;'>nincs mai hír</span><br>");
    }

    [Fact]
    public void GetBlogInfo_WithData_Happy()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

        var loggerRepo = new Mock<ILogger<AdminRepository>>();

        var idezetData = Options.Create(new AndrokatConfiguration
        {
            ContentMetaDataList = new List<ContentMetaDataModel>
            {
                GetContentMetaDataModel(Forras.bzarandokma, tipusNev: "Buzgó zarándokma"),
                GetContentMetaDataModel(Forras.b777, tipusNev: "777"),
                GetContentMetaDataModel(Forras.jezsuitablog, tipusNev: "Jezsuita blog")
            }
        });

        using var context = new AndrokatContext(GetDbContextOptions());

        var content1 = new Content
        {
            Cim = "bzarandokma cím",
            Tipus = (int)Forras.bzarandokma,
            Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03 04:05:06",
            Nid = Guid.NewGuid()
        };

        var content2 = new Content
        {
            Cim = "b777 cím",
            Tipus = (int)Forras.b777,
            Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03 04:05:06",
            Nid = Guid.NewGuid()
        };

        var content3 = new Content
        {
            Cim = "jezsuitablog cím",
            Tipus = (int)Forras.jezsuitablog,
            Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03 04:05:06",
            Nid = Guid.NewGuid()
        };

        context.Content.Add(content1);
        context.Content.Add(content2);
        context.Content.Add(content3);
        context.SaveChanges();

        var repo = new AdminRepository(context, loggerRepo.Object, clock.Object, idezetData, null);
        var result = repo.GetBlogInfo();

        result.Should().Contain("összes mai blog: 3<br>");
        result.Should().Contain("Buzgó zarándokma (" + DateTime.Now.ToString("yyyy") + "-02-03 04:05:06)<br>");
        result.Should().Contain("777 (" + DateTime.Now.ToString("yyyy") + "-02-03 04:05:06)<br>");
        result.Should().Contain("Jezsuita blog (" + DateTime.Now.ToString("yyyy") + "-02-03 04:05:06)<br>");
    }

    [Fact]
    public void GetBlogInfo_NoData_Happy()
    {
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.Parse(DateTime.Now.ToString("yyyy") + "-02-03T04:05:06"));

        var loggerRepo = new Mock<ILogger<AdminRepository>>();

        var idezetData = Options.Create(new AndrokatConfiguration
        {
            ContentMetaDataList = new List<ContentMetaDataModel>
            {
                GetContentMetaDataModel(Forras.bzarandokma, tipusNev: "Buzgó zarándokma"),
                GetContentMetaDataModel(Forras.b777, tipusNev: "777"),
                GetContentMetaDataModel(Forras.jezsuitablog, tipusNev: "Jezsuita blog")
            }
        });

        using var context = new AndrokatContext(GetDbContextOptions());

        var repo = new AdminRepository(context, loggerRepo.Object, clock.Object, idezetData, null);
        var result = repo.GetBlogInfo();

        result.Should().Be("<span style='color:red;'>nincs mai blog</span><br>");
    }
}