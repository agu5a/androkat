using androkat.hu.Models;
using androkat.hu.Pages;
using androkat.hu.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;

namespace androkat.hu.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ShowDetailViewModel : ViewModelBase
{
    private CancellationTokenSource cts;

    public string Id { get; set; }

    private Guid showId;

    private readonly PageService _pageService;
    private readonly ISourceData _sourceData;

    [ObservableProperty]
    ContentViewModel contentView;

    [ObservableProperty]
    string textToSearch;

    public ShowDetailViewModel(PageService pageService, ISourceData sourceData)
    {
        cts = new CancellationTokenSource();
        _pageService = pageService;
        _sourceData = sourceData;
    }

    internal async Task InitializeAsync()
    {
        if (Id != null)
        {
            showId = new Guid(Id);
        }

        await FetchAsync();
    }

    async Task FetchAsync()
    {
        var item = await _pageService.GetContentDtoByIdAsync(showId);

        if (item == null)
        {
            await Shell.Current.DisplayAlert(
                      "Title",
                      "Message",
                      "Close");
            return;
        }

        SourceData idezetSource = _sourceData.GetSourcesFromMemory(int.Parse(item.Tipus));
        var origImg = item.Image;
        item.Image = idezetSource.Img;
        var showViewModel = new ContentViewModel(item, true);
        showViewModel.datum = $"<b>Dátum</b>: {item.Datum.ToString("yyyy-MM-dd")}";
        showViewModel.detailscim = idezetSource.Title;
        showViewModel.contentImg = origImg;
        showViewModel.isFav = false;
        showViewModel.forras = $"<b>Forrás</b>: {idezetSource.Forrasszoveg}";
        showViewModel.type = ActivitiesHelper.GetActivitiesByValue(int.Parse(item.Tipus));
        ContentView = showViewModel;
    }

    [RelayCommand]
    async Task AddFavorite()
    {
        var item = await _pageService.InsertFavoriteContentAsync(new FavoriteContentDto
        {
            Cim = ContentView.ContentDto.Cim,
            Datum = ContentView.ContentDto.Datum,
            Forras = ContentView.ContentDto.Forras,
            Image = ContentView.ContentDto.Image,
            KulsoLink = ContentView.ContentDto.KulsoLink,
            Idezet = ContentView.ContentDto.Idezet,
            Nid = ContentView.ContentDto.Nid,
            Tipus = ContentView.ContentDto.Tipus,
            TypeName = ContentView.ContentDto.TypeName 
        });
    }

    [RelayCommand]
    async Task StartTextToSpeech()
    {
        /*if (item.getItemId() == R.id.speak)
            {
                string toSpeak = forrasTitle.getText().toString() + ". " + titleTW.getText().toString() + ". " + contentTw.getText().toString();
                t1.speak(toSpeak, TextToSpeech.QUEUE_FLUSH, null, null);
            }*/
        /*drawable = menu.findItem(R.id.speak).getIcon();
                drawable = DrawableCompat.wrap(drawable);
                DrawableCompat.setTint(drawable, ContextCompat.getColor(this, R.color.secondary));
                menu.findItem(R.id.speak).setIcon(drawable);*/


        var title = Regex.Replace(ContentView.ContentDto.Cim, "<.*?>", String.Empty);
        var idezet = Regex.Replace(ContentView.ContentDto.Idezet, "<.*?>", String.Empty);
        IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();

        var locale = locales.Where(w => w.Country.ToLower() == "hu" && w.Language.ToLower() == "hu").FirstOrDefault();
        if (locale == null)
        {
            await Application.Current.MainPage.DisplayAlert("Hiba!", "Nincs magyar nyelv telepítve a felolvasáshoz!", "OK");
            return;
        }

        int max = 2000;
        string toSpeak = title + ". " + idezet;

        if (toSpeak.Length < max)
        {
            await TextToSpeech.Default.SpeakAsync(title + ". " + idezet, new SpeechOptions { Locale = locale }, cancelToken: cts.Token);
            return;
        }

        isBusy = true;

        var task = new List<Task>
            {
                TextToSpeech.Default.SpeakAsync(title, new SpeechOptions { Locale = locale }, cancelToken: cts.Token)
            };

        if (idezet.Contains('.'))
        {
            string[] sep = idezet.Split('.');
            for (int i = 0; i < sep.Length; i++)
            {
                string temp = sep[i] + ".";
                task.Add(TextToSpeech.Default.SpeakAsync(temp, new SpeechOptions { Locale = locale }, cancelToken: cts.Token));
            }
        }
        else
            task.Add(TextToSpeech.Default.SpeakAsync(idezet, new SpeechOptions { Locale = locale }, cancelToken: cts.Token));

        await Task.WhenAll(task).ContinueWith((t) => { isBusy = false; }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    bool isBusy = false;

    public void CancelSpeech()
    {
        if (cts?.IsCancellationRequested ?? true)
            return;

        cts.Cancel();
    }

    [RelayCommand]
    async Task Subscribe()
    {
        await Share.RequestAsync(new ShareTextRequest
        {
            Title = "AndroKat: " + ContentView.ContentDto.Cim,
            Text = ContentView.ContentDto.Idezet
        });
    }
}
