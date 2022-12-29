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
using System.Text.Json;

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

    public IReadOnlyCollection<AudioModel> GetAudio()
    {
        var list = new List<AudioModel>();

        list.AddRange(GetAudioViewModel((int)Forras.audionapievangelium));
        list.AddRange(GetAudioViewModel((int)Forras.prayasyougo).OrderByDescending(o => o.Inserted).Take(2));
        list.AddRange(GetAudioViewModel((int)Forras.audiobarsi));
        list.AddRange(GetAudioViewModel((int)Forras.audiopalferi));
        list.AddRange(GetAudioViewModel((int)Forras.audiohorvath).OrderByDescending(o => o.Inserted).Take(2));
        list.AddRange(GetAudioViewModel((int)Forras.audiotaize));

        return list;
    }

    public IReadOnlyCollection<VideoSourceModel> GetVideoSourcePage()
    {
        var list = new List<VideoSourceModel>();
        var result = GetVideoSource();
        foreach (var item in result)
            list.Add(new VideoSourceModel
            {
                ChannelId = item.ChannelId,
                ChannelName = item.ChannelName
            });

        return list;
    }

    public IReadOnlyCollection<RadioModel> GetRadioPage()
    {
        var list = new List<RadioModel>();
        list.AddRange(GetRadio().Select(item => new RadioModel { Name = item.Key, Url = item.Value }));

        return list;
    }

    public IReadOnlyCollection<ContentModel> GetSzent()
    {
        var result = GetContentDetailsModel(new int[] { (int)Forras.maiszent });
        return result;
    }

    private IReadOnlyCollection<ContentModel> GetContentDetailsModel(int[] tipusok)
    {
        var list = new List<ContentModel>();

        var result = Get(tipusok);
        foreach (var item in result)
            list.Add(new ContentModel { ContentDetails = _mapper.Map<ContentDetailsModel>(item), MetaData = _mapper.Map<ContentMetaDataModel>(_androkatConfiguration.Value.GetContentMetaDataModelByTipus(item.Tipus)) });

        return list;
    }

    private IEnumerable<ContentDetailsModel> Get(int[] tipusok)
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
                    Tipus = (int)Forras.pio                    
            },
            new ContentDetailsModel
            {
                    Cim = "Mai szent cím",
                    Fulldatum = DateTime.Now,
                    Nid = Guid.NewGuid(),
                    Tipus = (int)Forras.maiszent
            }
        };

        return result.Where(w => tipusok.Contains(w.Tipus));
    }

    private List<AudioModel> GetAudioViewModel(int tipus)
    {
        var list = new List<AudioModel>
        {
            new AudioModel
            {
                Cim = "Audio cím",
                Idezet = "Idézet",
                MetaDataModel = new ContentMetaDataModel
                {
                    Link = "link",
                    TipusId = (Forras)Enum.ToObject(typeof(Forras), tipus),
                    Image = "images/palferi.png",
                },
                Tipus = tipus
            }
        };

        return list;
    }

    private IEnumerable<ImaModel> GetIma(string csoport)
    {
        var result = new List<ImaModel>
        {
            new ImaModel
            {
                Cim = "Ima Cím",
                Nid = Guid.NewGuid()
            }
        };

        return result;
    }

    private Dictionary<string, string> GetRadio()
    {
        var ra = "{\"szentistvan\":\"http://online.szentistvanradio.hu:7000/adas\",\"vatikan\":\"https://media.vaticannews.va/media/audio/program/504/ungherese_181222.mp3\",\"ezazanap\":\"https://www.radioking.com/play/ez-az-a-nap-radio\",\"mariaszerbia\":\"http://dreamsiteradiocp.com:8014/stream\",\"katolikus\":\"http://katolikusradio.hu:9000/live_hi.mp3\",\"maria\":\"http://www.mariaradio.hu:8000/mr\",\"mariaerdely\":\"http://stream.mariaradio.ro:8000/MRE\",\"mariaszlovakia\":\"http://193.87.81.131:8081/MariaRadioFelvidek\",\"solaradio\":\"http://188.165.11.30:7000/live.mp3\"}";

        var dic = JsonSerializer.Deserialize<Dictionary<string, string>>(ra);
        if (dic == null)
            return new Dictionary<string, string>();

        return dic;
    }

    private IEnumerable<VideoSourceModel> GetVideoSource()
    {
        var result = new List<VideoSourceModel>
        {
            new VideoSourceModel
            {
                ChannelId = "UCF3mEbdkhZwjQE8reJHm4sg",
                ChannelName = "AndroKat"
            }
        };
        return result;
    }
}