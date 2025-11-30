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
public partial class DetailViewModel(IPageService pageService, ISourceData sourceData) : ViewModelBase
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

    // Store the clean text content for sharing purposes
    private string _cleanTextContent;

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

    async Task FetchAsync()
    {
        var item = await pageService.GetContentEntityByIdAsync(_contentGuid);

        if (item == null)
        {
            await Shell.Current.DisplayAlertAsync(
                      "Hiba",
                      "Nincs tartalom",
                      "Bezárás");
            return;
        }

        SourceData idezetSource = sourceData.GetSourcesFromMemory(int.Parse(item.Tipus));

        // Store the clean text content for sharing (strip HTML but keep line breaks)
        _cleanTextContent = ConvertHtmlToPlainText(item.Idezet);

        // Add "Tovább..." link if KulsoLink is available
        var bodyContent = item.Idezet;
        if (!string.IsNullOrWhiteSpace(item.KulsoLink))
        {
            bodyContent += $"<br/><br/><a href=\"{item.KulsoLink}\">Tovább...</a>";
        }

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
                             $"</head><body>{bodyContent}</body></html>";

        var origImg = item.Image;
        item.Image = idezetSource.Img;
        var viewModel = new ContentItemViewModel(item)
        {
            datum = $"<b>Dátum</b>: {item.Datum:yyyy-MM-dd}",
            detailscim = idezetSource.Title,
            contentImg = origImg,
            isFav = false,
            forras = $"<b>Forrás</b>: {idezetSource.Forrasszoveg}",
            type = ActivitiesHelper.GetActivitiesByValue(int.Parse(item.Tipus)),
            forrasLink = idezetSource.Forras
        };
        ContentView = viewModel;
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

            await Shell.Current.DisplayAlertAsync(
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
        var result = await Shell.Current.DisplayAlertAsync(
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
                await mainPage.DisplayAlertAsync("Hiba!", "Nincs magyar nyelv telepítve a felolvasáshoz!", "OK");
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
        var shareText = ContentView.ContentEntity.Cim + "\n\n" + _cleanTextContent;

        await Share.RequestAsync(new ShareTextRequest
        {
            Title = "AndroKat: " + ContentView.ContentEntity.Cim,
            Text = shareText
        });
    }

    [RelayCommand]
    async Task OpenSourceLink()
    {
        if (!string.IsNullOrWhiteSpace(ContentView?.forrasLink))
        {
            try
            {
                await Shell.Current.GoToAsync($"WebViewPage?Url={Uri.EscapeDataString(ContentView.forrasLink)}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error opening link: {ex.Message}");
                await Shell.Current.DisplayAlertAsync("Hiba", "A link megnyitása sikertelen.", "OK");
            }
        }
    }

    private static string ConvertHtmlToPlainText(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;

        // Replace <br>, <br/>, <br />, </p>, </div> with newlines
        string text = Regex.Replace(html, @"<br\s*/?>|</p>|</div>", "\n", RegexOptions.IgnoreCase);

        // Replace <p> and <div> tags with nothing (they're block elements, handled by closing tags)
        text = Regex.Replace(text, @"<p[^>]*>|<div[^>]*>", "", RegexOptions.IgnoreCase);

        // Remove all other HTML tags
        text = Regex.Replace(text, @"<[^>]+>", "");

        // Decode HTML entities
        text = System.Net.WebUtility.HtmlDecode(text);

        // Clean up multiple consecutive newlines (more than 2)
        text = Regex.Replace(text, @"\n{3,}", "\n\n");

        // Trim whitespace from each line while preserving empty lines
        var lines = text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Trim();
        }
        text = string.Join("\n", lines);

        return text.Trim();
    }

    [GeneratedRegex("<.*?>")]
    private static partial Regex TitleRegex();
    [GeneratedRegex("<.*?>")]
    private static partial Regex IdezetRegex();
}
