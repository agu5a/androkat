using System.ComponentModel.DataAnnotations;

namespace androkat.domain.Configuration;

public class CredentialConfiguration
{
    [Required]
    public string GoogleAnalytics { get; set; }  

    [Required]
    public string CronApiKey { get; set; }
}