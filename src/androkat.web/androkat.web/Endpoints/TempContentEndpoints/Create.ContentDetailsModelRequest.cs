using androkat.domain.Model;
using FastEndpoints;
using System.ComponentModel.DataAnnotations;

namespace androkat.web.Endpoints.TempContentEndpoints;

public class ContentDetailsModelRequest
{
	[Required]
	public ContentDetailsModel ContentDetailsModel { get; set; }
    [FromHeader("X-API-Key")]
    [Required]
    public string ApiKeyHeaderName { get; set; }
}