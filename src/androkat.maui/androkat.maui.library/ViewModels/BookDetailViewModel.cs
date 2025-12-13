using androkat.maui.library.Abstraction;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;

namespace androkat.maui.library.ViewModels;

public partial class BookDetailViewModel : DetailViewModel, IDisposable
{
    [ObservableProperty]
    private bool isDownloading = false;

    [ObservableProperty]
    private double downloadProgress = 0.0;

    [ObservableProperty]
    private string downloadButtonText = "Letöltés";

    [ObservableProperty]
    private bool canDownload = true;

    [ObservableProperty]
    private bool canDelete = false;

    [ObservableProperty]
    private bool canRead = false;

    [ObservableProperty]
    private bool showWebView = false;

    [ObservableProperty]
    private string bookDescription = "";

    private string _bookUrl = "";
    private string _localFilePath = "";
    private HttpClient _httpClient;

    public BookDetailViewModel(IPageService pageService, ISourceData sourceData)
        : base(pageService, sourceData)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AndroKat", "03.58"));
    }

    public async Task InitializeBookAsync()
    {
        await InitializeAsync();

        if (ContentView?.ContentEntity != null)
        {
            // For books, the URL is stored in KulsoLink or Idezet
            _bookUrl = ContentView.ContentEntity.KulsoLink ?? ContentView.ContentEntity.Idezet;

            // Set up book-specific UI elements
            SetupBookDescription();
            CheckFileStatus();
        }
    }

    private void SetupBookDescription()
    {
        BookDescription = "Az olvasáshoz ePUB olvasó alkalmazás szükséges. A Google Play áruházból tudsz beszerezni ilyet: " +
                         "<a href=\"https://play.google.com/store/search?q=epub+reader&c=apps\">Play Store</a>.<br>" +
                         "Mi ezt ajánljuk: <a href=\"https://play.google.com/store/apps/details?id=ebook.epub.download.reader\">EBook</a> " +
                         "vagy ezt: <a href=\"https://play.google.com/store/apps/details?id=com.flyersoft.moonreader\">Moon+Reader</a>";

        // Show WebView only if there's no valid book URL
        ShowWebView = string.IsNullOrEmpty(_bookUrl) || !Uri.IsWellFormedUriString(_bookUrl, UriKind.Absolute);

        if (ShowWebView)
        {
            BookDescription = "Szólj nekünk, ha problémát találsz!";
        }
    }

    private void CheckFileStatus()
    {
        if (string.IsNullOrEmpty(_bookUrl) || !Uri.IsWellFormedUriString(_bookUrl, UriKind.Absolute))
        {
            CanDownload = false;
            CanDelete = false;
            CanRead = false;
            return;
        }

        try
        {
            var uri = new Uri(_bookUrl);
            var fileName = Path.GetFileName(uri.LocalPath);
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "book.epub";
            }

            _localFilePath = FileAccessHelper.GetLocalFilePath($"Book_{fileName}");

            bool fileExists = File.Exists(_localFilePath);

            CanDownload = !fileExists;
            CanDelete = fileExists;
            CanRead = fileExists;

            DownloadButtonText = fileExists ? "Már letöltve" : "Letöltés";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error checking file status: {ex.Message}");
            CanDownload = false;
            CanDelete = false;
            CanRead = false;
        }
    }

    private static Page GetCurrentPage()
    {
        return Application.Current?.Windows?.FirstOrDefault()?.Page ?? Shell.Current;
    }

    [RelayCommand]
    private async Task DownloadBook()
    {
        if (string.IsNullOrEmpty(_bookUrl) || !CanDownload)
            return;

        try
        {
            IsDownloading = true;
            DownloadProgress = 0.0;
            CanDownload = false;

            var progress = new Progress<double>(value =>
            {
                DownloadProgress = value;
            });

            await DownloadFileAsync(_bookUrl, _localFilePath, progress);

            IsDownloading = false;
            CheckFileStatus();

            await GetCurrentPage().DisplayAlertAsync(
                "Sikeres letöltés!",
                $"A könyv sikeresen letöltve: {Path.GetFileName(_localFilePath)}",
                "OK");
        }
        catch (Exception ex)
        {
            IsDownloading = false;
            DownloadProgress = 0.0;
            CheckFileStatus();

            await GetCurrentPage().DisplayAlertAsync(
                "Hiba!",
                $"A letöltés nem sikerült: {ex.Message}",
                "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteBook()
    {
        if (string.IsNullOrEmpty(_localFilePath) || !CanDelete)
            return;

        try
        {
            var result = await GetCurrentPage().DisplayAlertAsync(
                "Törlés megerősítése",
                "Biztosan törölni szeretnéd a letöltött könyvet?",
                "Igen", "Nem");

            if (!result)
                return;

            if (File.Exists(_localFilePath))
            {
                File.Delete(_localFilePath);
                CheckFileStatus();

                await GetCurrentPage().DisplayAlertAsync(
                    "Sikeres törlés!",
                    "A könyv sikeresen törölve.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await GetCurrentPage().DisplayAlertAsync(
                "Hiba!",
                $"A törlés nem sikerült: {ex.Message}",
                "OK");
        }
    }

    [RelayCommand]
    private async Task ReadBook()
    {
        if (string.IsNullOrEmpty(_localFilePath) || !CanRead)
        {
            await GetCurrentPage().DisplayAlertAsync(
                "Hiba!",
                "Nincs letöltve még a könyv.",
                "OK");
            return;
        }

        try
        {
            if (!File.Exists(_localFilePath))
            {
                await GetCurrentPage().DisplayAlertAsync(
                    "Hiba!",
                    "A letöltött fájl nem található.",
                    "OK");
                return;
            }

            // Try to open the file with the default application
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(_localFilePath),
                Title = "ePUB olvasó szükséges az olvasáshoz"
            });
        }
        catch (Exception ex)
        {
            await GetCurrentPage().DisplayAlertAsync(
                "Hiba!",
                $"Nem sikerült megnyitni a könyvet: {ex.Message}\n\nKérlek telepíts egy ePUB olvasó alkalmazást!",
                "OK");
        }
    }

    private async Task DownloadFileAsync(string url, string filePath, IProgress<double> progress)
    {
        using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var totalBytes = response.Content.Headers.ContentLength ?? -1L;
        var buffer = new byte[8192];
        var bytesRead = 0L;

        using var contentStream = await response.Content.ReadAsStreamAsync();
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

        int read;
        while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await fileStream.WriteAsync(buffer, 0, read);
            bytesRead += read;

            if (totalBytes > 0)
            {
                var progressValue = (double)bytesRead / totalBytes;
                progress?.Report(progressValue);
            }
        }
    }

    private bool _disposed = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _httpClient?.Dispose();
            }
            _disposed = true;
        }
    }
}
