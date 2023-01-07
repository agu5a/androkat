using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.domain;

public interface ICacheRepository
{
	IEnumerable<ContentDetailsModel> GetContentDetailsModelToCache();
	IEnumerable<ContentDetailsModel> GetHumorToCache();
	IEnumerable<ContentDetailsModel> GetMaiSzentToCache();
	IEnumerable<ContentDetailsModel> GetNapiFixToCache();
	IEnumerable<ImaModel> GetImaToCache();
}