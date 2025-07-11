﻿using androkat.maui.library.Abstraction;
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
        IEnumerable<FavoriteContentEntity> favorites = [new() { Nid = Guid.NewGuid(), Idezet = "Idezet", Tipus = "1" }];
        _pageServiceMock.Setup(p => p.GetFavoriteCountAsync()).ReturnsAsync(favorites.Count);
        _pageServiceMock.Setup(p => p.GetFavoriteContentsAsync()).ReturnsAsync(favorites.ToList());

        //act
        await _viewModel.InitializeAsync();

        //assert
        Assert.Equal(favorites.Count(), _viewModel.FavoriteCount);
        Assert.NotNull(_viewModel.Contents);
        Assert.NotEmpty(_viewModel.Contents);
        Assert.Single(_viewModel.Contents);
        Assert.Equal("Idezet", _viewModel.Contents[0].First().ContentEntity.Idezet);
    }

    [Fact]
    public async Task InitializeAsync_ShouldReturnOnError()
    {
        //arrange
        _pageServiceMock.Setup(p => p.GetFavoriteCountAsync()).ThrowsAsync(new Exception("test"));

        //act
        await Assert.ThrowsAsync<Exception>(() => _viewModel.InitializeAsync());

        //assert
        _pageServiceMock.Verify(p => p.GetFavoriteContentsAsync(), Times.Never);
        //_pageServiceMock.Verify(p => p.DisplayAlertAsync(
        //    It.IsAny<string>(),
        //    It.IsAny<string>(),
        //    It.IsAny<string>()), Times.Once);
        Assert.Empty(_viewModel.Contents);
    }
}
