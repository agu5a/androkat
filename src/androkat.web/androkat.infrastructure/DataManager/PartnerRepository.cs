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
    private readonly AndrokatContext _ctx;
    private readonly ILogger<PartnerRepository> _logger;
    private readonly IClock _clock;
    private readonly IMapper _mapper;

    public PartnerRepository(AndrokatContext ctx, ILogger<PartnerRepository> logger, IClock clock, IMapper mapper)
    {
        _ctx = ctx;
        _logger = logger;
        _clock = clock;
        _mapper = mapper;
    }

    public ContentDetailsModel GetTempContentByNid(string nid)
    {
        _logger.LogDebug("GetTempContentByNid was called, nid: {nid}", nid);
        try
        {
            var guid = Guid.Parse(nid);
            var res = _ctx.TempContent.FirstOrDefault(w => w.Nid == guid);
            return _mapper.Map<ContentDetailsModel>(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: GetTempContentByNid is failed {nid}", nid);
        }
        return null;
    }

    public IEnumerable<ContentDetailsModel> GetTempContentByTipus(int tipus)
    {
        _logger.LogDebug("GetTempContentByTipus was called, tipus: {tipus}", tipus);
        try
        {
            var res = _ctx.TempContent.Where(w => w.Tipus == tipus).OrderBy(o => o.Fulldatum);
            return _mapper.Map<List<ContentDetailsModel>>(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: GetTempContentByTipus is failed {tipus}", tipus);
        }
        return [];
    }

    public bool InsertTempContent(ContentDetailsModel contentDetailsModel)
    {
        _logger.LogDebug("InsertTempContent was called, nid: {nid}", contentDetailsModel.Nid);
        try
        {
            var res = _ctx.TempContent.Find(contentDetailsModel.Nid);
            if (res is not null)
            {
                return false;
            }
            _ctx.TempContent.Add(_mapper.Map<TempContent>(contentDetailsModel));
            _ctx.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: InsertTempContent is failed");
        }
        return false;
    }

    public bool LogInUser(string email)
    {
        _logger.LogDebug("LogInUser was called, email: {email}", email);
        try
        {
            var res = _ctx.Admin.FirstOrDefault(f => f.Email == email);
            if (res is not null)
            {
                res.LastLogin = _clock.Now.DateTime;
                _ctx.SaveChanges();
                return true;
            }

            _ctx.Admin.Add(new Admin { Email = email, LastLogin = _clock.Now.DateTime });
            _ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: LogInUser is failed {email}", email);
        }
        return false;
    }

    public bool DeleteTempContentByNid(string nid)
    {
        _logger.LogDebug("DeleteTempContentByNid was called, nid: {nid}", nid);
        try
        {
            var guid = Guid.Parse(nid);
            var res = _ctx.TempContent.Find(guid);
            if (res is not null)
            {
                _ctx.TempContent.Remove(res);
                _ctx.SaveChanges();
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: DeleteTempContentByNid is failed {nid}", nid);
        }
        return false;
    }
}