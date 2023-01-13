using System.Text.Json.Serialization;

namespace androkat.web.Endpoints.RadioMusorModelEndpoints;

public class RadioMusorModelResponse
{
	public RadioMusorModelResponse(bool result)
	{
		Result = result;
	}

    [JsonPropertyName("result")]
	public bool Result { get; init; }
}