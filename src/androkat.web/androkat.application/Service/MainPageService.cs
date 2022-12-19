using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Enum;
using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.application.Service;

public class MainPageService : IMainPageService
{
    private readonly IContentRepository _repository;

    public MainPageService(IContentRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<ContentModel> GetHome()
    {
        var result = _repository.GetContentDetailsModel(new int[] { (int)Forras.advent, 11, 14, (int)Forras.nagybojt, 17, 13, 9, 45, 7, 48, 19, 47, 24 });
        return result;
    }
}