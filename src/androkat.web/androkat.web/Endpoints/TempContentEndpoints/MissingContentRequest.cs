using FastEndpoints;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace androkat.web.Endpoints.TempContentEndpoints;

public class MissingContentRequest
{
    [FromHeader("X-API-Key")]
    [Required]
    public string ApiKey { get; set; }
}
