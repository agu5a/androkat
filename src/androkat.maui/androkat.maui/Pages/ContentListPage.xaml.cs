﻿using androkat.hu.Views;
using androkat.maui.library.ViewModels;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models;
using CommunityToolkit.Maui.Extensions;

namespace androkat.hu.Pages;

public partial class ContentListPage
{
    private ContentListViewModel ViewModel => (BindingContext as ContentListViewModel)!;
    private int _stackCount;
    private static bool ShowVisited => Settings.ShowVisited;

    public ContentListPage(ContentListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // Hack: Get the category Id
        var result = GetCategoryIdFromRoute();
        ViewModel.Id = string.IsNullOrEmpty(result) ? "34" : result;
        ViewModel.PageTitle = GetPageTitle(ViewModel.Id);
        if (_stackCount != 2)
        {
            //Nem DetailPage-ről jöttünk viszsa, így üres oldallal indulunk
            ViewModel.Contents.Clear();
        }
        else
        {
            // Visszajövünk DetailPage-ről, újra fetcheljük az adatokat hogy a read status frissüljön
            var filterOptions = FilterOptionsHelper.GetFilterOptionsForPageId(ViewModel.Id);
            var enabledSources = filterOptions.Count > 0 ? Settings.GetEnabledSources(filterOptions) : null;
            Task.Run(async () => await ViewModel.FetchAsync(ShowVisited, enabledSources));
        }
        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        _stackCount = Application.Current!.Windows[0].Page!.Navigation.NavigationStack.Count;
        base.OnNavigatedFrom(args);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        activityIndicator.IsRunning = true;
        activityIndicator.IsVisible = true;
        // Get the current page ID from route to ensure we have the correct ID
        var currentPageId = GetCategoryIdFromRoute();
        var pageId = string.IsNullOrEmpty(currentPageId) ? "34" : currentPageId;
        var filterOptions = FilterOptionsHelper.GetFilterOptionsForPageId(pageId);
        var enabledSources = filterOptions.Count > 0 ? Settings.GetEnabledSources(filterOptions) : null;
        await ViewModel.InitializeAsync(ShowVisited, enabledSources);
        activityIndicator.IsRunning = false;
        activityIndicator.IsVisible = false;
    }

    private async void OnFilterClicked(object sender, EventArgs e)
    {
        var filterView = new FilterView(ShowVisited, ViewModel.Id ?? "0");
        filterView.FilterChanged += async (_, args) =>
        {
            Settings.ShowVisited = args.ShowVisited;
            await ViewModel.FetchAsync(args.ShowVisited, args.EnabledSources);
        };

        await this.ShowPopupAsync(filterView);
    }

    private static string GetPageTitle(string pageTypeId)
    {
        return pageTypeId switch
        {
            "1" => "KÖNYVAJÁNLÓ",
            "2" => "Mai Szent",
            "3" => "Szentek idézetei",
            "4" => "Katolikus Hírek",
            "5" => "Blog, magazin",
            "6" => "Nevessünk!",
            "7" => "Imádságok",
            "8" => "Hanganyagok",
            "11" => "Könyvolvasó",
            "34" => "Gyónáshoz elmélkedés",
            //Weboldalak
            _ => "Evangélium, elmélkedés", //0
        };
    }

    private static string GetCategoryIdFromRoute()
    {
        // Hack: As the shell can't define query parameters
        // in XAML, we have to parse the route. 
        // as a convention the last route section defines the category.
        // ugly but works for now :-(
        var result = Shell.Current.CurrentState.Location.OriginalString.Split("/").LastOrDefault()!;
        return int.TryParse(result, out _) ? result : "";
    }
}