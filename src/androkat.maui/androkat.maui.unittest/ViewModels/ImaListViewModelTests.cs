using androkat.maui.library.Abstraction;
using androkat.maui.library.Models.Entities;
using androkat.maui.library.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace androkat.maui.unittest.ViewModels;

public class ImaListViewModelTests
{
    private readonly Mock<IPageService> _pageServiceMock;

    public ImaListViewModelTests()
    {
        _pageServiceMock = new Mock<IPageService>();
    }

    [Fact]
    public async Task InitializeAsync_ContentsNull_ReturnsHibaDialog()
    {
        //Arrange
        var contents = new List<ImadsagEntity>
        {
            new() {
                GroupName = "GroupName",
                Content = "Content",
                Datum = new DateTime(2022, 10, 10)
            }
        };

        _pageServiceMock.Setup(service => service.GetImaContents(1, 10, It.IsAny<int?>())).ReturnsAsync(contents);

        var viewModel = new ImaListViewModel(_pageServiceMock.Object);

        //Act
        await viewModel.InitializeAsync(1, 10);

        //Assert
        Assert.NotNull(viewModel.Contents);
        Assert.NotEmpty(viewModel.Contents);
        Assert.Equal("Imádságok", viewModel.Contents.First().detailscim);

        _pageServiceMock.Verify(x => x.GetImaContents(1, 10, It.IsAny<int?>()), Times.Once);
    }
}
