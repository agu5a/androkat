using System;

namespace androkat.domain.Model;

public class ImaModel
{
	public ImaModel(Guid nid, DateTime datum, string cim, string csoport, string szoveg)
	{
		Nid = nid;
		Datum = datum;
		Cim = cim;
		Csoport = csoport;
		Szoveg = szoveg;
	}

	public Guid Nid { get; }
    public DateTime Datum { get; }
    public string Cim { get; }
    public string Csoport { get; }
    public string Szoveg { get; }
}