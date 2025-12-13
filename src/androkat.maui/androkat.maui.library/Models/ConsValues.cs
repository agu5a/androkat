namespace androkat.maui.library.Models;

public static class ConsValues
{
        public const string VERZIO = "version2";
        public const string oldalSorrend = "oldalSorrend9"; //ha van új menüpont a menüben, akkor emelni kell a verziót!!
        public const int defMaxOffline = 200;
#pragma warning disable S1075 // URIs should not be hardcoded
        public const string ApiUrl = "http://api.androkat.hu/";
        public const string Szentistvanradio = "http://online.szentistvanradio.hu:7000/adas";
        public const string Katolikusradio = "http://katolikusradio.hu:9000/live_hi.mp3";
        public const string Vaticannews = "https://media.vaticannews.va/media2/audio/program/1900/ungherese_300825.mp3";
        public const string EzAzANap = "https://www.radioking.com/play/ez-az-a-nap-radio";
        public const string Mariaradio = "http://www.mariaradio.hu:8000/mr";
        public const string Katekizmus = "https://archiv.katolikus.hu/kek/";
        public const string EBiblia = "https://szentiras.hu/";
        public const string Zsolozsma = "https://breviar.sk/cgi-bin/l.cgi?qt=pdnes&j=hu";
        public const string MiseRend = "https://miserend.hu/";
        public const string MegszenteltTer = "https://sacredspace.com/hu/";
        public const string BonumTv = "https://katolikus.tv/elo-adas/";
        public const string LiturgiaTv = "https://liturgia.tv/";
        public const string AndrokatMarket = "https://play.google.com/store/apps/details?id=hu.AndroKat"; //market://search?q=pname:hu.AndroKat
#pragma warning restore S1075 // URIs should not be hardcoded

        public static readonly string[] rssResourceName =
                [

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
                Activities.laciatya.ToString(),
                Activities.medjugorje.ToString(),
                Activities.mello.ToString(),
                Activities.martonaron.ToString()
                ];

        public static readonly string[] szentekResourceName =
                [
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
                ];

        public static readonly string[] rssResourceNewsName =
                [
                        Activities.kurir.ToString(),
                    Activities.bonumtv.ToString(),
                    Activities.keresztenyelet.ToString()
                ];

        public static readonly string[] rssResourceAudioName =
                [
                        Activities.prayasyougo.ToString(),
                    Activities.audiopalferi.ToString(),
                    Activities.audiobarsi.ToString(),
                    Activities.audiohorvath.ToString(),
                    Activities.audiotaize.ToString(),
                    Activities.audionapievangelium.ToString()
                ];

        public static readonly string[] rssResourceMagazinName =
                [
                        Activities.b777.ToString(),
                    Activities.bzarandokma.ToString(),
                    Activities.jezsuitablog.ToString()
                ];
}
