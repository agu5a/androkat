using System;

namespace androkat.domain.Model;

public class AudioModel
{
    public AudioModel(string encodedUrl, string shareTitle, string url, string idezet, DateTime inserted, string cim, int tipus, ContentMetaDataModel metaDataModel)
    {
        EncodedUrl = encodedUrl;
        ShareTitle = shareTitle;
        Url = url;
        Idezet = idezet;
        Inserted = inserted;
        Cim = cim;
        Tipus = tipus;
        MetaDataModel = metaDataModel;
    }

    public string EncodedUrl { get; set; }
    public string ShareTitle { get; set; }
    public string Url { get; set; }
    public string Idezet { get; set; }
    public DateTime Inserted { get; set; }
    public string Cim { get; set; }
    public int Tipus { get; set; }
    public ContentMetaDataModel MetaDataModel { get; set; }
}