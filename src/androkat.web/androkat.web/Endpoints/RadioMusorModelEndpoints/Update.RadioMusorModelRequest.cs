using System.ComponentModel.DataAnnotations;

namespace androkat.web.Endpoints.RadioMusorModelEndpoints;

public class RadioMusorModelRequest
{
	[Required]
	public string Source { get; set; }
	[Required]
	public string Musor { get; set; }
	[Required]
	public string Inserted { get; set; }
}