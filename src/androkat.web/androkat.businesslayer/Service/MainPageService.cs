using androkat.domain.Enum;
using androkat.domain.Model;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace androkat.businesslayer.Service;

public class MainPageService : IMainPageService
{
    private readonly IMapper _mapper;

    public MainPageService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public List<MainViewModel> GetHome()
    {
        return new List<MainViewModel> { new MainViewModel
        {
            IdezetData = _mapper.Map<IdezetDataViewModel>(new IdezetData
            {
                TipusId = Forras.papaitwitter,
                TipusNev = "Pápa Twitter",
                Image = "Image"
            }),
            Napiolvaso = new NapiOlvasoViewModel
            {
                Cim = "Twitter cím",
                Fulldatum = DateTime.Now,
                Nid = Guid.NewGuid()
            }
        }};
    }
}