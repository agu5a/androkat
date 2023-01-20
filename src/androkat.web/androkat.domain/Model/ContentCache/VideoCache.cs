using System;
using System.Collections.Generic;

namespace androkat.domain.Model.ContentCache;

public class VideoCache
{
    public IReadOnlyCollection<VideoSourceModel> VideoSource { get; set; }
    public IReadOnlyCollection<VideoModel> Video { get; set; }
    public DateTime Inserted { get; set; }
}