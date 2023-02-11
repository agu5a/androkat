using androkat.domain.Model;
using FastEndpoints;
using System.ComponentModel.DataAnnotations;

namespace androkat.web.Endpoints.ContentDetailsModelEndpoints;

public class ContentDetailsModelRequest
{
	[Required]
	public ContentDetailsModel ContentDetailsModel { get; set; }

    [FromHeader("X-API-Key")]
    [Required]
    public string ApiKey { get; set; }
}