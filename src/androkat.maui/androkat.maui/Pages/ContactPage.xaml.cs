using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class ContactPage : ContentPage
{
    public ContactPage(ContactViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "S1075:URIs should not be hardcoded", Justification = "Contact page social links")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Event handler cannot be static")]
    private async void OnFacebookClicked(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("https://www.facebook.com/androkat", BrowserLaunchMode.SystemPreferred);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Event handler cannot be static")]
    private async void OnEmailClicked(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("mailto:uzenet@androkat.hu", BrowserLaunchMode.SystemPreferred);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "S1075:URIs should not be hardcoded", Justification = "Contact page social links")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Event handler cannot be static")]
    private async void OnTwitterClicked(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("https://twitter.com/AndroKat", BrowserLaunchMode.SystemPreferred);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "S1075:URIs should not be hardcoded", Justification = "Contact page social links")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Event handler cannot be static")]
    private async void OnYouTubeClicked(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("https://www.youtube.com/channel/UCF3mEbdkhZwjQE8reJHm4sg", BrowserLaunchMode.SystemPreferred);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "S1075:URIs should not be hardcoded", Justification = "Contact page social links")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Event handler cannot be static")]
    private async void OnInstagramClicked(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("https://www.instagram.com/androkat_app/", BrowserLaunchMode.SystemPreferred);
    }
}