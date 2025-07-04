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

namespace androkat.maui.unittest.ViewModels;

public class PrayDetailViewModelTests
{
    private readonly Mock<IPageService> _mockPageService;
    private readonly PrayDetailViewModel _viewModel;

    public PrayDetailViewModelTests()
    {
        _mockPageService = new Mock<IPageService>();
        _viewModel = new PrayDetailViewModel(_mockPageService.Object);
    }

    [Fact]
    public async Task InitializeAsync_SetsContentViewWithPrayerData()
    {
        // Arrange
        var prayId = Guid.NewGuid();
        var prayEntity = new ImadsagEntity
        {
            Nid = prayId,
            Cim = "Test Prayer",
            Content = "Test prayer content",
            Datum = DateTime.Now,
            TypeName = "ima"
        };

        _mockPageService.Setup(x => x.GetImadsagEntityByIdAsync(prayId))
            .ReturnsAsync(prayEntity);
        _mockPageService.Setup(x => x.GetFavoriteContentsAsync())
            .ReturnsAsync(new List<FavoriteContentEntity>());

        _viewModel.Id = prayId.ToString();

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        Assert.NotNull(_viewModel.ContentView);
        Assert.Equal("Test Prayer", _viewModel.ContentView.ContentEntity.Cim);
        Assert.Equal("Test prayer content", _viewModel.ContentView.ContentEntity.Idezet);
        Assert.Equal(Activities.ima, _viewModel.ContentView.type);
        Assert.Equal(((int)Activities.ima).ToString(), _viewModel.ContentView.ContentEntity.Tipus);
    }

    [Fact]
    public async Task InitializeAsync_SetsFromFavorites_WhenFromFavoritesIsTrue()
    {
        // Arrange
        var prayId = Guid.NewGuid();
        var prayEntity = new ImadsagEntity
        {
            Nid = prayId,
            Cim = "Test Prayer",
            Content = "Test prayer content",
            Datum = DateTime.Now,
            TypeName = "ima"
        };

        _mockPageService.Setup(x => x.GetImadsagEntityByIdAsync(prayId))
            .ReturnsAsync(prayEntity);
        _mockPageService.Setup(x => x.GetFavoriteContentsAsync())
            .ReturnsAsync(new List<FavoriteContentEntity>());

        _viewModel.Id = prayId.ToString();
        _viewModel.FromFavorites = "true";

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        Assert.True(_viewModel.ShowDeleteFavoriteButton);
    }

    [Fact]
    public async Task AddFavorite_AddsToFavorites_WhenNotAlreadyFavorited()
    {
        // Arrange
        var prayId = Guid.NewGuid();
        var prayEntity = new ImadsagEntity
        {
            Nid = prayId,
            Cim = "Test Prayer",
            Content = "Test prayer content",
            Datum = DateTime.Now,
            TypeName = "ima"
        };

        _mockPageService.Setup(x => x.GetImadsagEntityByIdAsync(prayId))
            .ReturnsAsync(prayEntity);
        _mockPageService.Setup(x => x.GetFavoriteContentsAsync())
            .ReturnsAsync(new List<FavoriteContentEntity>());
        _mockPageService.Setup(x => x.InsertFavoriteContentAsync(It.IsAny<FavoriteContentEntity>()))
            .ReturnsAsync(1);

        _viewModel.Id = prayId.ToString();
        await _viewModel.InitializeAsync();

        // Act
        await _viewModel.AddFavoriteCommand.ExecuteAsync(null);

        // Assert
        _mockPageService.Verify(x => x.InsertFavoriteContentAsync(It.IsAny<FavoriteContentEntity>()), Times.Once);
        Assert.True(_viewModel.IsAlreadyFavorited);
    }

    [Fact]
    public async Task CheckIfAlreadyFavorited_SetsIsAlreadyFavorited_WhenPrayerIsInFavorites()
    {
        // Arrange
        var prayId = Guid.NewGuid();
        var prayEntity = new ImadsagEntity
        {
            Nid = prayId,
            Cim = "Test Prayer",
            Content = "Test prayer content",
            Datum = DateTime.Now,
            TypeName = "ima"
        };

        var favoriteEntity = new FavoriteContentEntity
        {
            Nid = prayId,
            Cim = "Test Prayer",
            Idezet = "Test prayer content"
        };

        _mockPageService.Setup(x => x.GetImadsagEntityByIdAsync(prayId))
            .ReturnsAsync(prayEntity);
        _mockPageService.Setup(x => x.GetFavoriteContentsAsync())
            .ReturnsAsync(new List<FavoriteContentEntity> { favoriteEntity });

        _viewModel.Id = prayId.ToString();

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        Assert.True(_viewModel.IsAlreadyFavorited);
        Assert.True(_viewModel.IsFavoriteCheckCompleted);
    }

    [Fact]
    public void CanAddFavorite_ReturnsFalse_WhenAlreadyFavorited()
    {
        // Arrange
        _viewModel.IsAlreadyFavorited = true;
        _viewModel.IsFavoriteCheckCompleted = true;

        // Act
        var canAdd = _viewModel.AddFavoriteCommand.CanExecute(null);

        // Assert
        Assert.False(canAdd);
    }

    [Fact]
    public void CanAddFavorite_ReturnsTrue_WhenNotFavoritedAndCheckCompleted()
    {
        // Arrange
        _viewModel.IsAlreadyFavorited = false;
        _viewModel.IsFavoriteCheckCompleted = true;

        // Act
        var canAdd = _viewModel.AddFavoriteCommand.CanExecute(null);

        // Assert
        Assert.True(canAdd);
    }
}
