using androkat.maui.library.Models;
using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class DetailAudio
{
    private DetailViewModel ViewModel => (BindingContext as DetailViewModel)!;

    public DetailAudio(DetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
        Title.Text = "forrasTitle"; //Objects.requireNonNull(idezetSource).get_title()
        Leiras1.Text = ViewModel.Title;
        Leiras2.Text = ViewModel.ContentView.datum;

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
                "<a href=\"https://www.taize.fr/hu_article29660.html\">www.taize.fr/hu_article29660.html</a>",
            Activities.audionapievangelium =>
                "A <a href=\"https://www.katolikusradio.hu/evangelium\">katolikusradio.hu</a> által közétett napi evangélium hanganyagként.",
            _ => ""
        };
        
        Leiras3.Text = leiras + "<br><br><b>Töltse le</b> vagy <b>hallgassa meg most</b>";
    }

    protected override void OnDisappearing()
    {
        ViewModel.CancelSpeech();
        base.OnDisappearing();
    }

    private void Letoltes_OnClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
    
    private void Torles_OnClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
    
    private void Lejatszas_OnClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}