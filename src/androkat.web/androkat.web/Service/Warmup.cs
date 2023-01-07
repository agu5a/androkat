using androkat.application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace androkat.web.Service;

[ExcludeFromCodeCoverage]
public class Warmup : IHostedService
{
	private readonly IServiceProvider _serviceProvider;

	public Warmup(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			var warmupService = scope.ServiceProvider.GetService<IWarmupService>();
			warmupService.MainCacheFillUp();
			warmupService.ImaCacheFillUp();
		}

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}