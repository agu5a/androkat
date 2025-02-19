using androkat.domain.Enum;
using androkat.domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace androkat.domain.Configuration;

public class AndrokatConfiguration
{
    public IEnumerable<ContentMetaDataModel> ContentMetaDataList { get; set; }

    public ContentMetaDataModel GetContentMetaDataModelByTipus(int tipus)
    {
        var forras = (Forras)tipus;
        return ContentMetaDataList.FirstOrDefault(f => f.TipusId == forras);
    }

    public static IEnumerable<int> FixContentTypeIds()
    {
        return new List<int> { (int)Forras.pio, (int)Forras.janospal, (int)Forras.sztjanos, (int)Forras.kisterez,
            (int)Forras.terezanya, (int)Forras.bojte, (int)Forras.kempis, (int)Forras.humor, (int)Forras.vianney, (int)Forras.prohaszka,
            (int)Forras.szentszalezi, (int)Forras.sienaikatalin, (int)Forras.medjugorje, (int)Forras.mello };
    }

    public static IEnumerable<int> ContentTypeIds()
    {
        return new List<int>
        {
            (int)Forras.audiohorvath, (int)Forras.fokolare, (int)Forras.papaitwitter, (int)Forras.advent, (int)Forras.maievangelium,
            (int)Forras.ignac, (int)Forras.barsi, (int)Forras.horvath, (int)Forras.prayasyougo, (int)Forras.nagybojt, (int)Forras.szeretetujsag,
            (int)Forras.audionapievangelium, (int)Forras.audiobarsi, (int)Forras.audiopalferi, (int)Forras.regnum, (int)Forras.taize, (int)Forras.szentbernat,
            (int)Forras.laciatya, (int)Forras.ajanlatweb, (int)Forras.audiotaize
        };
    }

    //21 mai szent 46 book
    public static IEnumerable<int> BlogNewsContentTypeIds()
    {
        return new List<int>
        {
            (int)Forras.kurir, (int)Forras.bonumtv, (int)Forras.keresztenyelet, (int)Forras.b777, (int)Forras.bzarandokma, (int)Forras.jezsuitablog
        };
    }
}