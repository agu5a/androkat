using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace androkat.infrastructure.Model.SQLite;

[Table("radio")]
public class RadioMusor
{
    [Key]
    [Required]
    [DataMember(Name = "nid")]
    public Guid Nid { get; set; }

    [DataMember(Name = "source")]
    public string Source { get; set; }

    [DataMember(Name = "musor")]
    public string Musor { get; set; }

    [DataMember(Name = "inserted")]
    public string Inserted { get; set; } //"yyyy-MM-dd HH:mm:ss"
}