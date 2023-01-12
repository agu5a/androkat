using androkat.web.Endpoints.ContentDetailsModelEndpoints;
using Ardalis.HttpClientTestExtensions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace androkat.web.Tests.EndpointsTests;

[Collection("Sequential")]
public class CreateContentDetailsModelTests : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
	private readonly HttpClient _client;
    private const string _url = "/api/saveContentDetailsModel";

	public CreateContentDetailsModelTests(CustomWebApplicationFactory<WebMarker> factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task CreateContentDetailsModel_Happy()
	{
		var content = new StringContent(JsonSerializer.Serialize(new ContentDetailsModelRequest
		{
			ContentDetailsModel = new domain.Model.ContentDetailsModel
			{
				Tipus = 1,
				Cim = "cím2"
			}
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
			ContentDetailsModel = new domain.Model.ContentDetailsModel
			{
				Tipus = 1,
				Cim = "cím1"
			}
		}),
		Encoding.UTF8, "application/json");

        var result = await PostAndEnsureConflictAsync(_url, content);

		Assert.False(result.IsSuccessStatusCode);
	}

    private async Task<HttpResponseMessage> PostAndEnsureConflictAsync(string requestUri, HttpContent content)
    {
        HttpResponseMessage response = await _client.PostAsync(requestUri, content);
        if (response.StatusCode != HttpStatusCode.Conflict)
        {
            throw new HttpRequestException("");
        }

        return response;
    }
}