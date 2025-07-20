using System.Text.Json.Serialization;

namespace androkat.web.Endpoints.TempContentEndpoints;

public class MissingContentResponse
{
    public MissingContentResponse(string result)
    {
        Result = result;
    }

    [JsonPropertyName("result")]
    public string Result { get; set; }
}
