using SQLite;
using System.Runtime.Serialization;

namespace androkat.maui.library.Models.Entities;

[Table("gyonasiJegyzetv1")]
public class GyonasiJegyzet
{
    [PrimaryKey]
    [DataMember(Name = "nid")]
    public Guid Nid { get; set; }

    [DataMember(Name = "jegyzet")]
    public string Jegyzet { get; set; } = string.Empty;    
}