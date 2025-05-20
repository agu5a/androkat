using androkat.web.ViewModels;
using Ardalis.HttpClientTestExtensions;
using FluentAssertions;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace androkat.web.Tests.EndpointsTests;

[Collection("Sequential")]
public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory<IWebMarker>>
{
    private readonly HttpClient _client;
    private readonly string _url = TestEnvironmentHelper.GetHealthCheckTestsUrl();

    public HealthCheckTests(CustomWebApplicationFactory<IWebMarker> factory)
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