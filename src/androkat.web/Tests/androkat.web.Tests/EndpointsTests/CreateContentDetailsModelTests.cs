using androkat.web.Endpoints.ContentDetailsModelEndpoints;
using Ardalis.HttpClientTestExtensions;
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

		var result = await _client.PostAndDeserializeAsync<ContentDetailsModelResponse>("/api/saveContentDetailsModel", content);

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

		var result = await _client.PostAndEnsureBadRequestAsync("/api/saveContentDetailsModel", content);

		Assert.False(result.IsSuccessStatusCode);
	}
}