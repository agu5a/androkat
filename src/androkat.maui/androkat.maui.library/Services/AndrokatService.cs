using androkat.maui.library.Abstraction;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models;
using System.Text.Json;

namespace androkat.maui.library.Services;

public class AndrokatService : IAndrokatService
{

    HttpClient client;

    public async Task<ImaResponse> GetImadsag(DateTime date)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return new ImaResponse();

        HttpClient client = GetClient();

        var response = await client.GetStringAsync($"v2/ima?date={date.ToString("yyyy-MM-dd")}");

        JsonSerializerOptions options = new JsonSerializerOptions();
        options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
        return JsonSerializer.Deserialize<ImaResponse>(response, options);
    }

    public async Task<List<ContentResponse>> GetContents(string f, string n)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return new List<ContentResponse>();

        HttpClient client = GetClient();

        var response = await client.GetStringAsync($"v3/contents?tipus={f}&id={n}");

        JsonSerializerOptions options = new JsonSerializerOptions();
        options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
        return JsonSerializer.Deserialize<List<ContentResponse>>(response, options);
    }

    private HttpClient GetClient()
    {
        if (client != null)
            return client;

        client = new HttpClient { BaseAddress = new Uri("http://api.androkat.hu/") };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        return client;
    }
}