using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace androkat.infrastructure.Model.SQLite;

[Table("ima")]
public class ImaContent
{
    [Key]
    [Required]
    [DataMember(Name = "nid")]
    public Guid Nid { get; set; }

    [DataMember(Name = "datum")]
    public DateTime Datum { get; set; } //"yyyy-MM-dd HH:mm:ss"

    [DataMember(Name = "cim")]
    public string Cim { get; set; }

    [DataMember(Name = "csoport")]
    public string Csoport { get; set; }

    [DataMember(Name = "szoveg")]
    public string Szoveg { get; set; }
}