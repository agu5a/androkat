using androkat.domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
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
    public DateTime Fulldatum
    {
        get
        {
            var year = Datum == "02-29" ? "2024" : DateTime.UtcNow.ToString("yyyy");
            return DateTime.TryParse(year + "-" + Datum, CultureInfo.CreateSpecificCulture("hu-HU"), out var date) ? date : DateTime.MinValue;
        }
    }

    [NotMapped]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public int Tipus => (int)Forras.maiszent;

    [DataMember(Name = "cim")]
    public string Cim { get; set; } = string.Empty;

    [DataMember(Name = "idezet")]
    public string Idezet { get; set; } = string.Empty;

    [DataMember(Name = "inserted")]
    public DateTime Inserted { get; set; } //"yyyy-MM-dd HH:mm:ss"

    [StringLength(200)]
    [DataMember(Name = "img")]
    public string Img { get; set; }  
}