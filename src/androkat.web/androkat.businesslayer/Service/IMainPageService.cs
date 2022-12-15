using androkat.businesslayer.Model;
using System.Collections.Generic;

namespace androkat.businesslayer.Service;

public interface IMainPageService
{
    List<MainViewModel> GetHome();
}