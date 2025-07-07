using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.infrastructure.DataManager;
using androkat.infrastructure.Mapper;
using androkat.infrastructure.Model.SQLite;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using androkat.web.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace androkat.web.Tests.APITests;

public class CronTests : BaseTest
{
    [Theory]
    [InlineData((int)Forras.keresztenyelet, 1)]
    [InlineData(-1, 0)]
    public void GetKeresztenyelet(int tipus, int count)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var cache = GetIMemoryCache();
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var _fixture = new Fixture();
        var content = _fixture.Create<Content>();
        content.Cim = "cim";
        content.Tipus = tipus;
        content.Fulldatum = DateTime.Now.ToString("yyyy") + "-02-03";
        content.Inserted = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-02-03");

        context.Content.Add(content);
        context.SaveChanges();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var endPointConfig = Options.Create(new EndPointConfiguration
        {
            Cron = Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_CRON"),
            SaveContentDetailsModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_DETAILS_MODEL_API_URL"),
            SaveTempContentApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_API_URL"),
            UpdateRadioMusorModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_UPDATE_RADIO_MUSOR_API_URL"),
            HealthCheckApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_HEALTH_CHECK_API_URL"),
            GetContentsApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_GET_CONTENTS_API_URL")
        });
        var controller = new Cron(loggerRepo.Object, repository, new Mock<IAdminRepository>().Object, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object, endPointConfig);
        var resV1 = controller.GetKeresztenyelet();
        dynamic s2 = resV1.Result;
        List<string> result = s2.Value;
        result.Count.Should().Be(count);
        if (result.Count == 1)
        {
            result.First().Should().Be("cim");
        }
    }

    [Theory]
    [InlineData("2022-01-01", "2022-01-01", true)]
    [InlineData("2022-01-01", "2021-12-31", false)]
    public void HasNapiolvasoByDate(string date, string dbDate, bool expectedResult)
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var cache = GetIMemoryCache();
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var _fixture = new Fixture();
        var content = _fixture.Create<Content>();
        content.Fulldatum = dbDate;
        content.Tipus = 2;
        content.Fulldatum = dbDate.ToString();

        context.Content.Add(content);
        context.SaveChanges();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var endPointConfig = Options.Create(new EndPointConfiguration
        {
            Cron = Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_CRON"),
            SaveContentDetailsModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_DETAILS_MODEL_API_URL"),
            SaveTempContentApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_API_URL"),
            UpdateRadioMusorModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_UPDATE_RADIO_MUSOR_API_URL"),
            HealthCheckApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_HEALTH_CHECK_API_URL"),
            GetContentsApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_GET_CONTENTS_API_URL")
        });
        var controller = new Cron(loggerRepo.Object, repository, new Mock<IAdminRepository>().Object, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object, endPointConfig);
        var resV1 = controller.HasNapiolvasoByDate(2, date);
        dynamic s2 = resV1.Result;
        bool result = s2.Value;
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void GetSzentbernat()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var cache = GetIMemoryCache();
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var _fixture = new Fixture();
        var content = _fixture.Create<Content>();
        content.Fulldatum = "2022-12-06 00:00:01";
        content.Tipus = 50;

        context.Content.Add(content);
        context.SaveChanges();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var endPointConfig = Options.Create(new EndPointConfiguration
        {
            Cron = Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_CRON"),
            SaveContentDetailsModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_DETAILS_MODEL_API_URL"),
            SaveTempContentApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_API_URL"),
            UpdateRadioMusorModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_UPDATE_RADIO_MUSOR_API_URL"),
            HealthCheckApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_HEALTH_CHECK_API_URL"),
            GetContentsApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_GET_CONTENTS_API_URL")
        });
        var controller = new Cron(loggerRepo.Object, repository, new Mock<IAdminRepository>().Object, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object, endPointConfig);
        var resV1 = controller.GetSzentbernat();
        dynamic s2 = resV1.Result;
        DateTime result = s2.Value;
        result.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-06 00:00:01");
    }

    [Fact]
    public void GetSzentbernat_Nincs()
    {
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var controller = new Cron(loggerRepo.Object, new Mock<IApiRepository>().Object, new Mock<IAdminRepository>().Object, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object,
        Options.Create(new EndPointConfiguration
        {
            Cron = Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_CRON"),
            SaveContentDetailsModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_DETAILS_MODEL_API_URL"),
            SaveTempContentApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_API_URL"),
            UpdateRadioMusorModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_UPDATE_RADIO_MUSOR_API_URL"),
            HealthCheckApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_HEALTH_CHECK_API_URL"),
            GetContentsApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_GET_CONTENTS_API_URL")
        }));
        var resV1 = controller.GetSzentbernat();
        dynamic s2 = resV1.Result;
        DateTime result = s2.Value;
        result.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("0001-01-01 00:00:00");
    }

    [Fact]
    public void GetLastContentByTipus()
    {
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var cache = GetIMemoryCache();
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var _fixture = new Fixture();
        var content = _fixture.Create<Content>();
        content.Fulldatum = "2022-12-06 00:00:01";
        content.Tipus = (int)Forras.kurir;
        content.Inserted = DateTime.Parse("2022-12-06 00:00:01");

        context.Content.Add(content);
        context.SaveChanges();

        var idezetData = Options.Create(new AndrokatConfiguration { ContentMetaDataList = new List<ContentMetaDataModel> { } });
        var repository = new ApiRepository(context, clock.Object, mapper);
        var endPointConfig = Options.Create(new EndPointConfiguration
        {
            Cron = Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_CRON"),
            SaveContentDetailsModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_DETAILS_MODEL_API_URL"),
            SaveTempContentApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_API_URL"),
            UpdateRadioMusorModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_UPDATE_RADIO_MUSOR_API_URL"),
            HealthCheckApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_HEALTH_CHECK_API_URL"),
            GetContentsApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_GET_CONTENTS_API_URL")
        });
        var controller = new Cron(loggerRepo.Object, repository, new Mock<IAdminRepository>().Object, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object, endPointConfig);
        var resV1 = controller.GetLastContentByTipus((int)Forras.kurir);
        dynamic s2 = resV1.Result;
        DateTime result = s2.Value;
        result.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("2022-12-06 00:00:01");
    }

    [Fact]
    public void GetLastContentByTipus_Nincs()
    {
        var clock = new Mock<IClock>();
        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        using var context = new AndrokatContext(GetDbContextOptions());

        var controller = new Cron(loggerRepo.Object, new Mock<IApiRepository>().Object, new Mock<IAdminRepository>().Object, clock.Object, cronService.Object, new Mock<IWebHostEnvironment>().Object,
        Options.Create(new EndPointConfiguration
        {
            Cron = Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_CRON"),
            SaveContentDetailsModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_DETAILS_MODEL_API_URL"),
            SaveTempContentApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_SAVE_CONTENT_API_URL"),
            UpdateRadioMusorModelApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_UPDATE_RADIO_MUSOR_API_URL"),
            HealthCheckApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_HEALTH_CHECK_API_URL"),
            GetContentsApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_GET_CONTENTS_API_URL")
        }));
        var resV1 = controller.GetLastContentByTipus((int)Forras.kurir);
        dynamic s2 = resV1.Result;
        DateTime result = s2.Value;
        result.ToString("yyyy-MM-dd HH:mm:ss").Should().Be("0001-01-01 00:00:00");
    }

    [Theory]
    [InlineData(DayOfWeek.Saturday)]
    [InlineData(DayOfWeek.Sunday)]
    public void NapiUtravalo_Weekend_ShouldSkip(DayOfWeek dayOfWeek)
    {
        // Arrange
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        var testDate = new DateTimeOffset(2025, 6, dayOfWeek == DayOfWeek.Saturday ? 14 : 15, 10, 0, 0, TimeSpan.Zero); // Saturday or Sunday
        clock.Setup(c => c.Now).Returns(testDate);

        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();
        var webHostEnvironment = new Mock<IWebHostEnvironment>();
        webHostEnvironment.Setup(w => w.WebRootPath).Returns("/test/wwwroot");

        using var context = new AndrokatContext(GetDbContextOptions());
        var apiRepository = new ApiRepository(context, clock.Object, mapper);
        var adminRepository = new Mock<IAdminRepository>();

        var endPointConfig = Options.Create(new EndPointConfiguration
        {
            Cron = "testcron"
        });

        var controller = new Cron(loggerRepo.Object, apiRepository, adminRepository.Object, clock.Object, cronService.Object, webHostEnvironment.Object, endPointConfig);

        // Act
        var result = controller.NapiUtravalo();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().Be("Skipped - weekend");
    }

    [Fact]
    public void NapiUtravalo_NoMp3File_ShouldReturnNotFound()
    {
        // Arrange
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        var testDate = new DateTimeOffset(2025, 6, 17, 10, 0, 0, TimeSpan.Zero); // Tuesday
        clock.Setup(c => c.Now).Returns(testDate);

        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();
        var webHostEnvironment = new Mock<IWebHostEnvironment>();
        webHostEnvironment.Setup(w => w.WebRootPath).Returns("/nonexistent/path");

        using var context = new AndrokatContext(GetDbContextOptions());
        var apiRepository = new ApiRepository(context, clock.Object, mapper);
        var adminRepository = new Mock<IAdminRepository>();

        var endPointConfig = Options.Create(new EndPointConfiguration
        {
            Cron = "testcron"
        });

        var controller = new Cron(loggerRepo.Object, apiRepository, adminRepository.Object, clock.Object, cronService.Object, webHostEnvironment.Object, endPointConfig);

        // Act
        var result = controller.NapiUtravalo();

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Value.Should().Be("File not found: 06_17.mp3");
    }

    [Fact]
    public void NapiUtravalo_Mp3FileExists_NoDbRecord_ShouldAddRecord()
    {
        // Arrange
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        var testDate = new DateTimeOffset(2025, 6, 17, 10, 0, 0, TimeSpan.Zero); // Tuesday
        clock.Setup(c => c.Now).Returns(testDate);

        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        // Create a temporary directory and file for testing
        var tempDir = Path.Combine(Path.GetTempPath(), "androkat_test_" + Guid.NewGuid().ToString("N")[..8]);
        var downloadDir = Path.Combine(tempDir, "download");
        Directory.CreateDirectory(downloadDir);
        var testFilePath = Path.Combine(downloadDir, "06_17.mp3");
        System.IO.File.WriteAllText(testFilePath, "test audio content");

        var webHostEnvironment = new Mock<IWebHostEnvironment>();
        webHostEnvironment.Setup(w => w.WebRootPath).Returns(tempDir);

        using var context = new AndrokatContext(GetDbContextOptions());
        var apiRepository = new ApiRepository(context, clock.Object, mapper);

        var adminRepository = new Mock<IAdminRepository>();
        var mockLastTodayResult = new androkat.domain.Model.AdminPage.LastTodayResult
        {
            Cim = "Test Evangelium (Napi Ige)"
        };
        adminRepository.Setup(a => a.GetLastTodayContentByTipus((int)Forras.maievangelium))
                      .Returns(mockLastTodayResult);

        var endPointConfig = Options.Create(new EndPointConfiguration
        {
            Cron = "testcron"
        });

        var controller = new Cron(loggerRepo.Object, apiRepository, adminRepository.Object, clock.Object, cronService.Object, webHostEnvironment.Object, endPointConfig);

        try
        {
            // Act
            var result = controller.NapiUtravalo();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be("File exists, DB record added for: 06_17.mp3");
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    [Fact]
    public void NapiUtravalo_Mp3FileExists_DbRecordExists_ShouldReturnConflict()
    {
        // Arrange
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var config = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = "<License Key Here>";
            cfg.AddProfile<AutoMapperProfile>();
        }, loggerFactory);
        var mapper = config.CreateMapper();

        var clock = new Mock<IClock>();
        var testDate = new DateTimeOffset(2025, 6, 17, 10, 0, 0, TimeSpan.Zero); // Tuesday
        clock.Setup(c => c.Now).Returns(testDate);

        var loggerRepo = new Mock<ILogger<Cron>>();
        var cronService = new Mock<ICronService>();

        // Create a temporary directory and file for testing
        var tempDir = Path.Combine(Path.GetTempPath(), "androkat_test_" + Guid.NewGuid().ToString("N")[..8]);
        var downloadDir = Path.Combine(tempDir, "download");
        Directory.CreateDirectory(downloadDir);
        var testFilePath = Path.Combine(downloadDir, "06_17.mp3");
        System.IO.File.WriteAllText(testFilePath, "test audio content");

        var webHostEnvironment = new Mock<IWebHostEnvironment>();
        webHostEnvironment.Setup(w => w.WebRootPath).Returns(tempDir);

        using var context = new AndrokatContext(GetDbContextOptions());

        // Add existing content record with tipus 15 for the test date
        var _fixture = new Fixture();
        var existingContent = _fixture.Create<Content>();
        existingContent.Tipus = 15;
        existingContent.Fulldatum = "2025-06-17";
        existingContent.Inserted = DateTime.Parse("2025-06-17 10:00:00");
        context.Content.Add(existingContent);
        context.SaveChanges();

        var apiRepository = new ApiRepository(context, clock.Object, mapper);
        var adminRepository = new Mock<IAdminRepository>();

        var endPointConfig = Options.Create(new EndPointConfiguration
        {
            Cron = "testcron"
        });

        var controller = new Cron(loggerRepo.Object, apiRepository, adminRepository.Object, clock.Object, cronService.Object, webHostEnvironment.Object, endPointConfig);

        try
        {
            // Act
            var result = controller.NapiUtravalo();

            // Assert
            result.Should().BeOfType<ConflictObjectResult>();
            var conflictResult = result as ConflictObjectResult;
            conflictResult.Value.Should().Be("File and DB record already exist for: 06_17.mp3");
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}
