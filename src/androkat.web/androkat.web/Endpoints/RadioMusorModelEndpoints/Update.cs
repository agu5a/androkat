using androkat.domain;
using androkat.domain.Configuration;
using androkat.web.Service;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace androkat.web.Endpoints.RadioMusorModelEndpoints;

public class Update : Endpoint<RadioMusorModelRequest, RadioMusorModelResponse>
{
	private readonly IApiRepository _apiRepository;
	private readonly ILogger<Update> _logger;
	private readonly string _route;
    private readonly IApiKeyValidator _apiKeyValidator;

    public Update(IApiRepository apiRepository, ILogger<Update> logger, IOptions<EndPointConfiguration> endPointsCongfig, IApiKeyValidator apiKeyValidator)
	{
		_apiRepository = apiRepository;
		_logger = logger;
		_route = endPointsCongfig.Value.UpdateRadioMusorModelApiUrl;
        _apiKeyValidator = apiKeyValidator;
	}

	public override void Configure()
	{
		Post(_route);
		AllowAnonymous();
	}

	public override async Task HandleAsync(RadioMusorModelRequest request, CancellationToken ct)
	{
        if (!_apiKeyValidator.IsValid(request.ApiKeyHeaderName))
        {
            await SendAsync(new RadioMusorModelResponse(false), StatusCodes.Status401Unauthorized, ct);
            return;
        }
        
		await UpdateRadioMusor(request, ct);
	}

	private async Task UpdateRadioMusor(RadioMusorModelRequest request, CancellationToken ct)
	{
		RadioMusorModelResponse response;

		try
		{
			bool result = _apiRepository.UpdateRadioMusor(new domain.Model.RadioMusorModel(Guid.Empty, request.Source, request.Musor, request.Inserted));
			response = new RadioMusorModelResponse(result);

			await SendAsync(response, result ? StatusCodes.Status200OK : StatusCodes.Status409Conflict, ct);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception: Failed to run {name}", nameof(HandleAsync));

			await SendAsync(new RadioMusorModelResponse(false), StatusCodes.Status500InternalServerError, ct);
		}
	}
}