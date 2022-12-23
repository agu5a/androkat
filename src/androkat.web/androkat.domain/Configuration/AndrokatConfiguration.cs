using androkat.domain.Enum;
using androkat.domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace androkat.domain.Configuration;

public class AndrokatConfiguration
{
    public IEnumerable<ContentMetaDataModel> ContentMetaDataList { get; set; }

    public ContentMetaDataModel GetContentMetaDataModelByTipus(int tipus)
    {
        Forras forras = (Forras)tipus;
        return ContentMetaDataList.FirstOrDefault(f => f.TipusId == forras);
    }
}