using androkat.maui.library.Models;

namespace androkat.maui.library.Models;

public class FilterOption
{
    public string Key { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public Activities Activity { get; set; }
}

public static class FilterOptionsHelper
{
    public static List<FilterOption> GetFilterOptionsForPageId(string pageId)
    {
        var options = pageId switch
        {
            "0" => GetNapiOlvasoFilters(), // Evangélium, elmélkedés
            "3" => GetSzentekFilters(), // Szentek idézetei
            "4" => GetNewsFilters(), // Katolikus Hírek
            "5" => GetMagazinFilters(), // Blog, magazin
            "8" => GetAudioFilters(), // Hanganyagok
            _ => new List<FilterOption>()
        };

        System.Diagnostics.Debug.WriteLine($"FilterOptionsHelper.GetFilterOptionsForPageId({pageId}) returning {options.Count} options:");
        foreach (var option in options)
        {
            System.Diagnostics.Debug.WriteLine($"  - Key: '{option.Key}', DisplayName: '{option.DisplayName}'");
        }

        return options;
    }

    private static List<FilterOption> GetNapiOlvasoFilters()
    {
        return new List<FilterOption>
        {
            // Use integer values for Tipus field (not TypeName)
            new() { Key = ((int)Activities.barsi).ToString(), DisplayName = "Barsi és Telek - Magasság és mélység", Activity = Activities.barsi },
            new() { Key = ((int)Activities.horvath).ToString(), DisplayName = "Horváth István Sándor atya", Activity = Activities.horvath },
            new() { Key = ((int)Activities.fokolare).ToString(), DisplayName = "fokolare.hu", Activity = Activities.fokolare },
            new() { Key = ((int)Activities.maievangelium).ToString(), DisplayName = "Napi Ige és olvasmányok", Activity = Activities.maievangelium },
            new() { Key = ((int)Activities.papaitwitter).ToString(), DisplayName = "Ferenc pápa twitter üzenete", Activity = Activities.papaitwitter },
            new() { Key = ((int)Activities.advent).ToString(), DisplayName = "Adventi elmélkedések", Activity = Activities.advent },
            new() { Key = ((int)Activities.nagybojt).ToString(), DisplayName = "Nagyböjti elmélkedések", Activity = Activities.nagybojt },
            new() { Key = ((int)Activities.bojte).ToString(), DisplayName = "Böjte Csaba gondolatai", Activity = Activities.bojte },
            new() { Key = ((int)Activities.regnum).ToString(), DisplayName = "Regnum Christi Mozgalom", Activity = Activities.regnum },
            new() { Key = ((int)Activities.prohaszka).ToString(), DisplayName = "Prohászka Ottokár breviáriuma", Activity = Activities.prohaszka },
            new() { Key = ((int)Activities.szeretetujsag).ToString(), DisplayName = "Szeretet-újság", Activity = Activities.szeretetujsag },
            new() { Key = ((int)Activities.kempis).ToString(), DisplayName = "Kempis Tamás", Activity = Activities.kempis },
            new() { Key = ((int)Activities.taize).ToString(), DisplayName = "Taizé", Activity = Activities.taize },
            new() { Key = ((int)Activities.laciatya).ToString(), DisplayName = "Laci atya", Activity = Activities.laciatya },
            new() { Key = ((int)Activities.medjugorje).ToString(), DisplayName = "Medjugorje", Activity = Activities.medjugorje },
            new() { Key = ((int)Activities.mello).ToString(), DisplayName = "Anthony de Mello", Activity = Activities.mello }
        };
    }

    private static List<FilterOption> GetSzentekFilters()
    {
        return new List<FilterOption>
        {
            new() { Key = ((int)Activities.pio).ToString(), DisplayName = "Pio atya", Activity = Activities.pio },
            new() { Key = ((int)Activities.janospal).ToString(), DisplayName = "II. János Pál pápa", Activity = Activities.janospal },
            new() { Key = ((int)Activities.sztjanos).ToString(), DisplayName = "Keresztes Szent János", Activity = Activities.sztjanos },
            new() { Key = ((int)Activities.kisterez).ToString(), DisplayName = "Lisieux-i Szent Teréz", Activity = Activities.kisterez },
            new() { Key = ((int)Activities.terezanya).ToString(), DisplayName = "Kalkuttai Teréz anya", Activity = Activities.terezanya },
            new() { Key = ((int)Activities.ignac).ToString(), DisplayName = "Loyolai Szent Ignác", Activity = Activities.ignac },
            new() { Key = ((int)Activities.vianney).ToString(), DisplayName = "Vianney Szent János", Activity = Activities.vianney },
            new() { Key = ((int)Activities.szentbernat).ToString(), DisplayName = "Clairvaux-i Szent Bernát", Activity = Activities.szentbernat },
            new() { Key = ((int)Activities.szentszalezi).ToString(), DisplayName = "Szalézi Szent Ferenc", Activity = Activities.szentszalezi },
            new() { Key = ((int)Activities.sienaikatalin).ToString(), DisplayName = "Sienai Szent Katalin", Activity = Activities.sienaikatalin }
        };
    }

    private static List<FilterOption> GetNewsFilters()
    {
        return new List<FilterOption>
        {
            new() { Key = ((int)Activities.kurir).ToString(), DisplayName = "Magyar Kurír", Activity = Activities.kurir },
            new() { Key = ((int)Activities.bonumtv).ToString(), DisplayName = "Bonum TV", Activity = Activities.bonumtv },
            new() { Key = ((int)Activities.keresztenyelet).ToString(), DisplayName = "Keresztény Élet", Activity = Activities.keresztenyelet }
        };
    }

    private static List<FilterOption> GetAudioFilters()
    {
        return new List<FilterOption>
        {
            new() { Key = ((int)Activities.prayasyougo).ToString(), DisplayName = "Pray-as-you-go", Activity = Activities.prayasyougo },
            new() { Key = ((int)Activities.audiopalferi).ToString(), DisplayName = "Pálfy Pál Ferenc", Activity = Activities.audiopalferi },
            new() { Key = ((int)Activities.audiobarsi).ToString(), DisplayName = "Barsi Balázs", Activity = Activities.audiobarsi },
            new() { Key = ((int)Activities.audiohorvath).ToString(), DisplayName = "Horváth István Sándor atya", Activity = Activities.audiohorvath },
            new() { Key = ((int)Activities.audiotaize).ToString(), DisplayName = "Taizé", Activity = Activities.audiotaize },
            new() { Key = ((int)Activities.audionapievangelium).ToString(), DisplayName = "Napi evangélium", Activity = Activities.audionapievangelium }
        };
    }

    private static List<FilterOption> GetMagazinFilters()
    {
        return new List<FilterOption>
        {
            new() { Key = ((int)Activities.b777).ToString(), DisplayName = "777 blog", Activity = Activities.b777 },
            new() { Key = ((int)Activities.bzarandokma).ToString(), DisplayName = "Zarándok Magazin", Activity = Activities.bzarandokma },
            new() { Key = ((int)Activities.jezsuitablog).ToString(), DisplayName = "Jezsuita blog", Activity = Activities.jezsuitablog }
        };
    }
}
