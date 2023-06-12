using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.maui.library.ViewModels;

public partial class GyonasTile : ObservableObject
{
    public GyonasTile(string name, string icon)
    {
        Name = name;
        Icon = icon;
    }

    public string Name { get; }
    public string Icon { get; }

    [RelayCommand]
    Task Navigate(string page)
    {
        switch (page)
        {
            case "LELKI TÜKÖR":
                return Shell.Current.DisplayAlert("Hiba", "LELKI TÜKÖR Nincs még kész", "Bezárás");
            case "GYÓNÁS":
                return Shell.Current.DisplayAlert("Hiba", "GYÓNÁS Nincs még kész", "Bezárás");
            case "ELMÉLKEDÉS":
                return Shell.Current.DisplayAlert("Hiba", "ELMÉLKEDÉS Nincs még kész", "Bezárás");
            case "IMA":
                return Shell.Current.DisplayAlert("Hiba", "IMA Nincs még kész", "Bezárás");
            case "JEGYZET":
                return Shell.Current.DisplayAlert("Hiba", "JEGYZET Nincs még kész", "Bezárás");
            default: //TÖRLÉS
                return Shell.Current.DisplayAlert("Hiba", "TÖRLÉS Nincs még kész", "Bezárás");

        }
    }
}