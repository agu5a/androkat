using androkat.web.Endpoints.TempContentEndpoints;
using Ardalis.HttpClientTestExtensions;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace androkat.web.Tests.EndpointsTests;

[Collection("Sequential")]
public class CreateTempContentTests : IClassFixture<CustomWebApplicationFactory<IWebMarker>>
{
    private readonly HttpClient _client;
    private readonly string _url = TestEnvironmentHelper.GetCreateTempContentTestsUrl();

    public CreateTempContentTests(CustomWebApplicationFactory<IWebMarker> factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-API-Key", TestEnvironmentHelper.GetXAPIKey());
    }

    [Fact]
    public async Task CreateContentDetailsModel_Happy()
    {
        var content = new StringContent(JsonSerializer.Serialize(new ContentDetailsModelRequest
        {
            ContentDetailsModel = new domain.Model.ContentDetailsModel(Guid.Empty, DateTime.MinValue, "cím2", string.Empty, 1, DateTime.MinValue, string.Empty, "Image", string.Empty, string.Empty)
        }),
        Encoding.UTF8, "application/json");

        var result = await _client.PostAndDeserializeAsync<ContentDetailsModelResponse>(_url, content);

        Assert.True(result.Result);
    }

    [Fact]
    public async Task CreateContentDetailsModel_Conflict()
    {
        var content = new StringContent(JsonSerializer.Serialize(new ContentDetailsModelRequest
        {
            ContentDetailsModel = new domain.Model.ContentDetailsModel(Guid.Empty, DateTime.MinValue, "cím1", string.Empty, 1, DateTime.MinValue, string.Empty, "Image", string.Empty, string.Empty)
        }),
        Encoding.UTF8, "application/json");

        var result = await PostAndEnsureConflictAsync(_url, content);

        Assert.False(result.IsSuccessStatusCode);
    }

    private async Task<HttpResponseMessage> PostAndEnsureConflictAsync(string requestUri, HttpContent content)
    {
        var response = await _client.PostAsync(requestUri, content);
        if (response.StatusCode != HttpStatusCode.Conflict)
        {
            throw new HttpRequestException("");
        }

        return response;
    }
}