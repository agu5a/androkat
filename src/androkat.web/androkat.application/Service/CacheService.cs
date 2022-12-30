using androkat.application.Interfaces;
using androkat.domain;
using Microsoft.Extensions.Logging;

namespace androkat.application.Service;

public class CacheService : ICacheService
{
    private readonly IContentRepository _contentRepository;
    private readonly ILogger<CacheService> _logger;
    protected readonly IClock _clock;

    public CacheService(IContentRepository contentRepository, ILogger<CacheService> logger, IClock clock)
    {
        _contentRepository = contentRepository;
        _logger = logger;
        _clock = clock;
    }
}