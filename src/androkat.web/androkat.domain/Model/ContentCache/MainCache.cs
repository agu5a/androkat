using System;
using System.Collections.Generic;

namespace androkat.domain.Model.ContentCache;

public class MainCache
{
    public IReadOnlyCollection<ContentDetailsModel> ContentDetailsModels { get; set; }
    public IReadOnlyCollection<ContentDetailsModel> Egyeb { get; set; }
    public DateTime Inserted { get; set; }
}