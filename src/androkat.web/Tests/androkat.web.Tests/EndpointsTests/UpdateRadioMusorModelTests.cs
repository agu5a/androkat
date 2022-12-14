using androkat.web.Endpoints.ContentDetailsModelEndpoints;
using androkat.web.Endpoints.RadioMusorModelEndpoints;
using Ardalis.HttpClientTestExtensions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace androkat.web.Tests.EndpointsTests;

[Collection("Sequential")]
public class UpdateRadioMusorModelTests : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
	private readonly HttpClient _client;

	public UpdateRadioMusorModelTests(CustomWebApplicationFactory<WebMarker> factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task UpdateRadioMusorModel_Happy()
	{
		var content = new StringContent(JsonSerializer.Serialize(new RadioMusorModelRequest
		{
			Source = "Source1",
			Musor = "Műsor2"
		}),
		Encoding.UTF8, "application/json");

		var result = await _client.PostAndDeserializeAsync<RadioMusorModelResponse>("/api/updateRadioMusorModel", content);

		Assert.True(result.Result);
	}

	[Fact]
	public async Task UpdateRadioMusorModel_NotFound()
	{
		var content = new StringContent(JsonSerializer.Serialize(new RadioMusorModelRequest
		{
			Source = "Source2",
			Musor = "Műsor2"
		}),
		Encoding.UTF8, "application/json");

		var result = await _client.PostAndEnsureBadRequestAsync("/api/updateRadioMusorModel", content);

		Assert.False(result.IsSuccessStatusCode);
	}
}