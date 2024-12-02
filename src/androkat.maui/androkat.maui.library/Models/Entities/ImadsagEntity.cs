using SQLite;
using System.Runtime.Serialization;

namespace androkat.maui.library.Models.Entities;

[Table("Imadsag_V1")]
public class ImadsagEntity
{
    [DataMember(Name = "nid")]
    public Guid Nid { get; set; }

    [DataMember(Name = "typeName")]
    public string TypeName { get; set; } = string.Empty;

    [DataMember(Name = "isread")]
    public int IsRead { get; set; }

    [DataMember(Name = "ishided")]
    public bool IsHided { get; set; }

    [DataMember(Name = "cim")]
    public string Cim { get; set; } = string.Empty;

    [DataMember(Name = "leiras")]
    public string Content { get; set; } = string.Empty;

    [DataMember(Name = "datum")]
    public DateTime Datum { get; set; }

    [DataMember(Name = "groupName")]
    public string GroupName { get; set; } = string.Empty;

    [DataMember(Name = "csoport")]
    public int Csoport { get; set; }
}
