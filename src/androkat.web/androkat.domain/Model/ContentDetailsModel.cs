using System;

namespace androkat.domain.Model;

public class ContentDetailsModel
{
    public ContentDetailsModel(Guid nid, DateTime fulldatum, string cim,
        string idezet, int tipus, DateTime inserted = default, string kulsoLink = default, string img = default, string fileUrl = default, string forras = default)
    {
        Nid = nid;
        Fulldatum = fulldatum;
        Cim = cim;
        Idezet = idezet;
        Tipus = tipus;
        Inserted = inserted;
        KulsoLink = kulsoLink;
        Img = img;
        FileUrl = fileUrl;
        Forras = forras;
    }

    public Guid Nid { get; }
    public DateTime Fulldatum { get; }
    public string Cim { get; }
    public string Idezet { get; }
    public int Tipus { get; }
    public DateTime Inserted { get; }
    public string KulsoLink { get; }
    public string Img { get; }
    public string FileUrl { get; }
    public string Forras { get; }
}