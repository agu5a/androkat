using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace androkat.infrastructure.Model.SQLite;

[Table("napifixolvaso")]
public class FixContent
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
            return DateTime.TryParse(DateTime.Now.ToString("yyyy") + "-" + Datum, out DateTime date) ? date : DateTime.MinValue;
        }
    }

    [DataMember(Name = "cim")]
    public string Cim { get; set; }

    [DataMember(Name = "idezet")]
    public string Idezet { get; set; }

    [DataMember(Name = "tipus")]
    public int Tipus { get; set; }
}