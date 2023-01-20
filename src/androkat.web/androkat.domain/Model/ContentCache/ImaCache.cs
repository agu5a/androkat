using System;
using System.Collections.Generic;

namespace androkat.domain.Model.ContentCache;

public class ImaCache
{
    public IReadOnlyCollection<ImaModel> Imak { get; set; }
    public DateTime Inserted { get; set; }
}