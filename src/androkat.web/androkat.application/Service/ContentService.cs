using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Configuration;
using androkat.domain.Enum;
using androkat.domain.Model;
using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.application.Service;

public class ContentService : IContentService
{
    private readonly IContentRepository _repository;
    private readonly IMapper _mapper;
    private readonly IOptions<AndrokatConfiguration> _androkatConfiguration;

    public ContentService(IMapper mapper, IContentRepository repository, IOptions<AndrokatConfiguration> androkatConfiguration)
    {
        _mapper = mapper;
        _repository = repository;
        _androkatConfiguration = androkatConfiguration;
    }

    public IReadOnlyCollection<ContentModel> GetHome()
    {
        var result = GetContentDetailsModel(new int[]
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

    public IReadOnlyCollection<ContentModel> GetAjanlat()
    {
        var result = GetContentDetailsModel(new int[] { (int)Forras.ajanlatweb });
        return result;
    }

    public IReadOnlyCollection<ContentModel> GetSzentek()
    {
        var result = GetContentDetailsModel(new int[]
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

    public IReadOnlyCollection<ContentModel> GetImaPage(string csoport)
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

    public IReadOnlyCollection<AudioViewModel> GetAudio()
    {
        var list = new List<AudioViewModel>();

        list.AddRange(GetAudioViewModel((int)Forras.audionapievangelium));
        list.AddRange(GetAudioViewModel((int)Forras.prayasyougo).OrderByDescending(o => o.Inserted).Take(2));
        list.AddRange(GetAudioViewModel((int)Forras.audiobarsi));
        list.AddRange(GetAudioViewModel((int)Forras.audiopalferi)); 
        list.AddRange(GetAudioViewModel((int)Forras.audiohorvath).OrderByDescending(o => o.Inserted).Take(2)); 
        list.AddRange(GetAudioViewModel((int)Forras.audiotaize));

        return list;
    }

    public IReadOnlyCollection<VideoSourceViewModel> GetVideoSourcePage()
    {
        var list = new List<VideoSourceViewModel>();
        var result = GetVideoSource();
        foreach (var item in result)
            list.Add(new VideoSourceViewModel
            {
                ChannelId = item.ChannelId,
                ChannelName = item.ChannelName
            });

        return list;
    }

    private IReadOnlyCollection<ContentModel> GetContentDetailsModel(int[] tipusok)
    {
        var list = new List<ContentModel>();

        var result = Get(tipusok);
        foreach (var item in result)
            list.Add(new ContentModel { ContentDetails = _mapper.Map<ContentDetailsModel>(item), MetaData = _mapper.Map<ContentMetaDataModel>(_androkatConfiguration.Value.GetContentMetaDataModelByTipus(item.Tipus)) });

        return list;
    }

    private static IEnumerable<ContentDetailsModel> Get(int[] tipusok)
    {
        var result = new List<ContentDetailsModel>
        {
            new ContentDetailsModel
            {
                    Cim = "Twitter cím",
                    Fulldatum = DateTime.Now,
                    Nid = Guid.NewGuid(),
                    Tipus = (int)Forras.papaitwitter
            },
            new ContentDetailsModel
            {
                    Cim = "Advent cím",
                    Fulldatum = DateTime.Now,
                    Nid = Guid.NewGuid(),
                    Tipus = (int)Forras.advent
            },
            new ContentDetailsModel
            {
                    Cim = "Böjte cím",
                    Fulldatum = DateTime.Now,
                    Nid = Guid.NewGuid(),
                    Tipus = (int)Forras.bojte
            },
            new ContentDetailsModel
            {
                    Cim = "Ajánlat cím",
                    Fulldatum = DateTime.Now,
                    Nid = Guid.NewGuid(),
                    Tipus = (int)Forras.ajanlatweb,
                    Img = "image"
            },
            new ContentDetailsModel
            {
                    Cim = "Pio cím",
                    Fulldatum = DateTime.Now,
                    Nid = Guid.NewGuid(),
                    Tipus = (int)Forras.pio,
                    Img = "image"
            }
        };

        return result.Where(w => tipusok.Contains(w.Tipus));
    }

    private static List<AudioViewModel> GetAudioViewModel(int tipus)
    {
        var list = new List<AudioViewModel>
        {
            new AudioViewModel
            {
                Cim = "Audio cím",
                Idezet = "Idézet",
                MetaDataModel = new ContentMetaDataModel
                {
                    Link = "link",
                    TipusId = Forras.audionapievangelium
                },
                Tipus = (int)Forras.audionapievangelium
            }
        };

        return list;
    }


    private static IEnumerable<ImaViewModel> GetIma(string csoport)
    {
        var result = new List<ImaViewModel>
        {
            new ImaViewModel
            {
                Cim = "Ima Cím",
                Nid = Guid.NewGuid()
            }
        };

        return result;
    }

    private static IEnumerable<VideoSourceViewModel> GetVideoSource()
    {
        var result = new List<VideoSourceViewModel>
        {
            new VideoSourceViewModel
            {
                ChannelId = "UCF3mEbdkhZwjQE8reJHm4sg",
                ChannelName = "AndroKat"
            }
        };
        return result;
    }
}