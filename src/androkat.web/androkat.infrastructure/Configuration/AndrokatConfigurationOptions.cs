using androkat.application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;

namespace androkat.infrastructure.Configuration;

public class AndrokatConfigurationOptions : IConfigureOptions<AndrokatConfiguration>
{
    private readonly IContentMetaDataService _contentMetaDataService;
    private readonly ILogger<AndrokatConfigurationOptions> _logger;

    public AndrokatConfigurationOptions(IContentMetaDataService contentMetaDataService, ILogger<AndrokatConfigurationOptions> logger)
    {
        _contentMetaDataService = contentMetaDataService;
        _logger = logger;
    }

    public void Configure(AndrokatConfiguration options)
    {
        options.ContentMetaDataList = _contentMetaDataService.GetContentMetaDataList();
        _logger.LogInformation("{Name} Configure finished. Count: {count}", nameof(AndrokatConfigurationOptions), options.ContentMetaDataList.ToList().Count);
    }
}