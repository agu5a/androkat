using System;

namespace androkat.domain.Model;

public class ContentDetailsModel
{
    public Guid Nid { get; set; }
    public DateTime Fulldatum { get; set; }
    public string Cim { get; set; }
    public string Idezet { get; set; }
    public int Tipus { get; set; }
    public DateTime Inserted { get; set; }
    public string KulsoLink { get; set; }
    public string Img { get; set; }
    public string FileUrl { get; set; }    
}