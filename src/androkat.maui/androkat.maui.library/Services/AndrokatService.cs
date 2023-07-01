using androkat.maui.library.Abstraction;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Responses;
using System.Text.Json;

namespace androkat.maui.library.Services;

public class AndrokatService : IAndrokatService
{
    HttpClient client;

    public async Task<List<ServerInfoResponse>> GetServerInfo()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return new List<ServerInfoResponse>();

        client = GetClient();

        var response = await client.GetStringAsync("v2/ser");

        var options = new JsonSerializerOptions();
        options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
        var result = JsonSerializer.Deserialize<List<ServerInfoResponse>>(response, options);

        foreach (var item in result)
        {
            switch (item.Key)
            {
                case ConsValues.VERZIO:
                    Preferences.Set("newversion", int.Parse(item.Value));
                    break;
                case "website":
                case "radio":
                case "szentgellertkiado":
                    Preferences.Set(item.Key, item.Value);
                    break;
            }
        }

        Settings.LastUpdate = DateTime.Now;
        return result;
    }

    public async Task<ImaResponse> GetImadsag(DateTime date)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return new ImaResponse();

        client = GetClient();

        var response = await client.GetStringAsync($"v2/ima?date={date.ToString("yyyy-MM-dd")}");

        var options = new JsonSerializerOptions();
        options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
        return JsonSerializer.Deserialize<ImaResponse>(response, options);
    }

    public async Task<List<ContentResponse>> GetContents(string tipus, string nid)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return new List<ContentResponse>();

        client = GetClient();

        var response = await client.GetStringAsync($"v3/contents?tipus={tipus}&id={nid}");

        var options = new JsonSerializerOptions();
        options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
        return JsonSerializer.Deserialize<List<ContentResponse>>(response, options);
    }

    private HttpClient GetClient()
    {
        if (client != null)
            return client;

        client = new HttpClient { BaseAddress = new Uri(ConsValues.ApiUrl) };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        return client;
    }
}