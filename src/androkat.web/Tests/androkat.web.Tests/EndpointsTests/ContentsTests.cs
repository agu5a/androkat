using androkat.domain.Enum;
using androkat.domain.Model.WebResponse;
using Ardalis.HttpClientTestExtensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace androkat.web.Tests.EndpointsTests;

[Collection("Sequential")]
public class ContentsTests : IClassFixture<ContentsWebApplicationFactory<IWebMarker>>
{
    private readonly HttpClient _client;
    private const string _url = Constants.GetContentsApiTestsUrl;

    public ContentsTests(ContentsWebApplicationFactory<IWebMarker> factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData(1, "x281cd115128911ea8aa1cbeb38570c35")] //wrong guid, plus char
    [InlineData(1, "81cd115-1289-11ea-8aa1-cbeb38570c35")] //wrong guid, less 1 char
    [InlineData((int)Forras.book, "x281cd115128911ea8aa1cbeb38570c35")] //wrong guid, plus char
    [InlineData((int)Forras.b777, "81cd115-1289-11ea-8aa1-cbeb38570c35")] //wrong guid, less 1 char
    public async Task API_GetContentsByTipusAndId_BadRequest(int tipus, string id)
    {
        try
        {
            var result = await _client.GetAndDeserializeAsync<IEnumerable<ContentResponse>>($"{_url}?tipus={tipus}&id={id}");
        }
        catch (Exception ex)
        {
            Assert.True(((HttpRequestException)ex).StatusCode == HttpStatusCode.BadRequest);
        }
    }

    [Theory]
    [InlineData((int)Forras.book, "")]
    [InlineData((int)Forras.b777, "")]
    [InlineData((int)Forras.book, "281cd115-1289-11ea-8aa1-cbeb38570c35")]
    [InlineData((int)Forras.b777, "381cd115-1289-11ea-8aa1-cbeb38570c35")]
    public async Task API_GetEgyebOlvasnivaloByForrasAndNid_Book(int tipus, string id)
    {
        try
        {
            var s = await _client.GetAndDeserializeAsync<IEnumerable<ContentResponse>>($"{_url}?tipus={tipus}&id={id}");

            if (string.IsNullOrWhiteSpace(id))
            {
                s.First().Cim.Should().Be("cím1");
                s.First().KulsoLink.Should().BeEmpty();
                s.ElementAt(1).Cim.Should().Be("cím2");
                s.Count().Should().Be(2);
            }
            else
            {
                s.Count().Should().Be(0);
            }
        }
        catch (Exception ex)
        {

            Assert.True(((System.Net.Http.HttpRequestException)ex).StatusCode == HttpStatusCode.BadRequest);
        }
    }

    [Fact]
    public async Task API_GetEgyebOlvasnivaloByForrasAndNid_Kurir_V3_Zero()
    {
        var s = await _client.GetAndDeserializeAsync<IEnumerable<ContentResponse>>($"{_url}?tipus={(int)Forras.kurir}&id=");
        s.Count().Should().Be(0);
    }

    [Fact]
    public async Task API_GetEgyebOlvasnivaloByForrasAndNid_Audionapievangelium_V3_Zero()
    {
        var s = await _client.GetAndDeserializeAsync<IEnumerable<ContentResponse>>($"{_url}?tipus={(int)Forras.audionapievangelium}&id=");
        s.Count().Should().Be(0);
    }
}