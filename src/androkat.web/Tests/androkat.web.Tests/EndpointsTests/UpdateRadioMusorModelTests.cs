using androkat.web.Endpoints.RadioMusorModelEndpoints;
using Ardalis.HttpClientTestExtensions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace androkat.web.Tests.EndpointsTests;

[Collection("Sequential")]
public class UpdateRadioMusorModelTests : IClassFixture<CustomWebApplicationFactory<IWebMarker>>
{
    private readonly HttpClient _client;
    private readonly string _url = TestEnvironmentHelper.GetUpdateRadioMusorModelTestsUrl();

    public UpdateRadioMusorModelTests(CustomWebApplicationFactory<IWebMarker> factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-API-Key", TestEnvironmentHelper.GetXAPIKey());
    }

    [Fact]
    public async Task UpdateRadioMusorModel_Happy()
    {
        var content = new StringContent(JsonSerializer.Serialize(new RadioMusorModelRequest
        {
            Source = "Source1",
            Musor = "Műsor2",
            Inserted = "2022-12-12"
        }),
        Encoding.UTF8, "application/json");

        var result = await _client.PostAndDeserializeAsync<RadioMusorModelResponse>(_url, content);

        Assert.True(result.Result);
    }

    [Fact]
    public async Task UpdateRadioMusorModel_NotFound()
    {
        var content = new StringContent(JsonSerializer.Serialize(new RadioMusorModelRequest
        {
            Source = "Source2",
            Musor = "Műsor2",
            Inserted = "2022-12-12"
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