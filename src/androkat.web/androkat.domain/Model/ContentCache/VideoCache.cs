using System;
using System.Collections.Generic;

namespace androkat.domain.Model.ContentCache;

public class VideoCache
{
	public List<VideoSourceModel> VideoSource { get; set; }
	public List<VideoModel> Video { get; set; }
	public DateTime Inserted { get; set; }
}