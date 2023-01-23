using androkat.domain.Model;
using System.ComponentModel.DataAnnotations;

namespace androkat.web.Endpoints.TempContentEndpoints;

public class ContentDetailsModelRequest
{
	[Required]
	public ContentDetailsModel ContentDetailsModel { get; set; }
}