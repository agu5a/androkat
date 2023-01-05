using System;
using System.Collections.Generic;

namespace androkat.domain.Model.ContentCache;

public class MainCache
{
    public List<ContentDetailsModel> ContentDetailsModels { get; set; }
    public DateTime Inserted { get; set; }
}