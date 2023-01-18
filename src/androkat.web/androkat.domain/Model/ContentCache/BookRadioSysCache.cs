using System;
using System.Collections.Generic;

namespace androkat.domain.Model.ContentCache;

public class BookRadioSysCache
{
    public List<ContentDetailsModel> Books { get; set; }
    public List<SystemInfoModel> SystemData { get; set; }
    public List<RadioMusorModel> RadioMusor { get; set; }
    public DateTime Inserted { get; set; }
}