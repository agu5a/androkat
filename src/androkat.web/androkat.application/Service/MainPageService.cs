using androkat.application.Interfaces;
using androkat.domain.Enum;
using androkat.domain.Model;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace androkat.application.Service;

public class MainPageService : IMainPageService
{
    private readonly IMapper _mapper;

    public MainPageService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public List<ContentModel> GetHome()
    {
        return new List<ContentModel> 
        {
            new ContentModel
            {
                MetaData = new ContentMetaDataModel
                {
                    TipusId = Forras.papaitwitter,
                    TipusNev = "Ferenc pápa twitter üzenete",
                    Image = "images/ferencpapa.png"
                },
                ContentDetails = new ContentDetailsModel
                {
                    Cim = "Twitter cím",
                    Fulldatum = DateTime.Now,
                    Nid = Guid.NewGuid()
                }
            },
            new ContentModel
            {
                MetaData = new ContentMetaDataModel
                {
                    TipusId = Forras.advent,
                    TipusNev = "Advent",
                    Image = "images/advent.png"
                },
                ContentDetails = new ContentDetailsModel
                {
                    Cim = "Advent cím",
                    Fulldatum = DateTime.Now,
                    Nid = Guid.NewGuid()
                }
            }
        };
    }
}