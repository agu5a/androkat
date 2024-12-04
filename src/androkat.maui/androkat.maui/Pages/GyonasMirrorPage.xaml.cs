using androkat.maui.library.Models.Entities;
using androkat.maui.library.ViewModels;

namespace androkat.hu.Pages;

public partial class GyonasMirrorPage : ContentPage
{
    private GyonasMirrorViewModel ViewModel => (BindingContext as GyonasMirrorViewModel)!;
    private const string p11 = "Áldoztam-e halálos bűn állapotában?";
    private const string p12 = "Elmulasztottam-e imádkozni mindennap?";
    private const string p13 = "Hazudtam gyónásban, elhallgattam súlyos bűnt?";
    private const string p14 = "Hittem babonában, horoszkópban és egyéb divatos dolgokban?";
    private const string p15 = "Nem igyekszem szeretni Istent teljes szívemből?";

    private const string p21 = "Esküdöztem fölöslegesen?";
    private const string p22 = "Káromkodtam? Isten és a szentek nevét ejtettem-e ki átkozódó, csúnya  szavak kíséretében?";
    private const string p23 = "Szoktam-e emlegetni Isten, szentek nevét fölöslegesen, tiszteletlenül?";
    private const string p24 = "Zúgolódtam-e Isten ellen? Voltam-e kishitű vagy vakmerő?";
    private const string p25 = "Viccelődtem szent dolgokról? Nem bánom, ha másoktól hallom?";

    private const string p31 = "Vasárnap, ünnepnap mulasztottam-e szentmisét? (Hanyagságból? Hányszor?)";
    private const string p32 = "Szentmisén Isten és az ott levők iránt nem szeretettel és figyelemmel vettem részt?";
    private const string p33 = "Nem pihenésre, ünneplésre és a családdal való együttlétre fordítottam-e a vasárnapokat, ünnepeket?";
    private const string p34 = "Vasárnap, ünnepnap valós ok nélkül dolgoztam-e?";
    private const string p35 = "Nem tartom a kötelező és más böjtöket? Nem törekszem azt lelkivé tenni, vagy csak muszájból végzem?";

    private const string p41 = "Nem tisztelem szüleimet? Nem vagyok engedelmes? Nem próbálok örömet szerezni nekik?";
    private const string p42 = "Nem tisztelem az idősebbeket, vezetőimet, tanáraimat stb.?";
    private const string p43 = "Szeretetet, kötelességet tagadtam-e meg házastársammal, gyerekeimmel szemben?";
    private const string p44 = "Jó keresztény módjára nem mutatok példát családomnak?";

    private const string p51 = "Szoktam-e verekedni, durváskodni, gúnyolódni? Másoknak fájdalmat okozni?";
    private const string p52 = "Nem vigyázok egészségemre? (Nem kerülöm a dohányzást, alkoholt és a drogot stb.?)";
    private const string p53 = "Csábítottam, bátorítottam-e mást bűnre?";
    private const string p54 = "Közlekedésben nem felelősen veszek részt?";

    private const string p61 = "Ruházkodásomban nem vagyok körültekintő másokra és az alkalomra?";
    private const string p62 = "Nem tisztelek másokat, testüket? Önző vágyaim céljára használom őket?";
    private const string p63 = "Nem kerülöm az alkalmakat és gondolatokat, ami bűnbeesésre buzdít másokkal és magammal szemben?";
    private const string p64 = "Beszédemben, viselkedésemben nem kerülöm a szemérmetlen dolgokat?";

    private const string p71 = "Vettem-e el olyat, ami nem az enyém? (Nem adtam vissza még?)";
    private const string p72 = "Nem vigyáztam magam és mások holmijára?";
    private const string p73 = "Megtartottam-e szándékosan azt amit kölcsön kaptam? Adósságaimat nem fizettem vissza?";
    private const string p74 = "Nem becsülettel használom időmet tanulásban, munkában?";
    private const string p75 = "Nem keresem az alkalmat, hogy megosszam javaimat a rászorulókkal?";

    private const string p81 = "Hazudtam, hogy másról rosszat gondoljanak, vagy bajba kerüljön?";
    private const string p82 = "Hazudtam, hogy elkerüljek jogos büntetést, következményeket?";
    private const string p83 = "Nem kerülöm a pletykákat, nem védem meg mások becsületét?";
    private const string p84 = "Túl kritikus, negatív vagy könyörtelen vagyok másokkal szemben?";
    private const string p85 = "Tovább adom-e a rám bízott bizalmas titkokat?";

    private const string p91 = "Nem imádkozom, hogy le tudjam győzni a kísértéseket, bűnös gondolatokat?";
    private const string p92 = "Hagyom a fantáziálásom, gondolataim eluralkodjanak rajtam?";
    private const string p93 = "Nem tisztelem házastársamat, nem tudom őt elfogadni minden testi-lelki tulajdonságát? Összehasonlítgattom őt másokkal?";

    private const string p101 = "Önző vagyok? Irigykedtem-e (más értékeire, tehetségére stb.)?";
    private const string p102 = "Hálátlan vagyok Istennel szemben? Elégedetlen azzal szemben, amit kaptam tőle?";
    private const string p103 = "Nem bízok Isten gondviselésében? Aggódom testi-lelki szükségleteimért?";
    private const string p104 = "Isten helyett a világ dolgai, az anyagi javak életem fő céljai?";

    public GyonasMirrorPage(GyonasMirrorViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
        AddHorizontalStackLayouts();
    }

    private void AddHorizontalStackLayouts()
    {
        VP1.Children.Add(CreateHorizontalStackLayout("1|1", p11));
        VP1.Children.Add(CreateHorizontalStackLayout("1|2", p12));
        VP1.Children.Add(CreateHorizontalStackLayout("1|3", p13));
        VP1.Children.Add(CreateHorizontalStackLayout("1|4", p14));
        VP1.Children.Add(CreateHorizontalStackLayout("1|5", p15));

        VP2.Children.Add(CreateHorizontalStackLayout("2|1", p21));
        VP2.Children.Add(CreateHorizontalStackLayout("2|2", p22));
        VP2.Children.Add(CreateHorizontalStackLayout("2|3", p23));
        VP2.Children.Add(CreateHorizontalStackLayout("2|4", p24));
        VP2.Children.Add(CreateHorizontalStackLayout("2|5", p25));

        VP3.Children.Add(CreateHorizontalStackLayout("3|1", p31));
        VP3.Children.Add(CreateHorizontalStackLayout("3|2", p32));
        VP3.Children.Add(CreateHorizontalStackLayout("3|3", p33));
        VP3.Children.Add(CreateHorizontalStackLayout("3|4", p34));
        VP3.Children.Add(CreateHorizontalStackLayout("3|5", p35));

        VP4.Children.Add(CreateHorizontalStackLayout("4|1", p41));
        VP4.Children.Add(CreateHorizontalStackLayout("4|2", p42));
        VP4.Children.Add(CreateHorizontalStackLayout("4|3", p43));
        VP4.Children.Add(CreateHorizontalStackLayout("4|4", p44));

        VP5.Children.Add(CreateHorizontalStackLayout("5|1", p51));
        VP5.Children.Add(CreateHorizontalStackLayout("5|2", p52));
        VP5.Children.Add(CreateHorizontalStackLayout("5|3", p53));
        VP5.Children.Add(CreateHorizontalStackLayout("5|4", p54));

        VP6.Children.Add(CreateHorizontalStackLayout("6|1", p61));
        VP6.Children.Add(CreateHorizontalStackLayout("6|2", p62));
        VP6.Children.Add(CreateHorizontalStackLayout("6|3", p63));
        VP6.Children.Add(CreateHorizontalStackLayout("6|4", p64));

        VP7.Children.Add(CreateHorizontalStackLayout("7|1", p71));
        VP7.Children.Add(CreateHorizontalStackLayout("7|2", p72));
        VP7.Children.Add(CreateHorizontalStackLayout("7|3", p73));
        VP7.Children.Add(CreateHorizontalStackLayout("7|4", p74));
        VP7.Children.Add(CreateHorizontalStackLayout("7|5", p75));

        VP8.Children.Add(CreateHorizontalStackLayout("8|1", p81));
        VP8.Children.Add(CreateHorizontalStackLayout("8|2", p82));
        VP8.Children.Add(CreateHorizontalStackLayout("8|3", p83));
        VP8.Children.Add(CreateHorizontalStackLayout("8|4", p84));
        VP8.Children.Add(CreateHorizontalStackLayout("8|5", p85));

        VP9.Children.Add(CreateHorizontalStackLayout("9|1", p91));
        VP9.Children.Add(CreateHorizontalStackLayout("9|2", p92));
        VP9.Children.Add(CreateHorizontalStackLayout("9|3", p93));

        VP10.Children.Add(CreateHorizontalStackLayout("10|1", p101));
        VP10.Children.Add(CreateHorizontalStackLayout("10|2", p102));
        VP10.Children.Add(CreateHorizontalStackLayout("10|3", p103));
        VP10.Children.Add(CreateHorizontalStackLayout("10|4", p104));
    }


    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "<Pending>")]
    private async void CheckBox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox)
        {
            var id = checkBox.BindingContext?.ToString();

            bool isChecked = e.Value;
            if (isChecked)
            {
                await ViewModel.InsertBunok(new Bunok
                {
                    BunId = int.Parse(id!.Split('|')[0]),
                    ParancsId = int.Parse(id.Split('|')[1])
                });
            }
            else
            {
                await ViewModel.DeleteBunokByIds(int.Parse(id!.Split('|')[0]), int.Parse(id.Split('|')[1]));
            }
        }
    }

    private HorizontalStackLayout CreateHorizontalStackLayout(string context, string labelText)
    {
        var hsl1 = new HorizontalStackLayout
        {
            Padding = new Thickness(5)
        };

        var bunAndParancsId = context.Split('|');
        var bun = ViewModel.BunokList.FirstOrDefault(b => b.BunId == int.Parse(bunAndParancsId[0]) && b.ParancsId == int.Parse(bunAndParancsId[1]));

        var checkBox1 = new CheckBox
        {
            BindingContext = context,
            IsChecked = bun != null
        };
        checkBox1.CheckedChanged += CheckBox_CheckedChanged;

        var label1 = new Label
        {
            Text = labelText
        };

        hsl1.Children.Add(checkBox1);
        hsl1.Children.Add(label1);

        return hsl1;
    }
}