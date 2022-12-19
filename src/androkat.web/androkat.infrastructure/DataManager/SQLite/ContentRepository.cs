using androkat.domain;
using androkat.domain.Enum;
using androkat.domain.Model;
using androkat.infrastructure.Configuration;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.infrastructure.DataManager.SQLite;

public class ContentRepository : BaseRepository, IContentRepository
{
    private readonly ILogger<ContentRepository> _logger;
    private readonly IOptions<AndrokatConfiguration> _androkatConfiguration;

    public ContentRepository(
        ILogger<ContentRepository> logger,
        IMapper mapper,
        IOptions<AndrokatConfiguration> androkatConfiguration
        ) : base(mapper)
    {
        _logger = logger;
        _androkatConfiguration = androkatConfiguration;
    }

    public IEnumerable<ContentModel> GetContentDetailsModel(int[] tipusok)
    {
        var result = new List<Napiolvaso>
        {
            new Napiolvaso
            {
                    Cim = "Twitter cím",
                    Fulldatum = DateTime.Now.ToString(),
                    Nid = Guid.NewGuid(),
                    Tipus = (int)Forras.papaitwitter
            },
            new Napiolvaso
            {
                    Cim = "Advent cím",
                    Fulldatum = DateTime.Now.ToString(),
                    Nid = Guid.NewGuid(),
                    Tipus= (int)Forras.advent
            },
            new Napiolvaso
            {
                    Cim = "Advent cím",
                    Fulldatum = DateTime.Now.ToString(),
                    Nid = Guid.NewGuid(),
                    Tipus= (int)Forras.bojte
            }
        };

        var list = new List<ContentModel>();

        foreach (var item in result)
            list.Add(new ContentModel
            {
                ContentDetails = _mapper.Map<ContentDetailsModel>(item),
                MetaData = _mapper.Map<ContentMetaDataModel>(_androkatConfiguration.Value.GetContentMetaDataModelByTipus(item.Tipus))
            });

        _logger.LogInformation("{Name} has been finished, count: {count}", nameof(GetContentDetailsModel), list.Count);

        return list;
    }
}