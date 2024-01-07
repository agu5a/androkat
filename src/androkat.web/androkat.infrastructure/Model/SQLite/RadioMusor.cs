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
    public string Source { get; set; } = string.Empty;

    [DataMember(Name = "musor")]
    public string Musor { get; set; } = string.Empty;

    [DataMember(Name = "inserted")]
    public string Inserted { get; set; } = string.Empty; //"yyyy-MM-dd HH:mm:ss"
}