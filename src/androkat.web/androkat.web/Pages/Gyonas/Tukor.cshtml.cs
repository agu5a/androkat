using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System;
using System.Collections.Generic;

namespace androkat.web.Pages.Gyonas;

public static class Parancs
{
    public const string p11 = "Áldoztam-e halálos bûn állapotában?";
    public const string p12 = "Elmulasztottam-e imádkozni mindennap?";
    public const string p13 = "Hazudtam gyónásban, elhallgattam súlyos bûnt?";
    public const string p14 = "Hittem babonában, horoszkópban és egyéb divatos dolgokban?";
    public const string p15 = "Nem igyekszem szeretni Istent teljes szívembõl?";

    public const string p21 = "Esküdöztem fölöslegesen?";
    public const string p22 = "Káromkodtam? Isten és a szentek nevét ejtettem-e ki átkozódó, csúnya  szavak kíséretében?";
    public const string p23 = "Szoktam-e emlegetni Isten, szentek nevét fölöslegesen, tiszteletlenül?";
    public const string p24 = "Zúgolódtam-e Isten ellen? Voltam-e kishitû vagy vakmerõ?";
    public const string p25 = "Viccelõdtem szent dolgokról? Nem bánom, ha másoktól hallom?";

    public const string p31 = "Vasárnap, ünnepnap mulasztottam-e szentmisét? (Hanyagságból? Hányszor?)";
    public const string p32 = "Szentmisén Isten és az ott levõk iránt nem szeretettel és figyelemmel vettem részt?";
    public const string p33 = "Nem pihenésre, ünneplésre és a családdal való együttlétre fordítottam-e a vasárnapokat, ünnepeket?";
    public const string p34 = "Vasárnap, ünnepnap valós ok nélkül dolgoztam-e?";
    public const string p35 = "Nem tartom a kötelezõ és más böjtöket? Nem törekszem azt lelkivé tenni, vagy csak muszájból végzem?";

    public const string p41 = "Nem tisztelem szüleimet? Nem vagyok engedelmes? Nem próbálok örömet szerezni nekik?";
    public const string p42 = "Nem tisztelem az idõsebbeket, vezetõimet, tanáraimat stb.?";
    public const string p43 = "Szeretetet, kötelességet tagadtam-e meg házastársammal, gyerekeimmel szemben?";
    public const string p44 = "Jó keresztény módjára nem mutatok példát családomnak?";

    public const string p51 = "Szoktam-e verekedni, durváskodni, gúnyolódni? Másoknak fájdalmat okozni?";
    public const string p52 = "Nem vigyázok egészségemre? (Nem kerülöm a dohányzást, alkoholt és a drogot stb.?)";
    public const string p53 = "Csábítottam, bátorítottam-e mást bûnre?";
    public const string p54 = "Közlekedésben nem felelõsen veszek részt?";

    public const string p61 = "Ruházkodásomban nem vagyok körültekintõ másokra és az alkalomra?";
    public const string p62 = "Nem tisztelek másokat, testüket? Önzõ vágyaim céljára használom õket?";
    public const string p63 = "Nem kerülöm az alkalmakat és gondolatokat, ami bûnbeesésre buzdít másokkal és magammal szemben?";
    public const string p64 = "Beszédemben, viselkedésemben nem kerülöm a szemérmetlen dolgokat?";

    public const string p71 = "Vettem-e el olyat, ami nem az enyém? (Nem adtam vissza még?)";
    public const string p72 = "Nem vigyáztam magam és mások holmijára?";
    public const string p73 = "Megtartottam-e szándékosan azt amit kölcsön kaptam? Adósságaimat nem fizettem vissza?";
    public const string p74 = "Nem becsülettel használom idõmet tanulásban, munkában?";
    public const string p75 = "Nem keresem az alkalmat, hogy megosszam javaimat a rászorulókkal?";

    public const string p81 = "Hazudtam, hogy másról rosszat gondoljanak, vagy bajba kerüljön?";
    public const string p82 = "Hazudtam, hogy elkerüljek jogos büntetést, következményeket?";
    public const string p83 = "Nem kerülöm a pletykákat, nem védem meg mások becsületét?";
    public const string p84 = "Túl kritikus, negatív vagy könyörtelen vagyok másokkal szemben?";
    public const string p85 = "Tovább adom-e a rám bízott bizalmas titkokat?";

    public const string p91 = "Nem imádkozom, hogy le tudjam gyõzni a kísértéseket, bûnös gondolatokat?";
    public const string p92 = "Hagyom a fantáziálásom, gondolataim eluralkodjanak rajtam?";
    public const string p93 = "Nem tisztelem házastársamat, nem tudom õt elfogadni minden testi-lelki tulajdonságát? Összehasonlítgattom õt másokkal?";

    public const string p101 = "Önzõ vagyok? Irigykedtem-e (más értékeire, tehetségére stb.)?";
    public const string p102 = "Hálátlan vagyok Istennel szemben? Elégedetlen azzal szemben, amit kaptam tõle?";
    public const string p103 = "Nem bízok Isten gondviselésében? Aggódom testi-lelki szükségleteimért?";
    public const string p104 = "Isten helyett a világ dolgai, az anyagi javak életem fõ céljai?";
}

public class TukorModel : PageModel
{
    private readonly ILogger<TukorModel> _logger;

    public TukorModel(ILogger<TukorModel> logger)
    {
        _logger = logger;
    }

    #region CheckBoxes
    [BindProperty] public bool cb11 { get; set; }
    [BindProperty] public bool cb12 { get; set; }
    [BindProperty] public bool cb13 { get; set; }
    [BindProperty] public bool cb14 { get; set; }
    [BindProperty] public bool cb15 { get; set; }
    [BindProperty] public bool cb21 { get; set; }
    [BindProperty] public bool cb22 { get; set; }
    [BindProperty] public bool cb23 { get; set; }
    [BindProperty] public bool cb24 { get; set; }
    [BindProperty] public bool cb25 { get; set; }
    [BindProperty] public bool cb31 { get; set; }
    [BindProperty] public bool cb32 { get; set; }
    [BindProperty] public bool cb33 { get; set; }
    [BindProperty] public bool cb34 { get; set; }
    [BindProperty] public bool cb35 { get; set; }
    [BindProperty] public bool cb41 { get; set; }
    [BindProperty] public bool cb42 { get; set; }
    [BindProperty] public bool cb43 { get; set; }
    [BindProperty] public bool cb44 { get; set; }
    [BindProperty] public bool cb51 { get; set; }
    [BindProperty] public bool cb52 { get; set; }
    [BindProperty] public bool cb53 { get; set; }
    [BindProperty] public bool cb54 { get; set; }
    [BindProperty] public bool cb61 { get; set; }
    [BindProperty] public bool cb62 { get; set; }
    [BindProperty] public bool cb63 { get; set; }
    [BindProperty] public bool cb64 { get; set; }
    [BindProperty] public bool cb71 { get; set; }
    [BindProperty] public bool cb72 { get; set; }
    [BindProperty] public bool cb73 { get; set; }
    [BindProperty] public bool cb74 { get; set; }
    [BindProperty] public bool cb75 { get; set; }
    [BindProperty] public bool cb81 { get; set; }
    [BindProperty] public bool cb82 { get; set; }
    [BindProperty] public bool cb83 { get; set; }
    [BindProperty] public bool cb84 { get; set; }
    [BindProperty] public bool cb85 { get; set; }
    [BindProperty] public bool cb91 { get; set; }
    [BindProperty] public bool cb92 { get; set; }
    [BindProperty] public bool cb93 { get; set; }
    [BindProperty] public bool cb101 { get; set; }
    [BindProperty] public bool cb102 { get; set; }
    [BindProperty] public bool cb103 { get; set; }
    [BindProperty] public bool cb104 { get; set; }
    #endregion

    /*@for (int i = 0; i < 5; i++)
{
    <div>
        <input asp-for="@($"BindProperty{i}")" />
    </div>

    for (int i = 1; i <= 5; i++)
        {
            string dynamicBindProperty = $"DynamicBindProperty{i}";
            this.GetType().GetRuntimeProperty(dynamicBindProperty)?.SetValue(this, "default value");
        }
}*/



    public IActionResult OnPostClear()
    {
        HttpContext.Session.SetString("conf", "");
        return Redirect("/gyonas/tukor");
    }

    public void OnGet()
    {
        try
        {
            var conf = HttpContext.Session.GetString("conf");
            if (!string.IsNullOrWhiteSpace(conf))
            {
                var dic = JsonSerializer.Deserialize<Dictionary<string, bool>>(conf);
                cb11 = dic["cb11"];
                cb12 = dic["cb12"];
                cb13 = dic["cb13"];
                cb14 = dic["cb14"];
                cb15 = dic["cb15"];
                cb21 = dic["cb21"];
                cb22 = dic["cb22"];
                cb23 = dic["cb23"];
                cb24 = dic["cb24"];
                cb25 = dic["cb25"];
                cb31 = dic["cb31"];
                cb32 = dic["cb32"];
                cb33 = dic["cb33"];
                cb34 = dic["cb34"];
                cb35 = dic["cb35"];
                cb41 = dic["cb41"];
                cb42 = dic["cb42"];
                cb43 = dic["cb43"];
                cb44 = dic["cb44"];
                cb51 = dic["cb51"];
                cb52 = dic["cb52"];
                cb53 = dic["cb53"];
                cb54 = dic["cb54"];
                cb61 = dic["cb61"];
                cb62 = dic["cb62"];
                cb63 = dic["cb63"];
                cb64 = dic["cb64"];
                cb71 = dic["cb71"];
                cb72 = dic["cb72"];
                cb73 = dic["cb73"];
                cb74 = dic["cb74"];
                cb75 = dic["cb75"];
                cb81 = dic["cb81"];
                cb82 = dic["cb82"];
                cb83 = dic["cb83"];
                cb84 = dic["cb84"];
                cb85 = dic["cb85"];
                cb91 = dic["cb91"];
                cb92 = dic["cb92"];
                cb93 = dic["cb93"];

                cb101 = dic["cb101"];
                cb102 = dic["cb102"];
                cb103 = dic["cb103"];
                cb104 = dic["cb104"];
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }
    }

    public void OnPostSave()
    {
        try
        {
            Dictionary<string, bool> dic = new Dictionary<string, bool>
            {
                { "cb11", cb11 },
                { "cb12", cb12 },
                { "cb13", cb13 },
                { "cb14", cb14 },
                { "cb15", cb15 },

                { "cb21", cb21 },
                { "cb22", cb22 },
                { "cb23", cb23 },
                { "cb24", cb24 },
                { "cb25", cb25 },

                { "cb31", cb31 },
                { "cb32", cb32 },
                { "cb33", cb33 },
                { "cb34", cb34 },
                { "cb35", cb35 },

                { "cb41", cb41 },
                { "cb42", cb42 },
                { "cb43", cb43 },
                { "cb44", cb44 },

                { "cb51", cb51 },
                { "cb52", cb52 },
                { "cb53", cb53 },
                { "cb54", cb54 },

                { "cb61", cb61 },
                { "cb62", cb62 },
                { "cb63", cb63 },
                { "cb64", cb64 },

                { "cb71", cb71 },
                { "cb72", cb72 },
                { "cb73", cb73 },
                { "cb74", cb74 },
                { "cb75", cb75 },

                { "cb81", cb81 },
                { "cb82", cb82 },
                { "cb83", cb83 },
                { "cb84", cb84 },
                { "cb85", cb85 },

                { "cb91", cb91 },
                { "cb92", cb92 },
                { "cb93", cb93 },

                { "cb101", cb101 },
                { "cb102", cb102 },
                { "cb103", cb103 },
                { "cb104", cb104 }
            };

            HttpContext.Session.SetString("conf", JsonSerializer.Serialize(dic));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }
    }
}