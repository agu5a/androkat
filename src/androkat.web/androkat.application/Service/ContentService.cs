using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Enum;
using androkat.domain.Model;
using System.Collections.Generic;

namespace androkat.application.Service;

public class ContentService : IContentService
{
    private readonly IContentRepository _repository;

    public ContentService(IContentRepository repository)
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
}