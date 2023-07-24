using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace androkat.domain.Model.WebResponse;

public class ImaResponse
{
    [JsonPropertyName("hasMore")]
    public bool HasMore { get; set; }

    [JsonPropertyName("imak")]
    public List<ImaDetailsResponse> Imak { get; set; }
}
