using System.Text.Json.Serialization;

namespace androkat.web.Endpoints.ContentDetailsModelEndpoints;

public class ContentDetailsModelResponse
{
    public ContentDetailsModelResponse(bool result)
    {
        Result = result;
    }

    [JsonPropertyName("result")]
    public bool Result { get; init; }
}