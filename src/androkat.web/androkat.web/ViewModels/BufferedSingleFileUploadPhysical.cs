using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace androkat.web.ViewModels;

public class BufferedSingleFileUploadPhysical
{
    [Required]
    [Display(Name = "File")]
    public IFormFile FormFile { get; set; }
}