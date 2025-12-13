using System;
using androkat.hu.Views;
using androkat.maui.library.ViewModels;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models;
using CommunityToolkit.Maui.Extensions;

namespace androkat.hu.Pages;

public partial class ContentListPage
{
    private ContentListViewModel ViewModel => (BindingContext as ContentListViewModel)!;
    private int _stackCount;
    private bool _isInitialized;
    private static bool ShowVisited => Settings.ShowVisited;

    public ContentListPage(ContentListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // Hack: Get the category Id
        var result = GetCategoryIdFromRoute();
        var pageId = string.IsNullOrEmpty(result) ? "34" : result;
        var categoryChanged = !string.Equals(ViewModel.Id, pageId, StringComparison.Ordinal);
        ViewModel.Id = pageId;
        ViewModel.PageTitle = GetPageTitle(ViewModel.Id);

        if (!_isInitialized)
        {
            base.OnNavigatedTo(args);
            return;
        }

        var returningFromDetail = _stackCount == 2 && !categoryChanged;
        var currentStackCount = Application.Current!.Windows[0].Page!.Navigation.NavigationStack.Count;
        System.Diagnostics.Debug.WriteLine($"OnNavigatedTo: {(returningFromDetail ? "Coming back from DetailPage" : "Not coming back from DetailPage")}, stack count: {currentStackCount}");

        if (!returningFromDetail)
        {
            //Nem DetailPage-ről jöttünk viszsa, így üres oldallal indulunk
            ViewModel.Contents.Clear();
        }

        var enabledSources = ResolveEnabledSources(pageId);
        await ViewModel.FetchAsync(ShowVisited, enabledSources);

        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        System.Diagnostics.Debug.WriteLine("OnNavigatedFrom, navigation stack count: " + Application.Current!.Windows[0].Page!.Navigation.NavigationStack.Count);

        _stackCount = Application.Current!.Windows[0].Page!.Navigation.NavigationStack.Count;
        base.OnNavigatedFrom(args);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_isInitialized)
        {
            return;
        }
        activityIndicator.IsRunning = true;
        activityIndicator.IsVisible = true;
        // Get the current page ID from route to ensure we have the correct ID
        var currentPageId = GetCategoryIdFromRoute();
        var pageId = string.IsNullOrEmpty(currentPageId) ? "34" : currentPageId;
        ViewModel.Id = pageId;
        ViewModel.PageTitle = GetPageTitle(pageId);
        var enabledSources = ResolveEnabledSources(pageId);
        await ViewModel.InitializeAsync(ShowVisited, enabledSources);
        _isInitialized = true;
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

    private static List<string>? ResolveEnabledSources(string pageId)
    {
        var filterOptions = FilterOptionsHelper.GetFilterOptionsForPageId(pageId);
        return filterOptions.Count > 0 ? Settings.GetEnabledSources(filterOptions) : null;
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

        System.Diagnostics.Debug.WriteLine("Current route: " + Shell.Current.CurrentState.Location.OriginalString);

        var result = Shell.Current.CurrentState.Location.OriginalString.Split("/").LastOrDefault()!;
        return int.TryParse(result, out _) ? result : "";
    }
}