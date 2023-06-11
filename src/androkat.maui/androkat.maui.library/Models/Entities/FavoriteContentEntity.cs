using SQLite;
using System.Runtime.Serialization;

namespace androkat.maui.library.Models.Entities;

[Table("FavoriteContent_V1")]
public class FavoriteContentEntity
{
    [PrimaryKey]
    [DataMember(Name = "nid")]

    public Guid Nid { get; set; }

    /// <summary>
    /// recorddate, time
    /// </summary>
    [DataMember(Name = "datum")]
    public DateTime Datum { get; set; }

    /// <summary>
    /// title
    /// </summary>
    //[MaxLength(250), Unique]
    [DataMember(Name = "cim")]
    public string Cim { get; set; }

    /// <summary>
    /// content, leiras
    /// </summary>
    [DataMember(Name = "idezet")]
    public string Idezet { get; set; }

    [DataMember(Name = "tipus")]
    public string Tipus { get; set; }

    [DataMember(Name = "img")]
    public string Image { get; set; }

    [DataMember(Name = "forras")]
    public string Forras { get; set; }

    [DataMember(Name = "TypeName")]
    public string TypeName { get; set; }

    [DataMember(Name = "KulsoLink")]
    public string KulsoLink { get; set; }
}