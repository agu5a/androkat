using System.ComponentModel.DataAnnotations;

namespace androkat.web.ViewModels;

public class InsertData
{
    [Required]
    public required int Tipus { get; set; }
    [Required]
    public string Datum { get; set; }
    [Required]
    public string Cim { get; set; }
    [Required]
    public string Idezet { get; set; }
}
