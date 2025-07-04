using androkat.maui.library.Abstraction;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Text.RegularExpressions;

namespace androkat.maui.library.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
[QueryProperty(nameof(FromFavorites), nameof(FromFavorites))]
public partial class PrayDetailViewModel(IPageService pageService) : ViewModelBase
{
    private Guid _contentGuid;

    public string Id { get; set; }
    public string FromFavorites { get; set; }

    [ObservableProperty]
    ContentItemViewModel contentView;

    [ObservableProperty]
    bool showDeleteFavoriteButton;

    [ObservableProperty]
    bool isAlreadyFavorited;

    [ObservableProperty]
    bool isFavoriteCheckCompleted;

    public async Task InitializeAsync()
    {
        if (Id != null)
        {
            _contentGuid = new Guid(Id);
        }
        if (FromFavorites != null)
        {
            ShowDeleteFavoriteButton = bool.Parse(FromFavorites);
        }

        await FetchAsync();
        await CheckIfAlreadyFavoritedAsync();
    }

    private async Task CheckIfAlreadyFavoritedAsync()
    {
        var favorites = await pageService.GetFavoriteContentsAsync();
        IsAlreadyFavorited = favorites.Any(f => f.Nid == _contentGuid);
        IsFavoriteCheckCompleted = true;

        // Notify the command that its CanExecute state has changed
        AddFavoriteCommand.NotifyCanExecuteChanged();
    }

    private async Task FetchAsync()
    {
        var ima = await pageService.GetImadsagEntityByIdAsync(_contentGuid);
        if (ima == null)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert(
                              "Hiba",
                              "Nincs tartalom",
                              "Bezárás");
            }
            return;
        }

        var viewModelIma = new ContentItemViewModel(new ContentEntity
        {
            Cim = ima.Cim,
            Idezet = ima.Content,
            Tipus = ((int)Activities.ima).ToString(),
            Nid = ima.Nid,
            Datum = ima.Datum,
            TypeName = Activities.ima.ToString()
        })
        {
            datum = "",
            detailscim = ima.Cim,
            isFav = false,
            forras = "",
            type = Activities.ima
        };

        ContentView = viewModelIma;
    }

    [RelayCommand(CanExecute = nameof(CanAddFavorite))]
    async Task AddFavorite()
    {
        var result = await pageService.InsertFavoriteContentAsync(new FavoriteContentEntity
        {
            Cim = ContentView.ContentEntity.Cim,
            Datum = ContentView.ContentEntity.Datum,
            Forras = ContentView.ContentEntity.Forras,
            Image = ContentView.ContentEntity.Image,
            KulsoLink = ContentView.ContentEntity.KulsoLink,
            Idezet = ContentView.ContentEntity.Idezet,
            Nid = ContentView.ContentEntity.Nid,
            Tipus = ContentView.ContentEntity.Tipus,
            TypeName = ContentView.ContentEntity.TypeName
        });

        if (result > 0)
        {
            IsAlreadyFavorited = true;
            AddFavoriteCommand.NotifyCanExecuteChanged();

            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert(
                    "Sikeres művelet",
                    "A tartalom sikeresen hozzáadva a kedvencekhez!",
                    "OK");
            }
        }
    }

    private bool CanAddFavorite()
    {
        // Disable the button until we've checked if it's already favorited
        // and only enable it if it's not already favorited
        return IsFavoriteCheckCompleted && !IsAlreadyFavorited;
    }

    [RelayCommand]
    async Task DeleteFavorite()
    {
        if (Shell.Current == null) return;

        var result = await Shell.Current.DisplayAlert(
            "Törlés megerősítése",
            "Biztosan törölni szeretnéd ezt a kedvencet?",
            "Igen", "Nem");

        if (result)
        {
            _ = await pageService.DeleteFavoriteContentByNid(_contentGuid);

            // Update favorited status
            IsAlreadyFavorited = false;
            AddFavoriteCommand.NotifyCanExecuteChanged();

            // Navigate back to favorites page
            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    async Task StartTextToSpeech()
    {
        var title = TitleRegex().Replace(ContentView.ContentEntity.Cim, String.Empty);
        var idezet = IdezetRegex().Replace(ContentView.ContentEntity.Idezet, String.Empty);
        IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();

        var locale = locales.FirstOrDefault(w => w.Country.Equals("hu", StringComparison.CurrentCultureIgnoreCase) && w.Language.Equals("hu", StringComparison.CurrentCultureIgnoreCase));
        if (locale == null)
        {
            var mainPage = Application.Current?.Windows[0].Page;
            if (mainPage != null)
            {
                await mainPage.DisplayAlert("Hiba!", "Nincs magyar nyelv telepítve a felolvasáshoz!", "OK");
                return;
            }
        }

        int max = 2000;
        string toSpeak = title + ". " + idezet;

        if (toSpeak.Length < max)
        {
            await TextToSpeech.Default.SpeakAsync(title + ". " + idezet, new SpeechOptions { Locale = locale }, cancelToken: _cancellationTokenSource.Token);
            return;
        }

        var task = new List<Task>
        {
            TextToSpeech.Default.SpeakAsync(title, new SpeechOptions { Locale = locale }, cancelToken: _cancellationTokenSource.Token)
        };

        if (idezet.Contains('.'))
        {
            string[] sep = idezet.Split('.');
            for (int i = 0; i < sep.Length; i++)
            {
                string temp = sep[i] + ".";
                task.Add(TextToSpeech.Default.SpeakAsync(temp, new SpeechOptions { Locale = locale }, cancelToken: _cancellationTokenSource.Token));
            }
        }
        else
            task.Add(TextToSpeech.Default.SpeakAsync(idezet, new SpeechOptions { Locale = locale }, cancelToken: _cancellationTokenSource.Token));

        await Task.WhenAll(task).ContinueWith((t) => { /*isBusy*/ }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void CancelSpeech()
    {
        if (_cancellationTokenSource?.IsCancellationRequested ?? true)
            return;

        _cancellationTokenSource.Cancel();
    }

    [RelayCommand]
    async Task ShareContent()
    {
        await Share.RequestAsync(new ShareTextRequest
        {
            Title = "AndroKat: " + ContentView.ContentEntity.Cim,
            Text = ContentView.ContentEntity.Idezet
        });
    }

    [GeneratedRegex("<.*?>")]
    private static partial Regex TitleRegex();
    [GeneratedRegex("<.*?>")]
    private static partial Regex IdezetRegex();
}
