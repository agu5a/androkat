using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
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
    public DateTime Fulldatum
    {
        get
        {
            string year = Datum == "02-29" ? "2024" : DateTime.UtcNow.ToString("yyyy");
            return DateTime.TryParse(year + "-" + Datum, CultureInfo.CreateSpecificCulture("hu-HU"), out DateTime date) ? date : DateTime.MinValue;
        }
    }

    [DataMember(Name = "cim")]
    public string Cim { get; set; }

    [DataMember(Name = "idezet")]
    public string Idezet { get; set; }

    [DataMember(Name = "tipus")]
    public int Tipus { get; set; }

    [NotMapped]
    public DateTime Inserted
    {
        get
        {
            return DateTime.UtcNow;
        }
    }
}