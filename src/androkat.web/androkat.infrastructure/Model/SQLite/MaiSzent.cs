using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace androkat.infrastructure.Model.SQLite;

[Table("maiszent")]
public class Maiszent
{
    [Key]
    [Required]
    [DataMember(Name = "nid")]
    public Guid Nid { get; set; } 

    [StringLength(100)]
    [DataMember(Name = "datum")]
    public string Datum { get; set; } //"MM-dd"

    [NotMapped]
    public DateTime FullDate
    {
        get
        {
            _ = DateTime.TryParse(DateTime.Now.ToString("yyyy") + "-" + Datum, out DateTime date);
            return date;
        }
    }

    [DataMember(Name = "cim")]
    public string Cim { get; set; }

    [DataMember(Name = "idezet")]
    public string Idezet { get; set; }

    [DataMember(Name = "inserted")]
    public DateTime Inserted { get; set; } //"yyyy-MM-dd HH:mm:ss"

    [StringLength(200)]
    [DataMember(Name = "img")]
    public string Img { get; set; }  
}