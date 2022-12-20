using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.application.Interfaces;

public interface IMainPageService
{
    IEnumerable<ContentModel> GetAjanlat();
    IEnumerable<ContentModel> GetHome();
}