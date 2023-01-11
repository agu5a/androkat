using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace androkat.infrastructure.Model.SQLite;

[Table("systeminfo")]
public class Systeminfo
{
    [Key]
    [Required]
    [DataMember(Name = "id")]
    public int Id { get; set; }

    [DataMember(Name = "key")]
    public string Key { get; set; }

    [DataMember(Name = "value")]
    public string Value { get; set; }
}