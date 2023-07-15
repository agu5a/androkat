using androkat.maui.library.Abstraction;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using androkat.maui.library.ViewModels;
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
    private readonly Mock<IHelperSharedPreferences> _helperSharedPreferences;

    public ContentListViewModelTests()
    {
        _pageServiceMock = new Mock<IPageService>();
        _sourceDataMock = new Mock<ISourceData>();
        _androkatService = new Mock<IAndrokatService>();
        _helperSharedPreferences = new Mock<IHelperSharedPreferences>();
    }

    [Fact]
    public async Task InitializeAsync_Should_Fetch_And_Convert_Contents()
    {
        // Arrange
        var contents = new List<ContentEntity>
        {
            new ContentEntity
            {
                Tipus = "1",
                Image = "SomeImage",
                Datum = new DateTime(2022, 10, 10)
            }
        };

        _pageServiceMock.Setup(x => x.GetContentsAsync(It.IsAny<string>())).ReturnsAsync(contents);

        var idezetSourceMock = new SourceData
        {
            Img = "TestImg",
            Forrasszoveg = "Testforras",
            Title = "TestTitle"
        };

        _sourceDataMock.Setup(x => x.GetSourcesFromMemory(It.IsAny<int>())).Returns(idezetSourceMock);

        var viewModel = new ContentListViewModel(_pageServiceMock.Object, _sourceDataMock.Object, _androkatService.Object, _helperSharedPreferences.Object);

        // Act
        await viewModel.InitializeAsync();

        // Assert
        Assert.Equal("SomeImage", viewModel.Contents.First().First().contentImg);
        Assert.Equal(idezetSourceMock.Img, viewModel.Contents.First().First().ContentEntity.Image);

        _pageServiceMock.Verify(x => x.GetContentsAsync(It.IsAny<string>()), Times.Once);
        _sourceDataMock.Verify(x => x.GetSourcesFromMemory(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Subscribe_ShouldDoNothing()
    {
        //arrange
        var itemViewModelMock = new ContentItemViewModel(new ContentEntity());
        var viewModel = new ContentListViewModel(_pageServiceMock.Object, _sourceDataMock.Object, _androkatService.Object, _helperSharedPreferences.Object);

        //act
        await viewModel.Subscribe(itemViewModelMock);

        //assert

    }
}
