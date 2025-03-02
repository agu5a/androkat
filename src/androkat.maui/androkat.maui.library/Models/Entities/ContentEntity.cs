using SQLite;
using System.Runtime.Serialization;

namespace androkat.maui.library.Models.Entities;

[Table("napielmelkedesv1")]
public class ContentEntity
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
    public string Cim { get; set; } = string.Empty;

    /// <summary>
    /// content, leiras
    /// </summary>
    [DataMember(Name = "idezet")]
    public string Idezet { get; set; } = string.Empty;

    [DataMember(Name = "tipus")]
    public string Tipus { get; set; } = string.Empty;

    [DataMember(Name = "img")]
    public string Image { get; set; } = string.Empty;

    [DataMember(Name = "forras")]
    public string Forras { get; set; } = string.Empty;

    [DataMember(Name = "TypeName")]
    public string TypeName { get; set; } = string.Empty;

    [DataMember(Name = "IsRead")]
    public bool IsRead { get; set; }

    [DataMember(Name = "KulsoLink")]
    public string KulsoLink { get; set; } = string.Empty;

    [DataMember(Name = "GroupName")]
    public string GroupName { get; set; } = string.Empty;
}