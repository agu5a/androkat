using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace androkat.application.Service;

public class ContentService : IContentService
{
    private readonly IContentRepository _repository;
    private readonly IOptions<AndrokatConfiguration> _androkatConfiguration;

    public ContentService(IContentRepository repository, IOptions<AndrokatConfiguration> androkatConfiguration)
    {
        _repository = repository;
    }

    public IEnumerable<ContentModel> GetHome()
    {
        var result = _repository.GetContentDetailsModel(new int[] 
        { 
            (int)Forras.advent, 
            (int)Forras.maievangelium,
            (int)Forras.horvath, 
            (int)Forras.nagybojt, 
            (int)Forras.bojte, 
            (int)Forras.barsi, 
            (int)Forras.papaitwitter,
            (int)Forras.regnum, 
            (int)Forras.fokolare,
            (int)Forras.prohaszka, 
            (int)Forras.kempis,
            (int)Forras.taize,
            (int)Forras.szeretetujsag
        });
        return result;
    }

    public IEnumerable<ContentModel> GetAjanlat()
    {
        var result = _repository.GetContentDetailsModel(new int[] { (int)Forras.ajanlatweb });
        return result;
    }

    public IEnumerable<ContentModel> GetSzentek()
    {
        var result = _repository.GetContentDetailsModel(new int[] 
        {
            (int)Forras.vianney,
            (int)Forras.pio,
            (int)Forras.janospal,
            (int)Forras.szentbernat,
            (int)Forras.sztjanos,
            (int)Forras.kisterez,
            (int)Forras.terezanya,
            (int)Forras.ignac,
            (int)Forras.szentszalezi,
            (int)Forras.sienaikatalin 
        });
        return result;
    }

    public IEnumerable<ContentModel> GetImaPage(string csoport)
    {
        var list = new List<ContentModel>();
        var result = GetIma(csoport);
        foreach (var item in result)
            list.Add(new ContentModel
            {
                ContentDetails = new ContentDetailsModel
                {
                    Nid = item.Nid,
                    Cim = item.Cim
                }
            });

        return list;
    }

    private IEnumerable<ImaViewModel> GetIma(string csoport)
    {
        var result = new List<ImaViewModel>()
        {
            new ImaViewModel
            {
                Cim = "Ima Cím",
                Nid = Guid.NewGuid()
            }
        };

        return result;
    }
}