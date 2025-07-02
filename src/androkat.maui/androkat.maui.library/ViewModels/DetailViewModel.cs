using androkat.maui.library.Abstraction;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Text.RegularExpressions;

namespace androkat.maui.library.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
[QueryProperty(nameof(IsIma), nameof(IsIma))]
[QueryProperty(nameof(FromFavorites), nameof(FromFavorites))]
public partial class DetailViewModel(IPageService pageService, ISourceData sourceData) : ViewModelBase
{
    private Guid _contentGuid;
    private bool _isIma = false;

    public string Id { get; set; }
    public string IsIma { get; set; }
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
        if (IsIma != null)
        {
            _isIma = bool.Parse(IsIma);
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

    async Task FetchAsync()
    {
        if (!_isIma)
        {
            var item = await pageService.GetContentEntityByIdAsync(_contentGuid);

            if (item == null)
            {
                await Shell.Current.DisplayAlert(
                          "Hiba",
                          "Nincs tartalom",
                          "Bezárás");
                return;
            }
            item.KulsoLink = item.Idezet; // idejön a http link

            item.Idezet = "<html><!DOCTYPE html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />"
                                 + "<script type=\"text/javascript\">" +
                                 "document.addEventListener(\"DOMContentLoaded\", function() {" +
                                 "var links = document.getElementsByTagName(\"a\");" +
                                 "for (var i = 0; i < links.length; i++) {" +
                                 "links[i].addEventListener(\"click\", function(event) {" +
                                 "event.preventDefault(); " +
                                 "var href = this.getAttribute(\"href\");" +
                                 "if (href && href !== \"#\") {" +
                                 "window.location.href = \"appscheme://\" + href;" +
                                 "}});}});</script>" +
                                 "</head><body>" + item.Idezet + "</body></html>";

            SourceData idezetSource = sourceData.GetSourcesFromMemory(int.Parse(item.Tipus));
            var origImg = item.Image;
            item.Image = idezetSource.Img;
            var viewModel = new ContentItemViewModel(item)
            {
                datum = $"<b>Dátum</b>: {item.Datum:yyyy-MM-dd}",
                detailscim = idezetSource.Title,
                contentImg = origImg,
                isFav = false,
                forras = $"<b>Forrás</b>: {idezetSource.Forrasszoveg}",
                type = ActivitiesHelper.GetActivitiesByValue(int.Parse(item.Tipus))
            };
            ContentView = viewModel;
            return;
        }

        var ima = await pageService.GetImadsagEntityByIdAsync(_contentGuid);
        if (ima == null)
        {
            await Shell.Current.DisplayAlert(
                          "Hiba",
                          "Nincs tartalom",
                          "Bezárás");
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
            //contentImg = ima.Image,
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

            await Shell.Current.DisplayAlert(
                "Sikeres művelet",
                "A tartalom sikeresen hozzáadva a kedvencekhez!",
                "OK");
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
        /* if (item.getItemId() R.id.speak)
            
                string toSpeak = forrasTitle.getText().toString() + ". " + titleTW.getText().toString() + ". " + contentTw.getText().toString()
                t1.speak(toSpeak, TextToSpeech.QUEUE_FLUSH, null, null)
            
            drawable = menu.findItem(R.id.speak).getIcon()
                drawable = DrawableCompat.wrap(drawable)
                DrawableCompat.setTint(drawable, ContextCompat.getColor(this, R.color.secondary))
                menu.findItem(R.id.speak).setIcon(drawable)*/


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

        /*isBusy true*/

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
