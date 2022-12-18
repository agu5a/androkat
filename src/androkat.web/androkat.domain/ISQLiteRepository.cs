using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.domain;

public interface ISQLiteRepository
{
    IEnumerable<ContentModel> GetContentDetailsModel(List<int> tipusok);
}