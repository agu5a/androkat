namespace androkat.maui.library.Models;

public class ConsValues
{
    public static string oldalSorrend = "oldalSorrend9"; //ha van új menüpont a menüben, akkor emelni kell a verziót!!
    public static int defMaxOffline = 200;

    public static string[] rssResourceName = new[]
            {
                    Activities.barsi.ToString(),
                    Activities.horvath.ToString(),
                    Activities.fokolare.ToString(),
                    Activities.maievangelium.ToString(),
                    Activities.papaitwitter.ToString(),
                    Activities.advent.ToString(),
                    Activities.nagybojt.ToString(),
                    Activities.bojte.ToString(),
                    Activities.regnum.ToString(),
                    Activities.prohaszka.ToString(),
                    Activities.szeretetujsag.ToString(),
                    Activities.kempis.ToString(),
                    Activities.taize.ToString(),
                    Activities.laciatya.ToString()
            };

    public static string[] szentekResourceName = new[]
            {
                    Activities.pio.ToString(),
                    Activities.janospal.ToString(),
                    Activities.sztjanos.ToString(),
                    Activities.kisterez.ToString(),
                    Activities.terezanya.ToString(),
                    Activities.ignac.ToString(),
                    Activities.vianney.ToString(),
                    Activities.szentbernat.ToString(),
                    Activities.szentszalezi.ToString(),
                    Activities.sienaikatalin.ToString()
            };

    public static string[] rssResourceNewsName = new[]
            {
                    Activities.kurir.ToString(),
                    Activities.bonumtv.ToString(),
                    Activities.keresztenyelet.ToString()
            };

    public static string[] rssResourceAudioName = new[]
            {
                    Activities.prayasyougo.ToString(),
                    Activities.audiopalferi.ToString(),
                    Activities.audiobarsi.ToString(),
                    Activities.audiohorvath.ToString(),
                    Activities.audiotaize.ToString(),
                    Activities.audionapievangelium.ToString()
            };

    public static string[] rssResourceMagazinName = new[]
            {
                    Activities.b777.ToString(),
                    Activities.bkatolikusma.ToString(),
                    Activities.jezsuitablog.ToString()
            };
}
