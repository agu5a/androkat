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

[Collection("SharedWebAppCollection")]
public class ContentsTests : IClassFixture<ContentsWebApplicationFactory<IWebMarker>>
{
    private readonly HttpClient _client;
    private readonly string _url = TestEnvironmentHelper.GetContentsApiTestsUrl();

    public ContentsTests(ContentsWebApplicationFactory<IWebMarker> factory)
    {
        _client = factory.CreateClient();
    }

    // ETag tests - run first to avoid cache pollution
    [Fact]
    public async Task Test01_API_GetContents_Returns_ETag_Header()
    {
        // Act
        var response = await _client.GetAsync($"{_url}?tipus={(int)Forras.book}&id=");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Should().Contain(h => h.Key == "ETag");
        var etagHeader = response.Headers.GetValues("ETag").FirstOrDefault();
        etagHeader.Should().NotBeNullOrEmpty();
        etagHeader.Should().StartWith("\"");
        etagHeader.Should().EndWith("\"");
    }

    [Fact]
    public async Task Test02_API_GetContents_Returns_304_When_ETag_Matches()
    {
        // Arrange - First request to get the ETag
        var firstResponse = await _client.GetAsync($"{_url}?tipus={(int)Forras.book}&id=");
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var etag = firstResponse.Headers.GetValues("ETag").FirstOrDefault();
        etag.Should().NotBeNullOrEmpty();

        // Act - Second request with If-None-Match header
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}?tipus={(int)Forras.book}&id=");
        request.Headers.TryAddWithoutValidation("If-None-Match", etag);
        var secondResponse = await _client.SendAsync(request);

        // Assert
        secondResponse.StatusCode.Should().Be(HttpStatusCode.NotModified);
        secondResponse.Headers.Should().Contain(h => h.Key == "ETag");
        secondResponse.Headers.GetValues("ETag").FirstOrDefault().Should().Be(etag);
        var content = await secondResponse.Content.ReadAsStringAsync();
        content.Should().BeEmpty(); // 304 responses should have no body
    }

    [Fact]
    public async Task Test03_API_GetContents_Returns_200_When_ETag_Does_Not_Match()
    {
        // Arrange
        var wrongEtag = "\"wrong-etag-value-12345\"";

        // Act
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}?tipus={(int)Forras.book}&id=");
        request.Headers.TryAddWithoutValidation("If-None-Match", wrongEtag);
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Should().Contain(h => h.Key == "ETag");
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeEmpty(); // 200 responses should have body
    }

    [Fact]
    public async Task Test04_API_GetContents_ETag_Is_Consistent_For_Same_Content()
    {
        // Act - Make two requests without If-None-Match
        var firstResponse = await _client.GetAsync($"{_url}?tipus={(int)Forras.book}&id=");
        var firstEtag = firstResponse.Headers.GetValues("ETag").FirstOrDefault();

        var secondResponse = await _client.GetAsync($"{_url}?tipus={(int)Forras.book}&id=");
        var secondEtag = secondResponse.Headers.GetValues("ETag").FirstOrDefault();

        // Assert - ETags should be identical for the same content
        firstEtag.Should().Be(secondEtag);
    }

    [Fact]
    public async Task Test05_API_GetContents_Returns_Cache_Control_Header()
    {
        // Act
        var response = await _client.GetAsync($"{_url}?tipus={(int)Forras.book}&id=");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Should().Contain(h => h.Key == "Cache-Control");
        var cacheControl = response.Headers.GetValues("Cache-Control").FirstOrDefault();
        cacheControl.Should().Contain("private");
        cacheControl.Should().Contain("max-age=300");
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