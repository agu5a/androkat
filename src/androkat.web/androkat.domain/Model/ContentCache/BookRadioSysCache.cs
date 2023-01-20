using System;
using System.Collections.Generic;

namespace androkat.domain.Model.ContentCache;

public class BookRadioSysCache
{
    public IReadOnlyCollection<ContentDetailsModel> Books { get; set; }
    public IReadOnlyCollection<SystemInfoModel> SystemData { get; set; }
    public IReadOnlyCollection<RadioMusorModel> RadioMusor { get; set; }
    public DateTime Inserted { get; set; }
}