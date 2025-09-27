#nullable enable
using androkat.maui.library.Data;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace androkat.maui.unittest.Services;

/// <summary>
/// Direct tests for Repository filtering functionality using real database operations
/// </summary>
public class RepositoryFilteringRealTests
{
    [Fact]
    public async Task Repository_GetContentsByGroupName_WithNoFilters_ReturnsAllItems()
    {
        // Arrange
        var (path, repository) = await SeedTestData();

        // Act
        var result = await repository.GetContentsByGroupName("group_napiolvaso", returnVisited: true);

        // Assert
        Assert.Equal(6, result.Count); // All test items
        Assert.Contains(result, x => x.Tipus == "13" && !x.IsRead);
        Assert.Contains(result, x => x.Tipus == "13" && x.IsRead);
        Assert.Contains(result, x => x.Tipus == "14" && !x.IsRead);
        Assert.Contains(result, x => x.Tipus == "14" && x.IsRead);
        Assert.Contains(result, x => x.Tipus == "7" && !x.IsRead);
        Assert.Contains(result, x => x.Tipus == "7" && x.IsRead);

        DeleteTestDatabase(path);
    }

    [Fact]
    public async Task Repository_GetContentsByGroupName_WithSourceFilter_ReturnsFilteredItems()
    {
        // Arrange
        var (path, repository) = await SeedTestData();
        var enabledSources = new List<string> { "13" }; // Only barsi

        // Act
        var result = await repository.GetContentsByGroupName("group_napiolvaso", returnVisited: true, enabledSources);

        // Assert
        Assert.Equal(2, result.Count); // Only barsi items
        Assert.All(result, item => Assert.Equal("13", item.Tipus));
        Assert.Contains(result, x => !x.IsRead);
        Assert.Contains(result, x => x.IsRead);

        DeleteTestDatabase(path);
    }

    [Fact]
    public async Task Repository_GetContentsByGroupName_WithReadFilter_ReturnsUnreadItems()
    {
        // Arrange
        var (path, repository) = await SeedTestData();

        // Act
        var result = await repository.GetContentsByGroupName("group_napiolvaso", returnVisited: false);

        // Assert
        Assert.Equal(3, result.Count); // Only unread items
        Assert.All(result, item => Assert.False(item.IsRead));
        Assert.Contains(result, x => x.Tipus == "13");
        Assert.Contains(result, x => x.Tipus == "14");
        Assert.Contains(result, x => x.Tipus == "7");

        DeleteTestDatabase(path);
    }

    [Fact]
    public async Task Repository_GetContentsByGroupName_WithSourceAndReadFilters_AppliesBothFilters()
    {
        // Arrange
        var (path, repository) = await SeedTestData();
        var enabledSources = new List<string> { "13", "14" }; // barsi and horvath

        // Act
        var result = await repository.GetContentsByGroupName("group_napiolvaso", returnVisited: false, enabledSources);

        // Assert
        Assert.Equal(2, result.Count); // Only unread barsi and horvath items
        Assert.All(result, item =>
        {
            Assert.False(item.IsRead);
            Assert.Contains(item.Tipus, enabledSources);
        });
        Assert.Contains(result, x => x.Tipus == "13");
        Assert.Contains(result, x => x.Tipus == "14");

        DeleteTestDatabase(path);
    }

    [Fact]
    public async Task Repository_GetContentsByGroupName_WithEmptySourceFilter_ReturnsEmptyList()
    {
        // Arrange
        var (path, repository) = await SeedTestData();
        var enabledSources = new List<string>(); // Empty list

        // Act
        var result = await repository.GetContentsByGroupName("group_napiolvaso", returnVisited: true, enabledSources);

        // Assert
        Assert.Empty(result);

        DeleteTestDatabase(path);
    }

    [Fact]
    public async Task Repository_GetContentsByGroupName_WithNonExistentSource_ReturnsEmptyList()
    {
        // Arrange
        var (path, repository) = await SeedTestData();
        var enabledSources = new List<string> { "999" }; // Non-existent source

        // Act
        var result = await repository.GetContentsByGroupName("group_napiolvaso", returnVisited: true, enabledSources);

        // Assert
        Assert.Empty(result);

        DeleteTestDatabase(path);
    }

    [Fact]
    public async Task Repository_GetContentsByGroupName_WithMultipleSources_ReturnsAllMatchingItems()
    {
        // Arrange
        var (path, repository) = await SeedTestData();
        var enabledSources = new List<string> { "13", "14", "7" }; // All sources

        // Act
        var result = await repository.GetContentsByGroupName("group_napiolvaso", returnVisited: true, enabledSources);

        // Assert
        Assert.Equal(6, result.Count); // All items
        Assert.All(result, item => Assert.Contains(item.Tipus, enabledSources));

        DeleteTestDatabase(path);
    }

    [Fact]
    public async Task Repository_GetContentsByGroupName_WithPartialSources_ReturnsMatchingItems()
    {
        // Arrange
        var (path, repository) = await SeedTestData();
        var enabledSources = new List<string> { "14", "7" }; // horvath and fokolare

        // Act
        var result = await repository.GetContentsByGroupName("group_napiolvaso", returnVisited: true, enabledSources);

        // Assert
        Assert.Equal(4, result.Count); // 2 horvath + 2 fokolare
        Assert.All(result, item => Assert.Contains(item.Tipus, enabledSources));
        Assert.Contains(result, x => x.Tipus == "14" && !x.IsRead);
        Assert.Contains(result, x => x.Tipus == "14" && x.IsRead);
        Assert.Contains(result, x => x.Tipus == "7" && !x.IsRead);
        Assert.Contains(result, x => x.Tipus == "7" && x.IsRead);

        DeleteTestDatabase(path);
    }

    [Fact]
    public async Task Repository_GetContentsByGroupName_WithDifferentGroupNames_ReturnsCorrectItems()
    {
        // Arrange
        var (path, repository) = await SeedTestDataWithMultipleGroups();

        // Act - Get napiolvaso items
        var napiResult = await repository.GetContentsByGroupName("group_napiolvaso", returnVisited: true);

        // Act - Get szentek items
        var szentekResult = await repository.GetContentsByGroupName("group_szentek", returnVisited: true);

        // Assert
        Assert.Equal(2, napiResult.Count);
        Assert.All(napiResult, item => Assert.Equal("group_napiolvaso", item.GroupName));

        Assert.Equal(2, szentekResult.Count);
        Assert.All(szentekResult, item => Assert.Equal("group_szentek", item.GroupName));

        DeleteTestDatabase(path);
    }

    [Theory]
    [InlineData("13", 2)] // barsi - 2 items
    [InlineData("14", 2)] // horvath - 2 items  
    [InlineData("7", 2)]  // fokolare - 2 items
    [InlineData("999", 0)] // non-existent - 0 items
    public async Task Repository_GetContentsByGroupName_WithSingleSource_ReturnsCorrectCount(string sourceId, int expectedCount)
    {
        // Arrange
        var (path, repository) = await SeedTestData();
        var enabledSources = new List<string> { sourceId };

        // Act
        var result = await repository.GetContentsByGroupName("group_napiolvaso", returnVisited: true, enabledSources);

        // Assert
        Assert.Equal(expectedCount, result.Count);
        if (expectedCount > 0)
        {
            Assert.All(result, item => Assert.Equal(sourceId, item.Tipus));
        }

        DeleteTestDatabase(path);
    }

    private async Task<(string testDbPath, Repository repository)> SeedTestData()
    {
        var testEntities = new List<ContentEntity>
        {
            new()
            {
                Nid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Cim = "Barsi Test 1 - Unread",
                Tipus = "13", // barsi
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-1),
                Idezet = "Test content 1"
            },
            new()
            {
                Nid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Cim = "Barsi Test 2 - Read",
                Tipus = "13", // barsi
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-2),
                Idezet = "Test content 2"
            },
            new()
            {
                Nid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Cim = "Horvath Test 1 - Unread",
                Tipus = "14", // horvath
                TypeName = "horvath",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-3),
                Idezet = "Test content 3"
            },
            new()
            {
                Nid = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Cim = "Horvath Test 2 - Read",
                Tipus = "14", // horvath
                TypeName = "horvath",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-4),
                Idezet = "Test content 4"
            },
            new()
            {
                Nid = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Cim = "Fokolare Test 1 - Unread",
                Tipus = "7", // fokolare
                TypeName = "fokolare",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-5),
                Idezet = "Test content 5"
            },
            new()
            {
                Nid = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                Cim = "Fokolare Test 2 - Read",
                Tipus = "7", // fokolare
                TypeName = "fokolare",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-6),
                Idezet = "Test content 6"
            }
        };

        string userTmpFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "tmp"
        );

        // Make sure the folder exists
        Directory.CreateDirectory(userTmpFolder);

        var testDbPath = Path.Combine(userTmpFolder, $"test_androkat_{Guid.NewGuid()}.db");
        var repository = new Repository(testDbPath);

        Thread.Sleep(100);

        foreach (var entity in testEntities)
        {
            var result = await repository.InsertContent(entity);
            if (result.Item1 < 1)
            {
                throw new Exception("Failed to insert test entity into the database. " + result.Item2);
            }
        }

        return (testDbPath, repository);
    }

    private async Task<(string testDbPath, Repository repository)> SeedTestDataWithMultipleGroups()
    {
        var testEntities = new List<ContentEntity>
        {
            new()
            {
                Nid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Cim = "Barsi Test",
                Tipus = "13", // barsi
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-1),
                Idezet = "Test content 1"
            },
            new()
            {
                Nid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Cim = "Horvath Test",
                Tipus = "14", // horvath
                TypeName = "horvath",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-2),
                Idezet = "Test content 2"
            },
            new()
            {
                Nid = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Cim = "Pio Test",
                Tipus = "2", // pio
                TypeName = "pio",
                GroupName = "group_szentek",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-3),
                Idezet = "Test content 3"
            },
            new()
            {
                Nid = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Cim = "Vianney Test",
                Tipus = "20", // vianney
                TypeName = "vianney",
                GroupName = "group_szentek",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-4),
                Idezet = "Test content 4"
            }
        };

        string userTmpFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "tmp"
        );

        // Make sure the folder exists
        Directory.CreateDirectory(userTmpFolder);

        var testDbPath = Path.Combine(userTmpFolder, $"test_androkat_{Guid.NewGuid()}.db");
        var repository = new Repository(testDbPath);

        Thread.Sleep(100);

        foreach (var entity in testEntities)
        {
            var result = await repository.InsertContent(entity);
            if (result.Item1 < 1)
            {
                throw new Exception("Failed to insert test entity into the database. " + result.Item2);
            }
        }

        return (testDbPath, repository);
    }

#pragma warning disable xUnit1013 // Public method should be marked as test
    public void DeleteTestDatabase(string testDbPath)
#pragma warning restore xUnit1013 // Public method should be marked as test
    {
        try
        {
            if (File.Exists(testDbPath))
            {
                File.Delete(testDbPath);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete test database file: " + ex.Message);
        }
    }
}
