using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.application.Interfaces;

public interface IContentMetaDataService
{
    public IEnumerable<ContentMetaDataModel> GetContentMetaDataList(string path = "./Data/IdezetData.json");
}