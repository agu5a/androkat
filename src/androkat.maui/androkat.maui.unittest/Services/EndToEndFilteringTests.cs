#nullable enable
using androkat.maui.library.Data;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace androkat.maui.unittest.Services;

/// <summary>
/// End-to-end filtering scenario tests that simulate real user interactions
/// These tests verify that the complete filtering pipeline works as expected
/// </summary>
public class EndToEndFilteringTests : IDisposable
{
    private readonly string _testDbPath;
    private readonly Repository _repository;

    public EndToEndFilteringTests()
    {
        _testDbPath = Path.Combine(Path.GetTempPath(), $"test_androkat_e2e_{Guid.NewGuid():N}.db");
        _repository = new Repository(_testDbPath);
    }

    [Fact]
    public async Task EndToEnd_NapiOlvaso_ShowOnlyBarsiContent_ShouldFilterCorrectly()
    {
        // Arrange - Seed the database with realistic data
        await SeedNapiOlvasoData();

        // Get filter options for Napi Olvasmány page (page ID "0")
        var filterOptions = FilterOptionsHelper.GetFilterOptionsForPageId("0");

        // Find the Barsi filter option
        var barsiOption = filterOptions.FirstOrDefault(o => o.Activity == Activities.barsi);
        Assert.NotNull(barsiOption);
        Assert.Equal("13", barsiOption.Key);

        // Act - Apply Barsi filter (user selects only Barsi checkbox)
        var enabledSources = new List<string> { barsiOption.Key };
        var result = await _repository.GetContentsByGroupName("group_napiolvaso", returnVisited: true, enabledSources);

        // Assert - Should only return Barsi content
        Assert.Equal(3, result.Count); // 3 Barsi items
        Assert.All(result, item =>
        {
            Assert.Equal("13", item.Tipus);
            Assert.Equal("barsi", item.TypeName);
            Assert.Equal("group_napiolvaso", item.GroupName);
        });
    }

    [Fact]
    public async Task EndToEnd_NapiOlvaso_HideReadItems_ShouldFilterCorrectly()
    {
        // Arrange
        await SeedNapiOlvasoData();

        // Act - Hide read items (user unchecks "Show read items")
        var result = await _repository.GetContentsByGroupName("group_napiolvaso", returnVisited: false);

        // Assert - Should only return unread items
        var expectedUnreadCount = 4; // Based on seeded data
        Assert.Equal(expectedUnreadCount, result.Count);
        Assert.All(result, item => Assert.False(item.IsRead));
    }

    [Fact]
    public async Task EndToEnd_NapiOlvaso_BarsiAndHorvathUnreadOnly_ShouldApplyBothFilters()
    {
        // Arrange
        await SeedNapiOlvasoData();

        // Get filter options
        var filterOptions = FilterOptionsHelper.GetFilterOptionsForPageId("0");
        var barsiKey = filterOptions.First(o => o.Activity == Activities.barsi).Key;
        var horvathKey = filterOptions.First(o => o.Activity == Activities.horvath).Key;

        // Act - User selects Barsi and Horvath, and hides read items
        var enabledSources = new List<string> { barsiKey, horvathKey };
        var result = await _repository.GetContentsByGroupName("group_napiolvaso", returnVisited: false, enabledSources);

        // Assert - Should return only unread Barsi and Horvath items
        Assert.Equal(2, result.Count); // 1 unread Barsi + 1 unread Horvath
        Assert.All(result, item =>
        {
            Assert.Contains(item.Tipus, enabledSources);
            Assert.False(item.IsRead);
        });
    }

    [Fact]
    public async Task EndToEnd_Szentek_ShowOnlyPioContent_ShouldFilterCorrectly()
    {
        // Arrange
        await SeedSzentekData();

        // Get filter options for Szentek page (page ID "3")
        var filterOptions = FilterOptionsHelper.GetFilterOptionsForPageId("3");

        // Find the Pio filter option
        var pioOption = filterOptions.FirstOrDefault(o => o.Activity == Activities.pio);
        Assert.NotNull(pioOption);
        Assert.Equal("2", pioOption.Key);

        // Act - Apply Pio filter
        var enabledSources = new List<string> { pioOption.Key };
        var result = await _repository.GetContentsByGroupName("group_szentek", returnVisited: true, enabledSources);

        // Assert - Should only return Pio content
        Assert.Equal(2, result.Count); // 2 Pio items
        Assert.All(result, item =>
        {
            Assert.Equal("2", item.Tipus);
            Assert.Equal("pio", item.TypeName);
            Assert.Equal("group_szentek", item.GroupName);
        });
    }

    [Fact]
    public async Task EndToEnd_NoFiltersSelected_ShouldReturnEmptyList()
    {
        // Arrange
        await SeedNapiOlvasoData();

        // Act - User deselects all source filters (empty list)
        var enabledSources = new List<string>(); // No sources enabled
        var result = await _repository.GetContentsByGroupName("group_napiolvaso", returnVisited: true, enabledSources);

        // Assert - Should return empty list
        Assert.Empty(result);
    }

    [Fact]
    public async Task EndToEnd_AllFiltersEnabled_ShouldReturnAllItems()
    {
        // Arrange
        await SeedNapiOlvasoData();

        // Get all available filter options
        var filterOptions = FilterOptionsHelper.GetFilterOptionsForPageId("0");
        var allSourceKeys = filterOptions.Select(o => o.Key).ToList();

        // Act - User selects all source filters
        var result = await _repository.GetContentsByGroupName("group_napiolvaso", returnVisited: true, allSourceKeys);

        // Assert - Should return all items  
        Assert.Equal(7, result.Count); // All seeded items that match filter options
    }

    [Theory]
    [InlineData("0", "group_napiolvaso")] // Napi olvasmány
    [InlineData("3", "group_szentek")]    // Szentek
    [InlineData("4", "group_news")]       // Hírek
    [InlineData("5", "group_blog")]       // Magazin
    [InlineData("8", "group_audio")]      // Audio
    public void EndToEnd_FilterOptionsGeneration_ShouldReturnValidOptionsForAllPages(string pageId, string _)
    {
        // Act
        var filterOptions = FilterOptionsHelper.GetFilterOptionsForPageId(pageId);

        // Assert
        if (pageId == "0" || pageId == "3" || pageId == "4" || pageId == "5" || pageId == "8") // These pages have filter options
        {
            Assert.NotEmpty(filterOptions);
            Assert.All(filterOptions, option =>
            {
                Assert.NotEmpty(option.Key);
                Assert.NotEmpty(option.DisplayName);
                Assert.True(int.TryParse(option.Key, out int _), $"Key '{option.Key}' should be a valid integer");
            });
        }
        else
        {
            Assert.Empty(filterOptions); // Other pages don't have filter options
        }
    }

    private async Task SeedNapiOlvasoData()
    {
        // Clear existing data to ensure test isolation
        await _repository.DeleteAllContent();

        var entities = new List<ContentEntity>
        {
            // Barsi content (3 items)
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Barsi 1 - Read",
                Tipus = "13",
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-1),
                Idezet = "Barsi content 1"
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Barsi 2 - Unread",
                Tipus = "13",
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-2),
                Idezet = "Barsi content 2"
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Barsi 3 - Read",
                Tipus = "13",
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-3),
                Idezet = "Barsi content 3"
            },
            
            // Horvath content (2 items)
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Horvath 1 - Unread",
                Tipus = "14",
                TypeName = "horvath",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-4),
                Idezet = "Horvath content 1"
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Horvath 2 - Read",
                Tipus = "14",
                TypeName = "horvath",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-5),
                Idezet = "Horvath content 2"
            },
            
            // Fokolare content (2 items)
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Fokolare 1 - Unread",
                Tipus = "7",
                TypeName = "fokolare",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-6),
                Idezet = "Fokolare content 1"
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Fokolare 2 - Unread",
                Tipus = "7",
                TypeName = "fokolare",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-7),
                Idezet = "Fokolare content 2"
            },
            
            // Zsinagoga content (1 item)
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Zsinagoga 1 - Read",
                Tipus = "34",
                TypeName = "zsinagoga",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-8),
                Idezet = "Zsinagoga content 1"
            }
        };

        foreach (var entity in entities)
        {
            await _repository.InsertContent(entity);
        }
    }

    private async Task SeedSzentekData()
    {
        // Clear existing data to ensure test isolation
        await _repository.DeleteAllContent();

        var entities = new List<ContentEntity>
        {
            // Pio content (2 items)
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Pio 1 - Read",
                Tipus = "2",
                TypeName = "pio",
                GroupName = "group_szentek",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-1),
                Idezet = "Pio content 1"
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Pio 2 - Unread",
                Tipus = "2",
                TypeName = "pio",
                GroupName = "group_szentek",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-2),
                Idezet = "Pio content 2"
            },
            
            // Vianney content (1 item)
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Vianney 1 - Unread",
                Tipus = "20",
                TypeName = "vianney",
                GroupName = "group_szentek",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-3),
                Idezet = "Vianney content 1"
            }
        };

        foreach (var entity in entities)
        {
            await _repository.InsertContent(entity);
        }
    }

    public void Dispose()
    {
        try
        {
            if (File.Exists(_testDbPath))
            {
                File.Delete(_testDbPath);
            }
        }
        catch
        {
            // Ignore cleanup errors
        }
    }
}
