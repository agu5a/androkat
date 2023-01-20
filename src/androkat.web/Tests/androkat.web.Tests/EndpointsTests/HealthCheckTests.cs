using Ardalis.HttpClientTestExtensions;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Linq;
using androkat.web.ViewModels;

namespace androkat.web.Tests.EndpointsTests;

[Collection("Sequential")]
public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory<WebMarker>>
{
    private readonly HttpClient _client;
    private const string _url = "/fake_health";

    public HealthCheckTests(CustomWebApplicationFactory<WebMarker> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheckTest_Happy()
    {
        var result = await _client.GetAndEnsureSubstringAsync(_url, "{\"Status\":\"Healthy\",\"Checks\":[{\"Status\":\"Healthy\",\"Component\":\"AndrokatContext\",\"Description\":");
        Assert.NotNull(result);

        var response = JsonSerializer.Deserialize<HealthCheckResponse>(result);
        response.Checks.First().Status.Should().Be("Healthy");
    }
}