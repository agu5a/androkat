using androkat.businesslayer.Model;
using androkat.datalayer.Model;
using androkat.datalayer.Model.SQLite;
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
                TipusId = datalayer.Enum.Forras.papaitwitter,
                TipusNev = "Pápa Twitter",
                Image = "Image"
            }),
            Napiolvaso = _mapper.Map<NapiOlvasoViewModel>(new Napiolvaso
            {
                Cim = "Twitter cím",
                Datum = DateTime.Now.ToString("yyyy-MM-dd"),
                Nid = Guid.NewGuid()
            })
        }};
    }
}