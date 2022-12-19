using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.domain;

public interface IContentRepository
{
    IEnumerable<ContentModel> GetContentDetailsModel(List<int> tipusok);
}