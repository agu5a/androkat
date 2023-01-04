using System;

namespace androkat.domain.Model;

public class ImaModel
{
    public Guid Nid { get; set; }
    public DateTime Datum { get; set; }
    public string Cim { get; set; }
    public string Csoport { get; set; }
    public string Szoveg { get; set; }
}