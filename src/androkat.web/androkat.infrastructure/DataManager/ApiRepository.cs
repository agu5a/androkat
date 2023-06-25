using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Model;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace androkat.infrastructure.DataManager;

public class ApiRepository : BaseRepository, IApiRepository
{
	public ApiRepository(AndrokatContext ctx, 
		IClock clock,
		IMapper mapper) : base(ctx, clock, mapper)
	{
	}

	public bool UpdateRadioMusor(RadioMusorModel radioMusorModel)
	{
		var old = _ctx.RadioMusor.FirstOrDefault(w => w.Source == radioMusorModel.Source);
        if (old is not null)
		{
			old.Inserted = radioMusorModel.Inserted;
			old.Musor = radioMusorModel.Musor;
			_ctx.SaveChanges();
			return true;
		}

		return false;
	}

	public bool AddContentDetailsModel(ContentDetailsModel contentDetailsModel)
	{
		var exist = _ctx.Content.FirstOrDefault(w => w.Tipus == contentDetailsModel.Tipus
		&& (w.Cim.Contains(contentDetailsModel.Cim) || w.Nid == contentDetailsModel.Nid));
        if (exist is not null)
			return false;

		_ctx.Content.Add(_mapper.Map<Content>(contentDetailsModel));
		_ctx.SaveChanges();
		return true;
	}

public bool AddTempContent(ContentDetailsModel contentDetailsModel)
    {
        var exist = _ctx.TempContent.FirstOrDefault(w => w.Tipus == contentDetailsModel.Tipus
       && (w.Cim.Contains(contentDetailsModel.Cim) || w.Nid == contentDetailsModel.Nid));
        if (exist is not null)
            return false;

        _ctx.TempContent.Add(_mapper.Map<TempContent>(contentDetailsModel));
        _ctx.SaveChanges();
        return true;
    }
}