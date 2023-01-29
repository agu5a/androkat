using System;

namespace androkat.domain.Model;

public class RadioMusorModel
{
    public RadioMusorModel(Guid nid, string source, string musor, string inserted)
    {
        Nid = nid;
        Source = source;
        Musor = musor;
        Inserted = inserted;
    }

    public Guid Nid { get; }
    public string Source { get; }
    public string Musor { get; }
    public string Inserted { get; }
}