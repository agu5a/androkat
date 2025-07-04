using androkat.maui.library.Abstraction;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace androkat.maui.library.ViewModels;

public partial class ImaListViewModel : ViewModelBase
{
    private readonly IPageService _pageService;
    private bool _isInitializing;

    [ObservableProperty]
#pragma warning disable S1104 // Fields should not have public accessibility
    public string pageTitle;
#pragma warning restore S1104 // Fields should not have public accessibility

    [ObservableProperty]
    ObservableRangeCollection<ImaContentViewModel> contents;

    [ObservableProperty]
    ObservableCollection<PrayerCategory> categories;

    [ObservableProperty]
    PrayerCategory selectedCategory;

    public ImaListViewModel(IPageService pageService)
    {
        _isInitializing = true;
        _pageService = pageService;
        Contents = [];
        Categories = [];
        InitializeCategories();
        _isInitializing = false;
    }
    private void InitializeCategories()
    {
        try
        {
            // Match the Java string array exactly with correct ID mapping
            Categories.Add(new PrayerCategory { Id = -1, Name = "Összes" });        // index 0, returns -1
            Categories.Add(new PrayerCategory { Id = 11, Name = "Alapimák" });      // index 1, returns 11
            Categories.Add(new PrayerCategory { Id = 9, Name = "Napi imák" });      // index 2, returns 9
            Categories.Add(new PrayerCategory { Id = 12, Name = "Kérő és felajánló imák" }); // index 3, returns 12
            Categories.Add(new PrayerCategory { Id = 7, Name = "Hála és dicsőítő imák" });   // index 4, returns 7
            Categories.Add(new PrayerCategory { Id = 4, Name = "Litániák" });       // index 5, returns 4
            Categories.Add(new PrayerCategory { Id = 5, Name = "Saját imák" });     // index 6, returns 5
            Categories.Add(new PrayerCategory { Id = 3, Name = "Szentmise" });      // index 7, returns 3
            Categories.Add(new PrayerCategory { Id = 10, Name = "Szűz Mária" });    // index 8, returns 10
            Categories.Add(new PrayerCategory { Id = 2, Name = "Rózsafüzér" });     // index 9, returns 2
            Categories.Add(new PrayerCategory { Id = 1, Name = "Szentek imái" });   // index 10, returns 1
            Categories.Add(new PrayerCategory { Id = 0, Name = "Zsoltár" });        // index 11, returns 0
            Categories.Add(new PrayerCategory { Id = 1000, Name = "Törölt imák" }); // index 12, returns 1000

            System.Diagnostics.Debug.WriteLine($"Categories initialized with {Categories.Count} items");
            SelectedCategory = Categories[0]; // Default to "Összes"
            System.Diagnostics.Debug.WriteLine($"Default category set to: {SelectedCategory.Name}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in InitializeCategories: {ex}");
        }
    }

    partial void OnSelectedCategoryChanged(PrayerCategory value)
    {
        System.Diagnostics.Debug.WriteLine($"OnSelectedCategoryChanged called with: {value?.Name ?? "NULL"}, isInitializing: {_isInitializing}");

        if (value != null && !_isInitializing)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Category changed to: {value.Name} (ID: {value.Id})");

                // Clear contents immediately on UI thread
                Contents.Clear();

                // Then fetch new data
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await FetchAsync(1, 10);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in category change fetch: {ex}");
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnSelectedCategoryChanged: {ex}");
            }
        }
    }

    public async Task InitializeAsync(int pageNumber, int pageSize)
    {
        //Delay on first load until window loads
        await Task.Delay(1000);
        await FetchAsync(pageNumber, pageSize);
    }

    public async Task FetchAsync(int pageNumber, int pageSize)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"FetchAsync called with pageNumber: {pageNumber}, pageSize: {pageSize}");

            var categoryId = SelectedCategory?.Id ?? -1;
            System.Diagnostics.Debug.WriteLine($"Fetching prayers for category: {categoryId}, Category name: {SelectedCategory?.Name ?? "NULL"}");

            var imaContents = await _pageService.GetImaContents(pageNumber, pageSize, categoryId);

            System.Diagnostics.Debug.WriteLine($"Retrieved {imaContents.Count} prayers");

            if (imaContents.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("No prayers found - showing alert");
                await Shell.Current.DisplayAlert(
                   "Program hiba",
                    "Imák lekérdezése sikertelen, kérjük próbálja újra később.",
                    "Bezárás");

                return;
            }

            System.Diagnostics.Debug.WriteLine("Converting prayers to view models");
            var temp = ConvertToViewModels(imaContents);
            System.Diagnostics.Debug.WriteLine($"Converted {temp.Count} view models");

            System.Diagnostics.Debug.WriteLine("Adding to Contents collection");
            Contents.AddRange(temp);
            System.Diagnostics.Debug.WriteLine($"Contents collection now has {Contents.Count} items");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in FetchAsync: {ex}");
            try
            {
                await Shell.Current.DisplayAlert(
                    "Hiba",
                    $"Hiba történt az imák betöltése során: {ex.Message}",
                    "Bezárás");
            }
            catch (Exception alertEx)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing alert: {alertEx}");
            }
        }
    }

    private static List<ImaContentViewModel> ConvertToViewModels(IEnumerable<ImadsagEntity> items)
    {
        var viewmodels = new List<ImaContentViewModel>();
        foreach (var item in items)
        {
            var viewModel = new ImaContentViewModel(item)
            {
                datum = $"Dátum: {item.Datum:yyyy-MM-dd}",
                detailscim = "Imádságok",
                isFav = false,
                type = Activities.ima,
                forras = string.Empty // IMA items don't have a source
            };
            viewmodels.Add(viewModel);
        }

        return viewmodels;
    }
}