using androkat.domain.Enum;
using androkat.domain.Model.WebResponse;
using Ardalis.HttpClientTestExtensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace androkat.web.Tests.EndpointsTests;

[Collection("Sequential")]
public class ContentsTodayTests : IClassFixture<ContentsTodayWebApplicationFactory<IWebMarker>>
{
    private readonly HttpClient _client;
    private readonly string _url = TestEnvironmentHelper.GetContentsApiTestsUrl();

    public ContentsTodayTests(ContentsTodayWebApplicationFactory<IWebMarker> factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData((int)Forras.ajanlatweb, "")]
    [InlineData((int)Forras.humor, "")]
    [InlineData((int)Forras.maiszent, "")]
    [InlineData((int)Forras.pio, "")]
    [InlineData((int)Forras.ajanlatweb, "a81cd115-1289-11ea-8aa1-cbeb38570c35")]
    [InlineData((int)Forras.humor, "b81cd115-1289-11ea-8aa1-cbeb38570c35")]
    [InlineData((int)Forras.pio, "c81cd115-1289-11ea-8aa1-cbeb38570c35")]
    [InlineData((int)Forras.audiotaize, "")]
    public async Task API_GetContentByTipusAndNidV3(int tipus, string id)
    {
        var s = await _client.GetAndDeserializeAsync<IEnumerable<ContentResponse>>($"{_url}?tipus={tipus}&id={id}");

        if (string.IsNullOrWhiteSpace(id))
        {
            s.First().Cim.Should().Be("cím1");
            s.Count().Should().Be(tipus == (int)Forras.audiotaize ? 2 : 1);
        }
        else
        {
            s.Count().Should().Be(0);
        }

        if (tipus == (int)Forras.audiotaize)
        {
            s.First().Idezet.Should().Be("audiofile");
            s.ElementAt(1).Idezet.Should().Be("audiofile");
        }
    }

    [Theory]
    [InlineData(28, "dd1cd115-1289-11ea-8aa1-cbeb38570c35")]
    [InlineData(28, "")]
    public async Task API_GetContentsByTipusAndId(int tipus, string id)
    {
        var s = await _client.GetAndDeserializeAsync<IEnumerable<ContentResponse>>($"{_url}?tipus={tipus}&id={id}");
        if (string.IsNullOrWhiteSpace(id))
        {
            s.First().Cim.Should().Be("cím1");
            s.First().Idezet.Should().Be("audiofile");
            s.Count().Should().Be(2);
        }
        else
        {
            s.Count().Should().Be(0);
        }
    }
}