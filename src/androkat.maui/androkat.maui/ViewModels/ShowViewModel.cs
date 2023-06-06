using androkat.hu.Models;
using androkat.hu.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

public partial class ShowViewModel : ObservableObject
{
    public Show Show { get; set; }

    [ObservableProperty]
    bool isSubscribed;

    public IEnumerable<Episode> Episodes => Show?.Episodes;

    public Uri Image => Show?.Image;

    public string Author => Show?.Author;

    public string Title => Show?.Title;

    public string Description => Show?.Description;

    public ShowViewModel(Show show, bool isSubscribed)
    {
        Show = show;
        IsSubscribed = isSubscribed;
    }

    [RelayCommand]
    Task NavigateToDetail() => Shell.Current.GoToAsync($"{nameof(ShowDetailPage)}?Id={Show.Id}");
}