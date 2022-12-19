using androkat.application.Interfaces;
using androkat.domain;
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
        return _repository.GetContentDetailsModel(new List<int>());
    }
}