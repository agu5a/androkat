using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.application.Interfaces;

public interface IContentService
{
    IEnumerable<ContentModel> GetAjanlat();
    IEnumerable<ContentModel> GetHome();
    IEnumerable<ContentModel> GetSzentek();
    IEnumerable<ContentModel> GetImaPage(string csoport);
}