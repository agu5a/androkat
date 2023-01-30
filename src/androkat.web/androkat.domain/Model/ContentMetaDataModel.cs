using androkat.domain.Enum;

namespace androkat.domain.Model;

public class ContentMetaDataModel
{
    public ContentMetaDataModel(Forras tipusId, string tipusNev, string forras, string link, string segedlink, string image)
    {
        TipusId = tipusId;
        TipusNev = tipusNev;
        Forras = forras;
        Link = link;
        Segedlink = segedlink;
        Image = image;
    }

    public Forras TipusId { get; }
    public string TipusNev { get; }
    public string Forras { get; }
    public string Link { get; }
    public string Segedlink { get; }
    public string Image { get; }
}