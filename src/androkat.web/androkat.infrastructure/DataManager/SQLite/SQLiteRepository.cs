using androkat.domain;
using androkat.domain.Model;
using androkat.infrastructure.Configuration;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace androkat.infrastructure.DataManager.SQLite;

public class SQLiteRepository : BaseRepository, ISQLiteRepository
{
    private readonly ILogger<SQLiteRepository> _logger;
    private readonly IOptions<AndrokatConfiguration> _androkatConfiguration;

    public SQLiteRepository(
        ILogger<SQLiteRepository> logger,
        IMapper mapper,
        IOptions<AndrokatConfiguration> androkatConfiguration
        ) : base(mapper)
    {
        _logger = logger;
        _androkatConfiguration = androkatConfiguration;
    }

    public IEnumerable<ContentModel> GetContentDetailsModel(List<int> tipusok)
    {
        var result = new List<Napiolvaso>
        {
            new Napiolvaso
            {
                    Cim = "Twitter cím",
                    Fulldatum = DateTime.Now.ToString(),
                    Nid = Guid.NewGuid(),
                    Tipus = 9
            },
            new Napiolvaso
            {
                    Cim = "Advent cím",
                    Fulldatum = DateTime.Now.ToString(),
                    Nid = Guid.NewGuid(),
                    Tipus= 10
            }
        };

        var list = new List<ContentModel>();

        foreach (var item in result)
            list.Add(new ContentModel
            {
                ContentDetails = _mapper.Map<ContentDetailsModel>(item),
                MetaData = _mapper.Map<ContentMetaDataModel>(_androkatConfiguration.Value.GetContentMetaDataModelByTipus(item.Tipus))
            });

        return list;
    }
}