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
    public const string p11 = "�ldoztam-e hal�los b�n �llapot�ban?";
    public const string p12 = "Elmulasztottam-e im�dkozni mindennap?";
    public const string p13 = "Hazudtam gy�n�sban, elhallgattam s�lyos b�nt?";
    public const string p14 = "Hittem babon�ban, horoszk�pban �s egy�b divatos dolgokban?";
    public const string p15 = "Nem igyekszem szeretni Istent teljes sz�vemb�l?";

    public const string p21 = "Esk�d�ztem f�l�slegesen?";
    public const string p22 = "K�romkodtam? Isten �s a szentek nev�t ejtettem-e ki �tkoz�d�, cs�nya  szavak k�s�ret�ben?";
    public const string p23 = "Szoktam-e emlegetni Isten, szentek nev�t f�l�slegesen, tiszteletlen�l?";
    public const string p24 = "Z�gol�dtam-e Isten ellen? Voltam-e kishit� vagy vakmer�?";
    public const string p25 = "Viccel�dtem szent dolgokr�l? Nem b�nom, ha m�sokt�l hallom?";

    public const string p31 = "Vas�rnap, �nnepnap mulasztottam-e szentmis�t? (Hanyags�gb�l? H�nyszor?)";
    public const string p32 = "Szentmis�n Isten �s az ott lev�k ir�nt nem szeretettel �s figyelemmel vettem r�szt?";
    public const string p33 = "Nem pihen�sre, �nnepl�sre �s a csal�ddal val� egy�ttl�tre ford�tottam-e a vas�rnapokat, �nnepeket?";
    public const string p34 = "Vas�rnap, �nnepnap val�s ok n�lk�l dolgoztam-e?";
    public const string p35 = "Nem tartom a k�telez� �s m�s b�jt�ket? Nem t�rekszem azt lelkiv� tenni, vagy csak musz�jb�l v�gzem?";

    public const string p41 = "Nem tisztelem sz�leimet? Nem vagyok engedelmes? Nem pr�b�lok �r�met szerezni nekik?";
    public const string p42 = "Nem tisztelem az id�sebbeket, vezet�imet, tan�raimat stb.?";
    public const string p43 = "Szeretetet, k�teless�get tagadtam-e meg h�zast�rsammal, gyerekeimmel szemben?";
    public const string p44 = "J� kereszt�ny m�dj�ra nem mutatok p�ld�t csal�domnak?";

    public const string p51 = "Szoktam-e verekedni, durv�skodni, g�nyol�dni? M�soknak f�jdalmat okozni?";
    public const string p52 = "Nem vigy�zok eg�szs�gemre? (Nem ker�l�m a doh�nyz�st, alkoholt �s a drogot stb.?)";
    public const string p53 = "Cs�b�tottam, b�tor�tottam-e m�st b�nre?";
    public const string p54 = "K�zleked�sben nem felel�sen veszek r�szt?";

    public const string p61 = "Ruh�zkod�somban nem vagyok k�r�ltekint� m�sokra �s az alkalomra?";
    public const string p62 = "Nem tisztelek m�sokat, test�ket? �nz� v�gyaim c�lj�ra haszn�lom �ket?";
    public const string p63 = "Nem ker�l�m az alkalmakat �s gondolatokat, ami b�nbees�sre buzd�t m�sokkal �s magammal szemben?";
    public const string p64 = "Besz�demben, viselked�semben nem ker�l�m a szem�rmetlen dolgokat?";

    public const string p71 = "Vettem-e el olyat, ami nem az eny�m? (Nem adtam vissza m�g?)";
    public const string p72 = "Nem vigy�ztam magam �s m�sok holmij�ra?";
    public const string p73 = "Megtartottam-e sz�nd�kosan azt amit k�lcs�n kaptam? Ad�ss�gaimat nem fizettem vissza?";
    public const string p74 = "Nem becs�lettel haszn�lom id�met tanul�sban, munk�ban?";
    public const string p75 = "Nem keresem az alkalmat, hogy megosszam javaimat a r�szorul�kkal?";

    public const string p81 = "Hazudtam, hogy m�sr�l rosszat gondoljanak, vagy bajba ker�lj�n?";
    public const string p82 = "Hazudtam, hogy elker�ljek jogos b�ntet�st, k�vetkezm�nyeket?";
    public const string p83 = "Nem ker�l�m a pletyk�kat, nem v�dem meg m�sok becs�let�t?";
    public const string p84 = "T�l kritikus, negat�v vagy k�ny�rtelen vagyok m�sokkal szemben?";
    public const string p85 = "Tov�bb adom-e a r�m b�zott bizalmas titkokat?";

    public const string p91 = "Nem im�dkozom, hogy le tudjam gy�zni a k�s�rt�seket, b�n�s gondolatokat?";
    public const string p92 = "Hagyom a fant�zi�l�som, gondolataim eluralkodjanak rajtam?";
    public const string p93 = "Nem tisztelem h�zast�rsamat, nem tudom �t elfogadni minden testi-lelki tulajdons�g�t? �sszehasonl�tgattom �t m�sokkal?";

    public const string p101 = "�nz� vagyok? Irigykedtem-e (m�s �rt�keire, tehets�g�re stb.)?";
    public const string p102 = "H�l�tlan vagyok Istennel szemben? El�gedetlen azzal szemben, amit kaptam t�le?";
    public const string p103 = "Nem b�zok Isten gondvisel�s�ben? Agg�dom testi-lelki sz�ks�gleteim�rt?";
    public const string p104 = "Isten helyett a vil�g dolgai, az anyagi javak �letem f� c�ljai?";
}

public class TukorModel : PageModel
{
    private readonly ILogger<TukorModel> _logger;

    public TukorModel(ILogger<TukorModel> logger)
    {
        _logger = logger;
    }

    #region CheckBoxes
#pragma warning disable IDE1006 // Naming Styles
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
#pragma warning restore IDE1006 // Naming Styles
    #endregion


#pragma warning disable S125 // Sections of code should not be commented out
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
#pragma warning restore S125 // Sections of code should not be commented out


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
            if (string.IsNullOrWhiteSpace(conf))
            {
                return;
            }
            
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }
    }

    public void OnPostSave()
    {
        try
        {
            Dictionary<string, bool> dic = new()
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