using androkat.domain.Model;
using System.ComponentModel.DataAnnotations;

namespace androkat.web.Endpoints.ContentDetailsModelEndpoints;

public class ContentDetailsModelRequest
{
	[Required]
	public ContentDetailsModel ContentDetailsModel { get; set; }
}