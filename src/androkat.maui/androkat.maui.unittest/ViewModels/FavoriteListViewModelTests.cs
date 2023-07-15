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

namespace androkat.maui.library.tests.ViewModels;

public class FavoriteListViewModelTests
{
    private readonly FavoriteListViewModel _viewModel;
    private readonly Mock<IPageService> _pageServiceMock = new();
    private readonly Mock<ISourceData> _sourceDataMock = new();

    public FavoriteListViewModelTests()
    {
        var idezetSourceMock = new SourceData
        {
            Img = "TestImg",
            Forrasszoveg = "Testforras",
            Title = "TestTitle"
        };

        _sourceDataMock.Setup(x => x.GetSourcesFromMemory(It.IsAny<int>())).Returns(idezetSourceMock);
        _viewModel = new FavoriteListViewModel(_pageServiceMock.Object, _sourceDataMock.Object);
    }

    [Fact]
    public async Task InitializeAsync_ShouldPopulateContents()
    {
        //arrange
        IEnumerable<FavoriteContentEntity> favorites = new List<FavoriteContentEntity>() { new FavoriteContentEntity() { Nid = Guid.NewGuid(), Idezet = "Idezet", Tipus = "1" } };
        _pageServiceMock.Setup(p => p.GetFavoriteCountAsync()).ReturnsAsync(favorites.Count);
        _pageServiceMock.Setup(p => p.GetFavoriteContentsAsync()).ReturnsAsync(favorites.ToList());

        //act
        await _viewModel.InitializeAsync();

        //assert
        Assert.Equal(favorites.Count(), _viewModel.FavoriteCount);
        Assert.NotNull(_viewModel.Contents);
        Assert.NotEmpty(_viewModel.Contents);
        Assert.Collection(_viewModel.Contents[0], item => Assert.Equal("Idezet", item.ContentEntity.Idezet));
    }

    [Fact]
    public async Task InitializeAsync_ShouldReturnOnError()
    {
        //arrange
        _pageServiceMock.Setup(p => p.GetFavoriteCountAsync()).ThrowsAsync(new Exception("test"));

        //act
        await Assert.ThrowsAsync<Exception>(async () => await _viewModel.InitializeAsync());

        //assert
        _pageServiceMock.Verify(p => p.GetFavoriteContentsAsync(), Times.Never);
        //_pageServiceMock.Verify(p => p.DisplayAlertAsync(
        //    It.IsAny<string>(),
        //    It.IsAny<string>(),
        //    It.IsAny<string>()), Times.Once);
        Assert.Empty(_viewModel.Contents);
    }

    [Fact]
    public async Task Subscribe_ShouldDoNothing()
    {
        //arrange
        var itemViewModelMock = new FavoriteContentViewModel(new FavoriteContentEntity());

        //act
        await _viewModel.Subscribe(itemViewModelMock);

        //assert
        
    }
}
