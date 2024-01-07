using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Model;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.infrastructure.DataManager;

public class PartnerRepository : IPartnerRepository
{
#pragma warning disable S4487 // Unread "private" fields should be removed
    private readonly AndrokatContext _ctx;
    private readonly ILogger<PartnerRepository> _logger;
    private readonly IClock _clock;
    private readonly IMapper _mapper;
#pragma warning restore S4487 // Unread "private" fields should be removed

    public PartnerRepository(AndrokatContext ctx, ILogger<PartnerRepository> logger, IClock clock, IMapper mapper)
    {
        _ctx = ctx;
        _logger = logger;
        _clock = clock;
        _mapper = mapper;
    }

    public ContentDetailsModel GetTempContentByNid(string nid)
    {
        return null;
    }

    public IEnumerable<ContentDetailsModel> GetTempContentByTipus(int tipus)
    {
        return new List<ContentDetailsModel>();
    }

    public bool InsertTempContent(ContentDetailsModel contentDetailsModel)
    {
        return false;
    }

    public bool LogInUser(string email)
    {
        return false;
    }

    public bool DeleteTempContentByNid(string nid)
    {
        return false;
    }
}