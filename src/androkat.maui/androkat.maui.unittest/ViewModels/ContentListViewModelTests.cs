using androkat.maui.library.Abstraction;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using androkat.maui.library.Models.Responses;
using androkat.maui.library.ViewModels;
using Microsoft.Maui.Dispatching;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace androkat.maui.library.Tests.ViewModels;

public class ContentListViewModelTests
{
    private readonly Mock<IPageService> _pageServiceMock;
    private readonly Mock<ISourceData> _sourceDataMock;
    private readonly Mock<IAndrokatService> _androkatService;
    private readonly Mock<IDispatcher> _dispatcherMock;

    public ContentListViewModelTests()
    {
        _pageServiceMock = new Mock<IPageService>();
        _sourceDataMock = new Mock<ISourceData>();
        _androkatService = new Mock<IAndrokatService>();
        _dispatcherMock = new Mock<IDispatcher>();
    }

    [Fact]
    public async Task InitializeAsync_Should_Fetch_And_Convert_Contents()
    {
        // Arrange
        // Set Settings properties to avoid triggering update/version check logic
        Settings.LastUpdate = DateTime.Now; // Recent update to skip DownloadAll
        Settings.LastVersionCheck = DateTime.Now; // Recent version check to skip version check

        var contents = new List<ContentEntity>
        {
            new() {
                Tipus = "1",
                Image = "SomeImage",
                Datum = new DateTime(2022, 10, 10)
            }
        };

        var enabledSources = new List<string> { "1" }; // Provide explicit enabled sources to avoid Preferences calls

        // Setup mocks - note we don't need DownloadAll and GetServerInfo since we skip those logic branches
        _pageServiceMock.Setup(x => x.GetContentsAsync(It.IsAny<string>(), true, It.IsAny<List<string>>())).ReturnsAsync(contents);
        _pageServiceMock.Setup(x => x.GetVersion()).Returns(1);

        var idezetSourceMock = new SourceData
        {
            Img = "TestImg",
            Forrasszoveg = "Testforras",
            Title = "TestTitle"
        };

        _sourceDataMock.Setup(x => x.GetSourcesFromMemory(It.IsAny<int>())).Returns(idezetSourceMock);

        var viewModel = new ContentListViewModel(_dispatcherMock.Object, _pageServiceMock.Object, _sourceDataMock.Object, _androkatService.Object);

        // Act
        await viewModel.InitializeAsync(true, enabledSources);

        // Assert
        Assert.Equal("SomeImage", viewModel.Contents.First().First().contentImg);
        Assert.Equal(idezetSourceMock.Img, viewModel.Contents.First().First().ContentEntity.Image);

        _pageServiceMock.Verify(x => x.GetContentsAsync(It.IsAny<string>(), true, enabledSources), Times.Once);
        _sourceDataMock.Verify(x => x.GetSourcesFromMemory(It.IsAny<int>()), Times.Once);

        // Reset static properties to avoid affecting other tests
        Settings.LastUpdate = DateTime.MinValue;
        Settings.LastVersionCheck = DateTime.MinValue;
    }
}
