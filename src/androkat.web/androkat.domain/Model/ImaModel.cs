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

	public Guid Nid { get; private set; }
	public DateTime Datum { get; private set; }
	public string Cim { get; private set; }
	public string Csoport { get; private set; }
	public string Szoveg { get; private set; }
}