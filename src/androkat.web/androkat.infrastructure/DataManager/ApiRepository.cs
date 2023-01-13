using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Model;
using androkat.infrastructure.Model.SQLite;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace androkat.infrastructure.DataManager;

public class ApiRepository : BaseRepository, IApiRepository
{
	private readonly ILogger<ApiRepository> _logger;

	public ApiRepository(AndrokatContext ctx, ILogger<ApiRepository> logger,
		IClock clock,
		IMapper mapper) : base(ctx, clock, mapper)
	{
		_logger = logger;
	}

	public bool UpdateRadioMusor(RadioMusorModel radioMusorModel)
	{
		var old = _ctx.RadioMusor.FirstOrDefault(w => w.Source == radioMusorModel.Source);
		if (old != null)
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
		if (exist != null)
			return false;

		_ctx.Content.Add(_mapper.Map<Content>(contentDetailsModel));
		_ctx.SaveChanges();
		return true;
	}
}