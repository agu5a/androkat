using androkat.maui.library.Models;
using androkat.maui.library.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;

namespace androkat.hu.Pages;

public partial class DetailAudio
{
    private DetailViewModel ViewModel => (BindingContext as DetailViewModel)!;
    private string _downloadedFilePath = string.Empty;
    private ToolbarItem? _deleteFavoriteToolbarItem;

    public DetailAudio(DetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
        TitleLabel.Text = ViewModel.ContentView.detailscim;
        Leiras1.Text = ViewModel.ContentView.ContentEntity.Cim;
        Leiras2.Text = ViewModel.ContentView.datum;

        // Control toolbar item visibility
        UpdateToolbarItems();

        var leiras = ViewModel.ContentView.type switch
        {
            Activities.audiopalferi =>
                "Pál Feri atya: Vasárnapi-beszédek<br><a href=\"https://www.youtube.com/c/P%C3%A1lferiOnline/videos\">www.youtube.com</a>",
            Activities.audiohorvath =>
                "Horváth István Sándor atya (evangelium365.hu): napi evangélium és elmélkedések<br><a href=\"https://evangelium365.hu/\">evangelium365.hu</a>",
            Activities.audiobarsi =>
                "Balázs atya hétvégi szentbeszédei<br><a href=\"https://www.youtube.com/channel/UC_aC_9qFjPI5U0JrznoyYRA\">Barsi Balázs youtube csatorna</a>",
            Activities.prayasyougo =>
                "A <a href=\"https://open.spotify.com/show/4f2oWtBVw0jnTmeDrHh7ey\">jezsuiták minden napra összeállítanak</a> egy 10-15 perces elmélkedést, mely a napi olvasmányhoz kapcsolódik. Ezeket a hanganyagokat - a mai kor igényeihez és lehet&#x0151;ségeihez igazodva - mobiltelefonra, vagy bármilyen MP3-lejátszóra letöltve akár utazás közben is hallgathatjuk.",
            Activities.audiotaize =>
                "<a href=\"https://www.taize.fr/en/prayer-and-reflection\">www.taize.fr/en/prayer-and-reflection</a>",
            Activities.audionapievangelium =>
                "A <a href=\"https://www.katolikusradio.hu/evangelium\">katolikusradio.hu</a> által közétett napi evangélium hanganyagként.",
            _ => ""
        };

        Leiras3.Text = leiras + "<br><br><b>Töltse le</b> vagy <b>hallgassa meg most</b>";

        // Check if file already exists and update button states
        CheckFileExistsAndUpdateButtons();
    }

    private void CheckFileExistsAndUpdateButtons()
    {
        try
        {
            string? url = GetAudioUrl();
            if (!string.IsNullOrEmpty(url))
            {
                string fileName = GetFileName(url, ViewModel!.ContentView.type);
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string androkatFolder = Path.Combine(documentsPath, "AndroKat");
                string filePath = Path.Combine(androkatFolder, fileName);

                if (File.Exists(filePath))
                {
                    _downloadedFilePath = filePath;
                    DownloadButton.IsEnabled = false;
                    DownloadButton.Text = "MÁR LETÖLTVE";
                    DeleteButton.IsEnabled = true;
                }
                else
                {
                    DownloadButton.IsEnabled = true;
                    DownloadButton.Text = "LETÖLTÉS";
                    DeleteButton.IsEnabled = false;
                }
            }
        }
        catch (Exception)
        {
            // If there's an error checking files, just enable download
            DownloadButton.IsEnabled = true;
            DownloadButton.Text = "LETÖLTÉS";
            DeleteButton.IsEnabled = false;
        }
    }

    protected override void OnDisappearing()
    {
        ViewModel.CancelSpeech();
        base.OnDisappearing();
    }

    private async void Letoltes_OnClicked(object? sender, EventArgs e)
    {
        string? audioUrl = GetAudioUrl();
        if (string.IsNullOrEmpty(audioUrl))
        {
            await DisplayAlertAsync("Hiba", "Nincs elérhető hangfájl URL", "OK");
            return;
        }

        try
        {
            // Show progress indicators
            var progressLayoutControl = (VerticalStackLayout)this.FindByName("progressLayout");
            if (progressLayoutControl != null)
            {
                progressLayoutControl.IsVisible = true;
            }
            ProgressBar.IsVisible = true;

            string url = audioUrl;
            string fileName = GetFileName(url, ViewModel!.ContentView.type);

            // Download file
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AndroKat", "03.58"));
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string androkatFolder = Path.Combine(documentsPath, "AndroKat");

                // Create directory if it doesn't exist
                Directory.CreateDirectory(androkatFolder);

                _downloadedFilePath = Path.Combine(androkatFolder, fileName);
                await File.WriteAllBytesAsync(_downloadedFilePath, bytes);

                await DisplayAlertAsync("Siker", $"Letöltés sikeres!\nHelye: {_downloadedFilePath}", "OK");

                // Update button states
                var downloadButton = sender as Button;
                if (downloadButton != null)
                {
                    downloadButton.IsEnabled = false;
                    downloadButton.Text = "MÁR LETÖLTVE";
                }

                // Enable delete button
                DeleteButton.IsEnabled = true;
            }
            else
            {
                await DisplayAlertAsync("Hiba", "Nem sikerült letölteni a fájlt", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hiba", $"Letöltési hiba: {ex.Message}", "OK");
        }
        finally
        {
            // Hide progress indicators
            var progressLayoutControl = (VerticalStackLayout)this.FindByName("progressLayout");
            if (progressLayoutControl != null)
            {
                progressLayoutControl.IsVisible = false;
            }
            ProgressBar.IsVisible = false;
        }
    }

    private async void Torles_OnClicked(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(_downloadedFilePath))
            {
                // Try to find the file
                string? url = GetAudioUrl();
                if (!string.IsNullOrEmpty(url))
                {
                    string fileName = GetFileName(url, ViewModel!.ContentView.type);
                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string androkatFolder = Path.Combine(documentsPath, "AndroKat");
                    _downloadedFilePath = Path.Combine(androkatFolder, fileName);
                }
            }

            if (!string.IsNullOrEmpty(_downloadedFilePath) && File.Exists(_downloadedFilePath))
            {
                bool confirm = await DisplayAlertAsync("Megerősítés", "Biztosan törli a letöltött fájlt?", "Igen", "Nem");
                if (confirm)
                {
                    File.Delete(_downloadedFilePath);
                    _downloadedFilePath = string.Empty;

                    await DisplayAlertAsync("Siker", "Fájl törölve", "OK");

                    // Update button states
                    DownloadButton.IsEnabled = true;
                    DownloadButton.Text = "LETÖLTÉS";

                    var deleteButton = sender as Button;
                    if (deleteButton != null)
                    {
                        deleteButton.IsEnabled = false;
                    }
                }
            }
            else
            {
                await DisplayAlertAsync("Hiba", "Nincs letöltött fájl", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hiba", $"Törlési hiba: {ex.Message}", "OK");
        }
    }

    private async void Lejatszas_OnClicked(object? sender, EventArgs e)
    {
        try
        {
            string? audioUrl = GetAudioUrl();
            if (string.IsNullOrEmpty(audioUrl))
            {
                await DisplayAlertAsync("Hiba", "Nincs elérhető hangfájl", "OK");
                return;
            }

            // Check if we have a local file first
            string fileName = GetFileName(audioUrl, ViewModel!.ContentView.type);
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string androkatFolder = Path.Combine(documentsPath, "AndroKat");
            string localFilePath = Path.Combine(androkatFolder, fileName);

            Uri playUri;
            if (File.Exists(localFilePath))
            {
                // Play local file
                playUri = new Uri($"file://{localFilePath}");
            }
            else
            {
                // Play from URL
                playUri = new Uri(audioUrl);
            }

            // Use Launcher to open with default audio player
            await Launcher.Default.OpenAsync(playUri);
        }
        catch (Exception ex)
        {
            // Fallback: try to open URL in browser/media player
            try
            {
                string? audioUrl = GetAudioUrl();
                if (!string.IsNullOrEmpty(audioUrl))
                {
                    await Launcher.Default.OpenAsync(audioUrl);
                }
            }
            catch
            {
                await DisplayAlertAsync("Hiba", $"Nem sikerült megnyitni a hangfájlt: {ex.Message}", "OK");
            }
        }
    }

    private string? GetAudioUrl()
    {
        var audioUrl = ViewModel?.ContentView?.ContentEntity?.KulsoLink;
        if (!string.IsNullOrEmpty(audioUrl))
        {
            return audioUrl;
        }

        return null;
    }

    private static string GetFileName(string url, Activities activityType)
    {
        try
        {
            var uri = new Uri(url);
            string originalFileName = Path.GetFileName(uri.LocalPath);

            if (string.IsNullOrEmpty(originalFileName))
            {
                originalFileName = "audio.mp3";
            }

            // Remove extension to add prefix
            string nameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
            string extension = Path.GetExtension(originalFileName);

            if (string.IsNullOrEmpty(extension))
            {
                extension = ".mp3";
            }

            // Add prefix based on activity type (same as Java implementation)
            string prefix = activityType switch
            {
                Activities.audiobarsi => "Barsi",
                Activities.prayasyougo => "NapiUtraValo",
                Activities.audiohorvath => "Horvath",
                Activities.audiotaize => "Taize",
                Activities.audionapievangelium => "Evangelium",
                Activities.audiopalferi => "PalFeri",
                _ => "Audio"
            };

            return $"{prefix}_{nameWithoutExt}{extension}";
        }
        catch
        {
            return $"audio_{DateTime.Now:yyyyMMdd_HHmmss}.mp3";
        }
    }

    private void UpdateToolbarItems()
    {
        if (ViewModel.ShowDeleteFavoriteButton)
        {
            // Create and add the delete toolbar item if it doesn't exist
            if (_deleteFavoriteToolbarItem == null)
            {
                _deleteFavoriteToolbarItem = new ToolbarItem
                {
                    IconImageSource = "delete",
                    Text = "Törlés kedvencekből"
                };
                _deleteFavoriteToolbarItem.SetBinding(ToolbarItem.CommandProperty, "DeleteFavoriteCommand");
            }

            if (!ToolbarItems.Contains(_deleteFavoriteToolbarItem))
            {
                ToolbarItems.Insert(0, _deleteFavoriteToolbarItem); // Insert before send
            }
        }
        else
        {
            // Remove the delete toolbar item if it exists
            if (_deleteFavoriteToolbarItem != null)
            {
                ToolbarItems.Remove(_deleteFavoriteToolbarItem);
            }
        }
    }
}