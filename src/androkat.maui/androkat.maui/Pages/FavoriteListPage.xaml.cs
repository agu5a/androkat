using Android.OS;
using androkat.maui.library.Abstraction;
using androkat.maui.library.Models.Entities;
using androkat.maui.library.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace androkat.hu.Pages;

public partial class FavoriteListPage : ContentPage
{
    private readonly IPageService _pageService;
    private int _stackCount = 0;
    private FavoriteListViewModel ViewModel => BindingContext as FavoriteListViewModel;

    public FavoriteListPage(FavoriteListViewModel viewModel, IPageService pageService)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _pageService = pageService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        ViewModel.PageTitle = GetPageTitle();
        if (_stackCount != 2)
        {
            //Nem DetailPage-ről jöttünk viszsa, így üres oldallal indulunk
            ViewModel.Contents.Clear();
        }
        await ViewModel.InitializeAsync();
        base.OnNavigatedTo(args);
    }

    private static readonly string[] valueArray = ["application/json"];
    private static readonly string[] value = ["application/json"];

    private string GetPageTitle()
    {
        if (ViewModel.FavoriteCount > 0)
            return $"Kedvencek ({ViewModel.FavoriteCount})";
        else
            return $"Kedvencek";
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        _stackCount = Application.Current.MainPage.Navigation.NavigationStack.Count;
        base.OnNavigatedFrom(args);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var isDelete = await Shell.Current.DisplayAlert("Törlés", "Biztos törlöd az összes kedvencet?", "Igen", "Nem");
        if (isDelete)
        {
            await _pageService.DeleteAllFavorite();
            ViewModel.PageTitle = "Kedvencek";
            await ViewModel.InitializeAsync();

            using var cancellationTokenSource = new CancellationTokenSource();
            var toast = Toast.Make("Kedvencek adatbázis sikeresen törölve", ToastDuration.Short, 14d);
            await toast.Show(cancellationTokenSource.Token);
        }
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        // Check if the platform supports file picking
        //if (!await FilePicker.IsPickingSupportedAsync())
        //
        //    await DisplayAlert("Error", "File picking is not supported on this platform.", "OK")
        //    return
        //

        if (Build.VERSION.SdkInt < BuildVersionCodes.R)
        {
            var storagePermission = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (storagePermission == PermissionStatus.Unknown || storagePermission == PermissionStatus.Denied)
            {
                await DisplayAlert("Error", "Permission Denied", "OK");
            }
        }

        var savePath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath,
            "AndroKat", "kedvencek.json");

        if (!File.Exists(savePath))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                File.Create(savePath);
                await File.WriteAllTextAsync(savePath, "[]");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        var jsonFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> { { DevicePlatform.Android, value } });

        var fileResult = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = jsonFileType,
            PickerTitle = "Válasszd ki a kedvencek.json fájlt"
        });

        if (fileResult != null)
        {
            var list = await ViewModel.GetFavContentsAsync();
            string json = System.Text.Json.JsonSerializer.Serialize(list);
            await File.WriteAllTextAsync(savePath, json);

            using var cancellationTokenSource = new CancellationTokenSource();
            var toast = Toast.Make("Mentés sikerült", ToastDuration.Short, 14d);
            await toast.Show(cancellationTokenSource.Token);
        }
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        var jsonFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> { { DevicePlatform.Android, valueArray } });

        var fileResult = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = jsonFileType,
            PickerTitle = "Válasszd ki a kedvencek.json fájlt"
        });

        if (fileResult != null)
        {
            string fileContent = await File.ReadAllTextAsync(fileResult.FullPath);
            var o = System.Text.Json.JsonSerializer.Deserialize<List<FavoriteContentEntity>>(fileContent);

            using var cancellationTokenSource = new CancellationTokenSource();
            var toast = Toast.Make("Olvasás: " + o.Count, ToastDuration.Short, 14d);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
}