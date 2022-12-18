using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.application.Service;

public class MainPageService : IMainPageService
{
    private readonly ISQLiteRepository _sqliteRepository;

    public MainPageService(ISQLiteRepository sqliteRepository)
    {
        _sqliteRepository = sqliteRepository;
    }

    public IEnumerable<ContentModel> GetHome()
    {
        return _sqliteRepository.GetContentDetailsModel(new List<int>());
    }
}