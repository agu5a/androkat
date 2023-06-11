using androkat.maui.library.Abstraction;
using androkat.maui.library.Models;
using androkat.maui.library.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace androkat.maui.library.Tests.Services;

public class SourceDataMapperTests
{
    [Fact]
    public void GetSourcesFromMemory_IndexExists_ReturnsCorrectSourceData()
    {
        // Arrange
        var sources = new Dictionary<int, SourceData> {
            { 1, new SourceData { Id = 1, Title = "Title 1"} },
            { 2, new SourceData { Id = 2, Title = "Title 2"} },
            { 3, new SourceData { Id = 3, Title = "Title 3"} }
        };
        var memoryMock = new Mock<ISourceData>();
        memoryMock.Setup(x => x.GetSourcesFromMemory(It.IsAny<int>())).Returns((int i) => sources[i]);

        var sourceDataMapper = new SourceDataMapper(sources);

        // Act 
        var expectedResult = sources[2];
        var result = sourceDataMapper.GetSourcesFromMemory(2);

        //Assert
        Assert.Equal(expectedResult, result);
    }
}
