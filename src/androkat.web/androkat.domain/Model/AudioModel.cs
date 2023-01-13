using System;

namespace androkat.domain.Model;

public class AudioModel
{
    public string EncodedUrl { get; set; }
    public string ShareTitle { get; set; }
    public string Url { get; set; }
    public string Idezet { get; set; }
    public DateTime Inserted { get; set; }
    public string Cim { get; set; }
    public int Tipus { get; set; }
    public ContentMetaDataModel MetaDataModel { get; set; }
}