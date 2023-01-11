using androkat.domain;
using androkat.domain.Configuration;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace androkat.web.Endpoints.ContentDetailsModelEndpoints;

public class Create : Endpoint<ContentDetailsModelRequest, ContentDetailsModelResponse>
{
	private readonly IApiRepository _apiRepository;
	private readonly ILogger<Create> _logger;
	private readonly string _route;

	public Create(IApiRepository apiRepository, ILogger<Create> logger, IOptions<EndPointConfiguration> endPointsCongfig)
	{
		_apiRepository = apiRepository;
		_logger = logger;
		_route = endPointsCongfig.Value.SaveContentDetailsModelApiUrl;
	}

	public override void Configure()
	{
		Post(_route);
		AllowAnonymous();
	}

	public override async Task HandleAsync(ContentDetailsModelRequest request, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(request.ContentDetailsModel);
		await AddContentDetailsModel(request, ct);
	}

	private async Task AddContentDetailsModel(ContentDetailsModelRequest request, CancellationToken ct)
	{
		ContentDetailsModelResponse response;

		try
		{
			bool result = _apiRepository.AddContentDetailsModel(request.ContentDetailsModel);
			response = new ContentDetailsModelResponse(result);
			await SendAsync(response, result ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, ct);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception: Failed to run {name}", nameof(HandleAsync));

			await SendAsync(new ContentDetailsModelResponse(false), StatusCodes.Status500InternalServerError, ct);
		}
	}
}
