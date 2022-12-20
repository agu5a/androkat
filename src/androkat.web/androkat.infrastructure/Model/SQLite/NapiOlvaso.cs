using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace androkat.infrastructure.Model.SQLite;

[Table("napiolvaso")]
public class Napiolvaso
{
    [Key]
    [Required]
    [DataMember(Name = "nid")]
    public Guid Nid { get; set; }

    [StringLength(100)]
    [DataMember(Name = "fulldatum")]
    public string Fulldatum { get; set; } //"yyyy-MM-dd"

    [DataMember(Name = "cim")]
    public string Cim { get; set; }

    [DataMember(Name = "tipus")]
    public int Tipus { get; set; }

    [StringLength(200)]
    [DataMember(Name = "img")]
    public string Img { get; set; }
}