#nullable enable
using androkat.maui.library.Data;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace androkat.maui.unittest.Services;

public class FilteringTests
{
    [Fact]
    public void ApplyFilters_WithNullEnabledSources_ShouldReturnAllResults()
    {
        // Arrange
        var testData = CreateTestContentEntities();

        // Act
        var result = InvokeApplyFilters(testData, true, null);

        // Assert
        Assert.Equal(testData.Count, result.Count);
        Assert.Equal(testData, result);
    }

    [Fact]
    public void ApplyFilters_WithEmptyEnabledSources_ShouldReturnEmptyList()
    {
        // Arrange
        var testData = CreateTestContentEntities();
        var enabledSources = new List<string>();

        // Act
        var result = InvokeApplyFilters(testData, true, enabledSources);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ApplyFilters_WithSpecificSources_ShouldFilterByTipusField()
    {
        // Arrange
        var testData = CreateTestContentEntities();
        var enabledSources = new List<string> { "13", "14" }; // barsi and horvath

        // Act
        var result = InvokeApplyFilters(testData, true, enabledSources);

        // Assert
        Assert.Equal(3, result.Count); // 2 barsi + 1 horvath = 3 items
        Assert.All(result, item => Assert.Contains(item.Tipus, enabledSources));
    }

    [Fact]
    public void ApplyFilters_WithReadVisitedFalse_ShouldFilterOutReadItems()
    {
        // Arrange
        var testData = CreateTestContentEntities();

        // Act
        var result = InvokeApplyFilters(testData, false, null);

        // Assert
        var unreadItems = testData.Where(x => !x.IsRead).ToList();
        Assert.Equal(unreadItems.Count, result.Count);
        Assert.All(result, item => Assert.False(item.IsRead));
    }

    [Fact]
    public void ApplyFilters_WithSourceFilterAndReadFilter_ShouldApplyBothFilters()
    {
        // Arrange
        var testData = CreateTestContentEntities();
        var enabledSources = new List<string> { "13" }; // Only barsi

        // Act
        var result = InvokeApplyFilters(testData, false, enabledSources);

        // Assert
        var expectedItems = testData.Where(x =>
            enabledSources.Contains(x.Tipus) && !x.IsRead).ToList();
        Assert.Equal(expectedItems.Count, result.Count);
        Assert.All(result, item =>
        {
            Assert.Contains(item.Tipus, enabledSources);
            Assert.False(item.IsRead);
        });
    }

    [Fact]
    public void ApplyFilters_WithNonMatchingSources_ShouldReturnEmptyList()
    {
        // Arrange
        var testData = CreateTestContentEntities();
        var enabledSources = new List<string> { "999" }; // Non-existent source

        // Act
        var result = InvokeApplyFilters(testData, true, enabledSources);

        // Assert
        Assert.Empty(result);
    }

    [Theory]
    [InlineData("13")] // barsi
    [InlineData("14")] // horvath
    [InlineData("7")]  // fokolare
    public void ApplyFilters_WithSingleSource_ShouldReturnOnlyMatchingItems(string sourceId)
    {
        // Arrange
        var testData = CreateTestContentEntities();
        var enabledSources = new List<string> { sourceId };

        // Act
        var result = InvokeApplyFilters(testData, true, enabledSources);

        // Assert
        var expectedItems = testData.Where(x => x.Tipus == sourceId).ToList();
        Assert.Equal(expectedItems.Count, result.Count);
        Assert.All(result, item => Assert.Equal(sourceId, item.Tipus));
    }

    [Fact]
    public void FilterOptionsHelper_GeneratesCorrectKeysForNapiOlvaso()
    {
        // Act
        var options = FilterOptionsHelper.GetFilterOptionsForPageId("0");

        // Assert
        Assert.NotEmpty(options);

        // Verify that keys are integer strings matching Activity enum values
        var barsiOption = options.FirstOrDefault(o => o.Activity == Activities.barsi);
        Assert.NotNull(barsiOption);
        Assert.Equal(((int)Activities.barsi).ToString(), barsiOption.Key);

        var horvathOption = options.FirstOrDefault(o => o.Activity == Activities.horvath);
        Assert.NotNull(horvathOption);
        Assert.Equal(((int)Activities.horvath).ToString(), horvathOption.Key);
    }

    [Fact]
    public void FilterOptionsHelper_GeneratesCorrectKeysForSzentek()
    {
        // Act
        var options = FilterOptionsHelper.GetFilterOptionsForPageId("3");

        // Assert
        Assert.NotEmpty(options);

        var pioOption = options.FirstOrDefault(o => o.Activity == Activities.pio);
        Assert.NotNull(pioOption);
        Assert.Equal(((int)Activities.pio).ToString(), pioOption.Key);
    }

    [Theory]
    [InlineData("0")] // Napi olvaso
    [InlineData("3")] // Szentek
    [InlineData("4")] // News
    [InlineData("5")] // Magazin
    [InlineData("8")] // Audio
    public void FilterOptionsHelper_ReturnsFilterOptionsForValidPageIds(string pageId)
    {
        // Act
        var options = FilterOptionsHelper.GetFilterOptionsForPageId(pageId);

        // Assert
        Assert.NotEmpty(options);
        Assert.All(options, option =>
        {
            Assert.NotEmpty(option.Key);
            Assert.NotEmpty(option.DisplayName);
            Assert.True(int.TryParse(option.Key, out _), $"FilterOption key '{option.Key}' should be a valid integer");
        });
    }

    [Fact]
    public void FilterOptionsHelper_ReturnsEmptyForInvalidPageId()
    {
        // Act
        var options = FilterOptionsHelper.GetFilterOptionsForPageId("999");

        // Assert
        Assert.Empty(options);
    }

    private static List<ContentEntity> CreateTestContentEntities()
    {
        return new List<ContentEntity>
        {
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Barsi Test 1",
                Tipus = "13", // barsi
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-1)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Barsi Test 2",
                Tipus = "13", // barsi
                TypeName = "barsi",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-2)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Horvath Test",
                Tipus = "14", // horvath
                TypeName = "horvath",
                GroupName = "group_napiolvaso",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-3)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Fokolare Test",
                Tipus = "7", // fokolare
                TypeName = "fokolare",
                GroupName = "group_napiolvaso",
                IsRead = true,
                Datum = DateTime.Now.AddDays(-4)
            },
            new()
            {
                Nid = Guid.NewGuid(),
                Cim = "Pio Test",
                Tipus = "2", // pio
                TypeName = "pio",
                GroupName = "group_szentek",
                IsRead = false,
                Datum = DateTime.Now.AddDays(-5)
            }
        };
    }

    private static List<ContentEntity> InvokeApplyFilters(List<ContentEntity> results, bool returnVisited, List<string>? enabledSources)
    {
        // Use reflection to call the private static ApplyFilters method
        var repositoryType = typeof(Repository);
        var applyFiltersMethod = repositoryType.GetMethod("ApplyFilters", BindingFlags.NonPublic | BindingFlags.Static);

        if (applyFiltersMethod == null)
        {
            throw new InvalidOperationException("ApplyFilters method not found");
        }

        var result = applyFiltersMethod.Invoke(null, new object?[] { results, returnVisited, enabledSources });
        return (List<ContentEntity>)result!;
    }
}
