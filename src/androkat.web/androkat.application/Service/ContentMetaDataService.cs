using androkat.application.Interfaces;
using androkat.domain.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace androkat.application.Service;

public class ContentMetaDataService : IContentMetaDataService
{
    private readonly ILogger<ContentMetaDataService> _logger;

    public ContentMetaDataService(ILogger<ContentMetaDataService> logger)
    {
        _logger = logger;
    }

    public IEnumerable<ContentMetaDataModel> GetContentMetaDataList(string path = "./Data/IdezetData.json")
    {
        using var tr = new StreamReader(path);
        var json = tr.ReadToEnd();
        var list = JsonSerializer.Deserialize<List<ContentMetaDataModel>>(json);

        _logger.LogInformation("{Name} GetContentMetaDataList has been finished. Count: {count}", nameof(ContentMetaDataService), list.Count);

        return [.. list.OrderBy(o => o.TipusNev)];
    }
}