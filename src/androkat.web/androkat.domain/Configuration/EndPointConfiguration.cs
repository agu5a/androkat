using System.ComponentModel.DataAnnotations;

namespace androkat.domain.Configuration;

public class EndPointConfiguration
{
    [Required]
    public string SaveContentDetailsModelApiUrl { get; set; }
    [Required]
    public string SaveTempContentApiUrl { get; set; }
    [Required]
    public string UpdateRadioMusorModelApiUrl { get; set; }
    [Required]
    public string HealthCheckApiUrl { get; set; }
}