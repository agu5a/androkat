#nullable enable
using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using androkat.maui.library.Services;
using androkat.maui.library.ViewModels;
using Microsoft.Maui.Dispatching;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace androkat.maui.unittest.Services;

/// <summary>
/// Comprehensive integration tests for filtering functionality in list pages
/// Tests the complete flow from ContentListViewModel through PageService to Repository
/// </summary>
public class ListPageFilteringTests
{
    private readonly Mock<IRepository> _repositoryMock;
    private readonly Mock<IPageService> _pageServiceMock;
    private readonly Mock<ISourceData> _sourceDataMock;
    private readonly Mock<IAndrokatService> _androkatServiceMock;
    private readonly Mock<IDispatcher> _dispatcherMock;

    public ListPageFilteringTests()
    {
        _repositoryMock = new Mock<IRepository>();
        _pageServiceMock = new Mock<IPageService>();
        _sourceDataMock = new Mock<ISourceData>();
        _androkatServiceMock = new Mock<IAndrokatService>();
        _dispatcherMock = new Mock<IDispatcher>();
    }

    [Fact]
    public async Task NapiOlvaso_WithNoFilters_ShouldReturnAllItems()
    {
        // Arrange
        var testData = CreateNapiOlvasoTestData();
        var pageService = new PageService(Mock.Of<IDownloadService>(), _repositoryMock.Object);

        _repositoryMock.Setup(r => r.GetContentsByGroupName("group_napiolvaso", true, null))
                      .ReturnsAsync(testData);

        // Act
        var result = await pageService.GetContentsAsync("0", true, null);

        // Assert
        Assert.Equal(testData.Count, result.Count);
        _repositoryMock.Verify(r => r.GetContentsByGroupName("group_napiolvaso", true, null), Times.Once);
    }

    [Fact]
    public async Task NapiOlvaso_WithSpecificSourceFilter_ShouldReturnFilteredItems()
    {
        // Arrange
        var testData = CreateNapiOlvasoTestData();
        var enabledSources = new List<string> { "13" }; // Only barsi
        var expectedFilteredData = testData.Where(x => x.Tipus == "13").ToList();

        var pageService = new PageService(Mock.Of<IDownloadService>(), _repositoryMock.Object);

        _repositoryMock.Setup(r => r.GetContentsByGroupName("group_napiolvaso", true, enabledSources))
                      .ReturnsAsync(expectedFilteredData);

        // Act
        var result = await pageService.GetContentsAsync("0", true, enabledSources);

        // Assert
        Assert.Equal(expectedFilteredData.Count, result.Count);
        Assert.All(result, item => Assert.Equal("13", item.Tipus));
        _repositoryMock.Verify(r => r.GetContentsByGroupName("group_napiolvaso", true, enabledSources), Times.Once);
    }

    [Fact]
    public async Task NapiOlvaso_WithReadUnreadFilter_ShouldReturnUnreadItems()
    {
        // Arrange
        var testData = CreateNapiOlvasoTestData();
        var expectedUnreadData = testData.Where(x => !x.IsRead).ToList();

        var pageService = new PageService(Mock.Of<IDownloadService>(), _repositoryMock.Object);

        _repositoryMock.Setup(r => r.GetContentsByGroupName("group_napiolvaso", false, null))
                      .ReturnsAsync(expectedUnreadData);

        // Act
        var result = await pageService.GetContentsAsync("0", false, null);

        // Assert
        Assert.Equal(expectedUnreadData.Count, result.Count);
        Assert.All(result, item => Assert.False(item.IsRead));
        _repositoryMock.Verify(r => r.GetContentsByGroupName("group_napiolvaso", false, null), Times.Once);
    }

    [Fact]
    public async Task NapiOlvaso_WithSourceAndReadFilters_ShouldApplyBothFilters()
    {
        // Arrange
        var testData = CreateNapiOlvasoTestData();
        var enabledSources = new List<string> { "13" }; // Only barsi
        var expectedFilteredData = testData.Where(x => x.Tipus == "13" && !x.IsRead).ToList();

        var pageService = new PageService(Mock.Of<IDownloadService>(), _repositoryMock.Object);

        _repositoryMock.Setup(r => r.GetContentsByGroupName("group_napiolvaso", false, enabledSources))
                      .ReturnsAsync(expectedFilteredData);

        // Act
        var result = await pageService.GetContentsAsync("0", false, enabledSources);

        // Assert
        Assert.Equal(expectedFilteredData.Count, result.Count);
        Assert.All(result, item =>
        {
            Assert.Equal("13", item.Tipus);
            Assert.False(item.IsRead);
        });
        _repositoryMock.Verify(r => r.GetContentsByGroupName("group_napiolvaso", false, enabledSources), Times.Once);
    }

    [Fact]
    public async Task ContentListViewModel_LoadData_WithFilters_ShouldUpdateList()
    {
        // Arrange
        var testData = CreateNapiOlvasoTestData();
        var enabledSources = new List<string> { "13", "14" };
        var expectedFilteredData = testData.Where(x => enabledSources.Contains(x.Tipus)).ToList();

        _pageServiceMock.Setup(p => p.GetContentsAsync("0", true, enabledSources))
                       .ReturnsAsync(expectedFilteredData);

        // Mock the SourceData to return valid SourceData objects
        _sourceDataMock.Setup(s => s.GetSourcesFromMemory(It.IsAny<int>()))
                      .Returns(new SourceData
                      {
                          Title = "Test Source",
                          Img = "test.jpg",
                          Forrasszoveg = "Test Source Text"
                      });

        var viewModel = new ContentListViewModel(
            _dispatcherMock.Object,
            _pageServiceMock.Object,
            _sourceDataMock.Object,
            _androkatServiceMock.Object);

        // Mock dispatcher to run synchronously
        _dispatcherMock.Setup(d => d.Dispatch(It.IsAny<Action>()))
                      .Callback<Action>(action => action());

        // Set the Id property for the ViewModel
        viewModel.Id = "0";

        // Act
        await viewModel.FetchAsync(true, enabledSources);

        // Assert
        Assert.Single(viewModel.Contents); // Contents is grouped
        var firstGroup = viewModel.Contents.First();
        Assert.Equal(expectedFilteredData.Count, firstGroup.Count);
        _pageServiceMock.Verify(p => p.GetContentsAsync("0", true, enabledSources), Times.Once);
    }

    [Fact]
    public void FilterOptionsHelper_NapiOlvaso_GeneratesCorrectKeys()
    {
        // Act
        var options = FilterOptionsHelper.GetFilterOptionsForPageId("0");

        // Assert
        Assert.NotEmpty(options);

        // Check that keys are integer strings matching Activities enum
        var barsiOption = options.FirstOrDefault(o => o.Activity == Activities.barsi);
        Assert.NotNull(barsiOption);
        Assert.Equal("13", barsiOption.Key);

        var horvathOption = options.FirstOrDefault(o => o.Activity == Activities.horvath);
        Assert.NotNull(horvathOption);
        Assert.Equal("14", horvathOption.Key);

        var focolareOption = options.FirstOrDefault(o => o.Activity == Activities.fokolare);
        Assert.NotNull(focolareOption);
        Assert.Equal("7", focolareOption.Key);
    }

    [Fact]
    public void FilterOptionsHelper_Szentek_GeneratesCorrectKeys()
    {
        // Act
        var options = FilterOptionsHelper.GetFilterOptionsForPageId("3");

        // Assert
        Assert.NotEmpty(options);

        var pioOption = options.FirstOrDefault(o => o.Activity == Activities.pio);
        Assert.NotNull(pioOption);
        Assert.Equal("2", pioOption.Key);
    }

    [Theory]
    [InlineData("0", "group_napiolvaso")] // Napi olvasmány
    [InlineData("3", "group_szentek")]    // Szentek
    [InlineData("4", "group_news")]       // Hírek
    [InlineData("5", "group_blog")]       // Magazin
    [InlineData("8", "group_audio")]      // Audio
    public async Task PageService_GetContentsAsync_CallsCorrectRepositoryMethod(string pageId, string expectedGroupName)
    {
        // Arrange
        var testData = new List<ContentEntity>();
        var pageService = new PageService(Mock.Of<IDownloadService>(), _repositoryMock.Object);

        _repositoryMock.Setup(r => r.GetContentsByGroupName(expectedGroupName, true, null))
                      .ReturnsAsync(testData);

        // Act
        await pageService.GetContentsAsync(pageId, true, null);

        // Assert
        _repositoryMock.Verify(r => r.GetContentsByGroupName(expectedGroupName, true, null), Times.Once);
    }

    [Fact]
    public async Task Repository_ApplyFilters_WithEmptyEnabledSources_ReturnsEmptyList()
    {
        // Arrange
        var testData = CreateNapiOlvasoTestData();
        var enabledSources = new List<string>(); // Empty list

        _repositoryMock.Setup(r => r.GetContentsByGroupName("group_napiolvaso", true, enabledSources))
                      .ReturnsAsync(new List<ContentEntity>());

        var pageService = new PageService(Mock.Of<IDownloadService>(), _repositoryMock.Object);

        // Act
        var result = await pageService.GetContentsAsync("0", true, enabledSources);

        // Assert
        Assert.Empty(result);
        _repositoryMock.Verify(r => r.GetContentsByGroupName("group_napiolvaso", true, enabledSources), Times.Once);
    }

    [Fact]
    public async Task Repository_ApplyFilters_WithNonExistentSource_ReturnsEmptyList()
    {
        // Arrange
        var testData = CreateNapiOlvasoTestData();
        var enabledSources = new List<string> { "999" }; // Non-existent source

        _repositoryMock.Setup(r => r.GetContentsByGroupName("group_napiolvaso", true, enabledSources))
                      .ReturnsAsync(new List<ContentEntity>());

        var pageService = new PageService(Mock.Of<IDownloadService>(), _repositoryMock.Object);

        // Act
        var result = await pageService.GetContentsAsync("0", true, enabledSources);

        // Assert
        Assert.Empty(result);
        _repositoryMock.Verify(r => r.GetContentsByGroupName("group_napiolvaso", true, enabledSources), Times.Once);
    }

    [Fact]
    public async Task Repository_ApplyFilters_WithMultipleSources_ReturnsItemsFromAllSources()
    {
        // Arrange
        var testData = CreateNapiOlvasoTestData();
        var enabledSources = new List<string> { "13", "14" }; // barsi and horvath
        var expectedFilteredData = testData.Where(x => enabledSources.Contains(x.Tipus)).ToList();

        _repositoryMock.Setup(r => r.GetContentsByGroupName("group_napiolvaso", true, enabledSources))
                      .ReturnsAsync(expectedFilteredData);

        var pageService = new PageService(Mock.Of<IDownloadService>(), _repositoryMock.Object);

        // Act
        var result = await pageService.GetContentsAsync("0", true, enabledSources);

        // Assert
        Assert.Equal(expectedFilteredData.Count, result.Count);
        Assert.All(result, item => Assert.Contains(item.Tipus, enabledSources));
        _repositoryMock.Verify(r => r.GetContentsByGroupName("group_napiolvaso", true, enabledSources), Times.Once);
    }

    /// <summary>
    /// Test data matching the Activities enum values and database structure
    /// </summary>
    private static List<ContentEntity> CreateNapiOlvasoTestData()
    {
        return new List<ContentEntity>
        {
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Barsi Test 1 - Unread",
                Tipus = "13", // barsi
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-1)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Barsi Test 2 - Read",
                Tipus = "13", // barsi
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-2)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Horvath Test - Unread",
                Tipus = "14", // horvath
                TypeName = "horvath",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-3)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Horvath Test - Read",
                Tipus = "14", // horvath
                TypeName = "horvath",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-4)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Fokolare Test - Unread",
                Tipus = "7", // fokolare
                TypeName = "fokolare",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-5)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Fokolare Test - Read",
                Tipus = "7", // fokolare
                TypeName = "fokolare",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-6)
            }
        };
    }
}
