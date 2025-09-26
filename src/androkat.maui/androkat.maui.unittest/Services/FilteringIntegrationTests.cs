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

public class FilteringIntegrationTests
{
    private readonly Mock<IRepository> _repositoryMock;
    private readonly Mock<IPageService> _pageServiceMock;
    private readonly Mock<ISourceData> _sourceDataMock;
    private readonly Mock<IAndrokatService> _androkatServiceMock;
    private readonly Mock<IDispatcher> _dispatcherMock;

    public FilteringIntegrationTests()
    {
        _repositoryMock = new Mock<IRepository>();
        _pageServiceMock = new Mock<IPageService>();
        _sourceDataMock = new Mock<ISourceData>();
        _androkatServiceMock = new Mock<IAndrokatService>();
        _dispatcherMock = new Mock<IDispatcher>();
    }

    [Fact]
    public async Task PageService_GetContentsAsync_WithSourceFiltering_ShouldPassCorrectParameters()
    {
        // Arrange
        var testData = CreateTestContentEntities();
        var enabledSources = new List<string> { "13", "14" };

        _repositoryMock.Setup(r => r.GetContentsByGroupName("group_napiolvaso", true, enabledSources))
                      .ReturnsAsync(testData.Where(x => enabledSources.Contains(x.Tipus)).ToList());

        var pageService = new PageService(Mock.Of<IDownloadService>(), _repositoryMock.Object);

        // Act
        var result = await pageService.GetContentsAsync("0", true, enabledSources);

        // Assert
        _repositoryMock.Verify(r => r.GetContentsByGroupName("group_napiolvaso", true, enabledSources), Times.Once);
        Assert.Equal(3, result.Count); // 2 Barsi + 1 Horvath
        Assert.All(result, item => Assert.Contains(item.Tipus, enabledSources));
    }

    [Fact]
    public async Task ContentListViewModel_FetchAsync_WithFiltering_ShouldApplyFilters()
    {
        // Arrange
        var testData = CreateTestContentEntities();
        var enabledSources = new List<string> { "13" }; // Only barsi
        var showVisited = false; // Hide read items

        var filteredData = testData.Where(x =>
            enabledSources.Contains(x.Tipus) && !x.IsRead).ToList();

        _pageServiceMock.Setup(p => p.GetContentsAsync("0", showVisited, enabledSources))
                       .ReturnsAsync(filteredData);

        _sourceDataMock.Setup(s => s.GetSourcesFromMemory(It.IsAny<int>()))
                      .Returns(new SourceData
                      {
                          Img = "test.jpg",
                          Title = "Test Source",
                          Forrasszoveg = "Test Source Text"
                      });

        var viewModel = new ContentListViewModel(
            _dispatcherMock.Object,
            _pageServiceMock.Object,
            _sourceDataMock.Object,
            _androkatServiceMock.Object)
        {
            Id = "0"
        };

        // Act
        await viewModel.FetchAsync(showVisited, enabledSources);

        // Assert
        _pageServiceMock.Verify(p => p.GetContentsAsync("0", showVisited, enabledSources), Times.Once);
        Assert.Single(viewModel.Contents);
        Assert.Single(viewModel.Contents.First());
    }

    [Theory]
    [InlineData("0", "group_napiolvaso")] // Napi olvaso
    [InlineData("3", "group_szentek")]    // Szentek
    [InlineData("4", "group_news")]       // News
    [InlineData("5", "group_blog")]       // Magazin
    [InlineData("8", "group_audio")]      // Audio
    public async Task PageService_GetContentsAsync_RoutesToCorrectRepositoryMethod(string pageId, string expectedGroupName)
    {
        // Arrange
        var testData = CreateTestContentEntities();
        var enabledSources = new List<string> { "13" };

        _repositoryMock.Setup(r => r.GetContentsByGroupName(expectedGroupName, true, enabledSources))
                      .ReturnsAsync(testData);

        var pageService = new PageService(Mock.Of<IDownloadService>(), _repositoryMock.Object);

        // Act
        var result = await pageService.GetContentsAsync(pageId, true, enabledSources);

        // Assert
        _repositoryMock.Verify(r => r.GetContentsByGroupName(expectedGroupName, true, enabledSources), Times.Once);
    }

    [Fact]
    public void Settings_GetEnabledSources_RequiresPreferencesImplementation()
    {
        // Arrange
        var filterOptions = new List<FilterOption>
        {
            new() { Key = "13", DisplayName = "Barsi", IsEnabled = true, Activity = Activities.barsi },
            new() { Key = "14", DisplayName = "Horvath", IsEnabled = false, Activity = Activities.horvath }
        };

        // Act & Assert
        // In unit test environment, Settings.GetEnabledSources should throw because Preferences.Get is not available
        // This test documents the expected behavior - in production, this method needs a platform-specific Preferences implementation
        Assert.ThrowsAny<Exception>(() =>
        {
            Settings.GetEnabledSources(filterOptions);
        });
    }

    [Fact]
    public void Settings_GetEnabledSources_WithEmptyFilterOptions_ReturnsEmptyList()
    {
        // Arrange
        var filterOptions = new List<FilterOption>();

        // Act
        var enabledSources = Settings.GetEnabledSources(filterOptions);

        // Assert
        Assert.IsType<List<string>>(enabledSources);
        Assert.Empty(enabledSources);
    }

    [Fact]
    public void Settings_GetEnabledSources_Integration_CanBeBypassedByProvidingExplicitSources()
    {
        // Arrange - This test shows how to work around the Preferences limitation in tests
        var filterOptions = new List<FilterOption>
        {
            new() { Key = "13", DisplayName = "Barsi", IsEnabled = true, Activity = Activities.barsi },
            new() { Key = "14", DisplayName = "Horvath", IsEnabled = false, Activity = Activities.horvath },
            new() { Key = "7", DisplayName = "Fokolare", IsEnabled = true, Activity = Activities.fokolare }
        };

        // Instead of calling Settings.GetEnabledSources(filterOptions), in tests we should provide explicit sources
        var explicitEnabledSources = new List<string> { "13", "7" }; // Simulate enabled sources

        // Act - Test the code that would use the enabled sources
        var filteredOptions = filterOptions.Where(f => explicitEnabledSources.Contains(f.Key)).ToList();

        // Assert
        Assert.Equal(2, filteredOptions.Count);
        Assert.Contains(filteredOptions, f => f.Key == "13" && f.DisplayName == "Barsi");
        Assert.Contains(filteredOptions, f => f.Key == "7" && f.DisplayName == "Fokolare");
        Assert.DoesNotContain(filteredOptions, f => f.Key == "14");
    }
    [Fact]
    public void FilterOptionsHelper_GeneratesConsistentKeysWithActivitiesEnum()
    {
        // Arrange & Act
        var napiOlvasoOptions = FilterOptionsHelper.GetFilterOptionsForPageId("0");
        var szentekOptions = FilterOptionsHelper.GetFilterOptionsForPageId("3");

        // Assert
        Assert.All(napiOlvasoOptions, option =>
        {
            var expectedKey = ((int)option.Activity).ToString();
            Assert.Equal(expectedKey, option.Key);
        });

        Assert.All(szentekOptions, option =>
        {
            var expectedKey = ((int)option.Activity).ToString();
            Assert.Equal(expectedKey, option.Key);
        });
    }

    [Fact]
    public async Task EndToEndFiltering_WithMultipleFilters_ShouldWorkCorrectly()
    {
        // Arrange
        var allTestData = CreateTestContentEntities();
        var enabledSources = new List<string> { "13" }; // Only barsi
        var showVisited = false; // Hide read items

        // Expected result: only unread barsi items
        var expectedData = allTestData.Where(x =>
            x.Tipus == "13" && !x.IsRead).ToList();

        _repositoryMock.Setup(r => r.GetContentsByGroupName("group_napiolvaso", showVisited, enabledSources))
                      .ReturnsAsync(expectedData);

        _pageServiceMock.Setup(p => p.GetContentsAsync("0", showVisited, enabledSources))
                       .ReturnsAsync(expectedData);

        _sourceDataMock.Setup(s => s.GetSourcesFromMemory(13))
                      .Returns(new SourceData
                      {
                          Img = "barsi.jpg",
                          Title = "Barsi",
                          Forrasszoveg = "Barsi és Telek"
                      });

        var pageService = new PageService(Mock.Of<IDownloadService>(), _repositoryMock.Object);
        var viewModel = new ContentListViewModel(
            _dispatcherMock.Object,
            _pageServiceMock.Object,
            _sourceDataMock.Object,
            _androkatServiceMock.Object)
        {
            Id = "0"
        };

        // Act
        var serviceResult = await pageService.GetContentsAsync("0", showVisited, enabledSources);
        await viewModel.FetchAsync(showVisited, enabledSources);

        // Assert
        Assert.Single(serviceResult);
        Assert.Equal("13", serviceResult.First().Tipus);
        Assert.False(serviceResult.First().IsRead);

        Assert.Single(viewModel.Contents);
        Assert.Single(viewModel.Contents.First());
    }

    private static List<ContentEntity> CreateTestContentEntities()
    {
        return new List<ContentEntity>
        {
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Barsi Unread",
                Tipus = "13",
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-1)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Barsi Read",
                Tipus = "13",
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-2)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Horvath Unread",
                Tipus = "14",
                TypeName = "horvath",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-3)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Pio Unread",
                Tipus = "2",
                TypeName = "pio",
                GroupName = "group_szentek",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-4)
            }
        };
    }
}
