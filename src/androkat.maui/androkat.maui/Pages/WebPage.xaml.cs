﻿using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class WebPage : ContentPage
{
    private WebViewModel ViewModel => BindingContext as WebViewModel;

    public WebPage(WebViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await ViewModel.InitializeAsync();
        activityIndicator.IsRunning = false;
        activityIndicator.IsVisible = false;
    }
}