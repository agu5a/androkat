using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.domain;

public interface ICacheRepository
{
	IReadOnlyCollection<ContentDetailsModel> GetContentDetailsModelToCache();
	IReadOnlyCollection<ContentDetailsModel> GetHumorToCache();
	IReadOnlyCollection<ContentDetailsModel> GetMaiSzentToCache();
	IReadOnlyCollection<ContentDetailsModel> GetNapiFixToCache();
	IReadOnlyCollection<ImaModel> GetImaToCache();
	IReadOnlyCollection<VideoModel> GetVideoToCache();
	IReadOnlyCollection<VideoSourceModel> GetVideoSourceToCache();
}