using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Models.Entities;
using androkat.maui.library.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace androkat.maui.unittest.Services;

public class PageServiceTests
{
    private readonly IServiceScopeFactory _factory;
    private readonly Mock<ISourceData> _sourceDataMock = new();
    private readonly Mock<IAndrokatService> _androkatServiceMock = new();
    private readonly Mock<IDownloadService> _downloadServiceMock = new();

    public PageServiceTests()
    {
        var services = new ServiceCollection();
        services.AddScoped<IRepository, Repository>();
        services.AddScoped<DownloadService>();
        services.AddScoped<ISourceData, SourceDataMapper>();
        services.AddSingleton<IAndrokatService, AndrokatService>();

        var serviceProvider = services.BuildServiceProvider();
        _factory = serviceProvider.GetService<IServiceScopeFactory>();
    }

    [Fact]
    public async Task Test_GetContentEntityByIdAsync()
    {
        var repositoryMock = new Mock<IRepository>();
        var expectedContent = new ContentEntity
        {
            Nid = Guid.NewGuid(),
            Cim = "Test Cim",
            Idezet = "Test Idezet"
        };
        repositoryMock.Setup(repo => repo.GetContentById(It.IsAny<Guid>())).ReturnsAsync(expectedContent).Verifiable();

        var pageService = new PageService(_downloadServiceMock.Object, repositoryMock.Object);

        var result = await pageService.GetContentEntityByIdAsync(expectedContent.Nid);

        repositoryMock.Verify(repo => repo.GetContentById(expectedContent.Nid), Times.Once);
        Assert.Equal(expectedContent, result);
    }

    [Fact]
    public async Task DownloadAll_ReturnsCorrectCount()
    {
        // Arrange
        _downloadServiceMock
            .Setup(dl => dl.DownloadAll())
            .ReturnsAsync(5);

        var repositoryMock = new Mock<IRepository>();
        var pageService = new PageService(_downloadServiceMock.Object, repositoryMock.Object);

        // Act
        var result = await pageService.DownloadAll();

        // Assert
        Assert.Equal(5, result);
        _downloadServiceMock.Verify(dl => dl.DownloadAll(), Times.Once);
    }

    [Fact]
    public async Task Test_InsertFavoriteContentAsync()
    {
        var repositoryMock = new Mock<IRepository>();
        var favoriteContentEntity = new FavoriteContentEntity
        {
            Nid = Guid.NewGuid(),
            Cim = "Test Cim",
            Idezet = "Test Idezet"
        };
        var expectedCount = 1;
        repositoryMock.Setup(repo => repo.InsertFavoriteContent(It.IsAny<FavoriteContentEntity>())).ReturnsAsync(expectedCount).Verifiable();
        var pageService = new PageService(_downloadServiceMock.Object, repositoryMock.Object);

        var result = await pageService.InsertFavoriteContentAsync(favoriteContentEntity);

        repositoryMock.Verify(repo => repo.InsertFavoriteContent(favoriteContentEntity), Times.Once);
        Assert.Equal(expectedCount, result);
    }

    [Fact]
    public async Task Test_GetFavoriteContentsAsync()
    {
        var repositoryMock = new Mock<IRepository>();
        var expectedContents = new List<FavoriteContentEntity>
            {
                new() {Nid = Guid.NewGuid(), Cim = "Test Cim 1", Idezet = "Test Idezet 1", Image = "Image1.png"},
                new() {Nid = Guid.NewGuid(), Cim = "Test Cim 2", Idezet = "Test Idezet 2", Image = "Image2.png"},
                new() {Nid = Guid.NewGuid(), Cim = "Test Cim 3", Idezet = "Test Idezet 3", Image = "Image3.png"}
            };
        repositoryMock.Setup(repo => repo.GetFavoriteContents()).ReturnsAsync(expectedContents).Verifiable();
        var pageService = new PageService(_downloadServiceMock.Object, repositoryMock.Object);

        var result = await pageService.GetFavoriteContentsAsync();

        repositoryMock.Verify(repo => repo.GetFavoriteContents(), Times.Once);
        Assert.Equal(expectedContents, result);
    }

    [Fact]
    public async Task Test_GetImaContents()
    {
        var repositoryMock = new Mock<IRepository>();
        var expectedContents = new List<ImadsagEntity>
            {
                new() {Nid = Guid.NewGuid(), Cim = "Test Cim 1", Content = "Test Content 1", Csoport = 1 },
                new() {Nid = Guid.NewGuid(), Cim = "Test Cim 2", Content = "Test Content 2", Csoport = 2 },
                new() {Nid = Guid.NewGuid(), Cim = "Test Cim 3", Content = "Test Content 3", Csoport = 3 }
            };
        repositoryMock.Setup(repo => repo.GetImaContents(1, 10)).ReturnsAsync(expectedContents).Verifiable();
        var pageService = new PageService(_downloadServiceMock.Object, repositoryMock.Object);

        var result = await pageService.GetImaContents(1, 10);

        repositoryMock.Verify(repo => repo.GetImaContents(1, 10), Times.Once);
        Assert.Equal(expectedContents, result);
    }

    [Fact]
    public async Task Test_GetFavoriteCountAsync()
    {
        var repositoryMock = new Mock<IRepository>();
        var expectedCount = 5;
        repositoryMock.Setup(repo => repo.GetFavoriteCount()).ReturnsAsync(expectedCount).Verifiable();
        var pageService = new PageService(_downloadServiceMock.Object, repositoryMock.Object);

        var result = await pageService.GetFavoriteCountAsync();

        repositoryMock.Verify(repo => repo.GetFavoriteCount(), Times.Once);
        Assert.Equal(expectedCount, result);
    }

    [Theory]
    [InlineData("1", nameof(IRepository.GetAjanlatokContents))]
    [InlineData("2", nameof(IRepository.GetMaiszentContents))]
    [InlineData("3", nameof(IRepository.GetSzentekContents))]
    [InlineData("4", nameof(IRepository.GetNewsContents))]
    [InlineData("5", nameof(IRepository.GetBlogContents))]
    [InlineData("6", nameof(IRepository.GetHumorContents))]
    [InlineData("8", nameof(IRepository.GetAudioContents))]
    [InlineData("11", nameof(IRepository.GetBookContents))]
    [InlineData("0", nameof(IRepository.GetContents))]
    public async Task Test_GetContentsAsync(string pageTypeId, string methodName)
    {
        var repositoryMock = new Mock<IRepository>();
        var expectedContents = Activator.CreateInstance(typeof(List<ContentEntity>)) as List<ContentEntity>;
        repositoryMock.Setup(repo => repo.GetAjanlatokContents()).ReturnsAsync(expectedContents).Verifiable();
        repositoryMock.Setup(repo => repo.GetMaiszentContents()).ReturnsAsync(expectedContents).Verifiable();
        repositoryMock.Setup(repo => repo.GetSzentekContents()).ReturnsAsync(expectedContents).Verifiable();
        repositoryMock.Setup(repo => repo.GetNewsContents()).ReturnsAsync(expectedContents).Verifiable();
        repositoryMock.Setup(repo => repo.GetBlogContents()).ReturnsAsync(expectedContents).Verifiable();
        repositoryMock.Setup(repo => repo.GetHumorContents()).ReturnsAsync(expectedContents).Verifiable();
        repositoryMock.Setup(repo => repo.GetAudioContents()).ReturnsAsync(expectedContents).Verifiable();
        repositoryMock.Setup(repo => repo.GetBookContents()).ReturnsAsync(expectedContents).Verifiable();
        repositoryMock.Setup(repo => repo.GetContents()).ReturnsAsync(expectedContents).Verifiable();
        var pageService = new PageService(_downloadServiceMock.Object, repositoryMock.Object);

        var result = await pageService.GetContentsAsync(pageTypeId);

        switch (methodName)
        {
            case "GetAjanlatokContents":
                repositoryMock.Verify(x => x.GetAjanlatokContents(), Times.Once);
                break;
            case "GetMaiszentContents":
                repositoryMock.Verify(x => x.GetMaiszentContents(), Times.Once);
                break;
            case "GetSzentekContents":
                repositoryMock.Verify(x => x.GetSzentekContents(), Times.Once);
                break;
            case "GetNewsContents":
                repositoryMock.Verify(x => x.GetNewsContents(), Times.Once);
                break;
            case "GetBlogContents":
                repositoryMock.Verify(x => x.GetBlogContents(), Times.Once);
                break;
            case "GetHumorContents":
                repositoryMock.Verify(x => x.GetHumorContents(), Times.Once);
                break;
            case "GetAudioContents":
                repositoryMock.Verify(x => x.GetAudioContents(), Times.Once);
                break;
            case "GetBookContents":
                repositoryMock.Verify(x => x.GetBookContents(), Times.Once);
                break;
            case "GetElmelkedesContents":
                repositoryMock.Verify(x => x.GetContents(), Times.Once);
                break;
        };

        Assert.IsType<List<ContentEntity>>(result);
        Assert.Equal(expectedContents, result);
    }

    [Fact]
    public async Task Should_Delete_All_Content_And_Ima()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository>();
        repositoryMock.Setup(repo => repo.DeleteAllContent()).ReturnsAsync(5).Verifiable();
        repositoryMock.Setup(repo => repo.DeleteAllImadsag()).ReturnsAsync(40).Verifiable();

        var pageService = new PageService(_downloadServiceMock.Object, repositoryMock.Object);

        // Act
        var result = await pageService.DeleteAllContentAndIma();

        // Assert
        repositoryMock.Verify(x => x.DeleteAllContent(), Times.Once);
        repositoryMock.Verify(x => x.DeleteAllImadsag(), Times.Once);
        Assert.Equal(45, result);
    }
}
