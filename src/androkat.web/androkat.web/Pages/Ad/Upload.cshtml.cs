using androkat.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace androkat.web.Pages.Ad;

//https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/mvc/models/file-uploads/samples/3.x/SampleApp/Pages/BufferedSingleFileUploadPhysical.cshtml.cs
//https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-6.0#upload-small-files-with-buffered-model-binding-to-physical-storage

//[Authorize()]
public class UploadModel : PageModel
{
    private readonly ILogger<UploadModel> _logger;
    private readonly string[] _permittedExtensions = [".m4a", ".jpeg", ".jpg", ".mp3", ".png"];

    public UploadModel(ILogger<UploadModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public BufferedSingleFileUploadPhysical FileUpload1 { get; set; }

    [BindProperty]
    public BufferedSingleFileUploadPhysical FileUpload2 { get; set; }

    public string Result { get; private set; }

    [BindProperty]
    public string FileNameReplace { get; set; }

    public IActionResult OnPostReplace()
    {
        if (string.IsNullOrWhiteSpace(FileNameReplace))
        {
            Result = "Please correct the FileNameReplace";
            return Page();
        }

        //yt5s.com - igazi fájl név (128 kbps).mp3
        Result = FileNameReplace.ToLower().Replace(" ", "_").Replace("ő", "o").Replace("ö", "o").Replace("ó", "o").Replace("ü", "u").Replace("ű", "u").Replace("ú", "u")
            .Replace("í", "i").Replace("é", "e").Replace("á", "a").Replace("yt5s.com_-_", "").Replace("_(128_kbps)", "");
        return Page();
    }

    public async Task<IActionResult> OnPostUploadImagesAsync()
    {
        try
        {
            if (FileUpload2.FormFile is null)
            {
                Result = "Please correct the form.";
                return Page();
            }

            var formFileContent = await ProcessFormFile<BufferedSingleFileUploadPhysical>(FileUpload2.FormFile, ModelState, _permittedExtensions);

            if (formFileContent.Length == 0)
            {
                Result = "hiba";
                return Page();
            }

            var folderName = Path.Combine("wwwroot", "images", "ajanlatok");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var filePath = Path.Combine(pathToSave, Path.GetFileName(FileUpload2.FormFile.FileName));

            using (var fileStream = System.IO.File.Create(filePath))
            {
                await fileStream.WriteAsync(formFileContent);
            }

            Result = $"size: {FileUpload2.FormFile.Length} filePath: {filePath}";
        }
        catch (Exception ex)
        {
            Result = ex.Message;
            _logger.LogError(ex, "");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostUploadAsync()
    {
        try
        {
            if (FileUpload1.FormFile is null)
            {
                Result = "Please correct the form.";
                return Page();
            }

            var formFileContent = await ProcessFormFile<BufferedSingleFileUploadPhysical>(FileUpload1.FormFile, ModelState, _permittedExtensions);

            if (formFileContent.Length == 0)
            {
                Result = "hiba";
                return Page();
            }

            var folderName = Path.Combine("wwwroot", "download");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var filePath = Path.Combine(pathToSave, Path.GetFileName(FileUpload1.FormFile.FileName));

            using (var fileStream = System.IO.File.Create(filePath))
            {
                await fileStream.WriteAsync(formFileContent);
            }

            Result = $"size: {FileUpload1.FormFile.Length} filePath: {filePath}";
        }
        catch (Exception ex)
        {
            Result = ex.Message;
            _logger.LogError(ex, "");
        }

        return Page();
    }

    private async Task<byte[]> ProcessFormFile<T>(IFormFile formFile, ModelStateDictionary modelState, string[] permittedExtensions)
    {
        var fieldDisplayName = string.Empty;

        // Use reflection to obtain the display name for the model
        // property associated with this IFormFile. If a display
        // name isn't found, error messages simply won't show
        // a display name.
        MemberInfo property = typeof(T).GetProperty(formFile.Name[(formFile.Name.IndexOf('.') + 1)..]);
        if (property is not null && property.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute displayAttribute)
            fieldDisplayName = $"{displayAttribute.Name} ";

        // Don't trust the file name sent by the client. To display the file name, HTML-encode the value.
        var trustedFileNameForDisplay = WebUtility.HtmlEncode(formFile.FileName);

        // Check the file length. This check doesn't catch files that only have a BOM as their content.
        if (formFile.Length == 0)
        {
            Result = $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.";
            modelState.AddModelError(formFile.Name, $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");
            return [];
        }

        try
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            // Check the content length in case the file's only
            // content was a BOM and the content is actually
            // empty after removing the BOM.
            if (memoryStream.Length == 0)
            {
                Result = $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.";
                modelState.AddModelError(formFile.Name, $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");
            }

            if (!IsValidFileExtensionAndSignature(formFile.FileName, memoryStream, permittedExtensions))
            {
                Result = $"{fieldDisplayName}({trustedFileNameForDisplay}) file " +
                    "type isn't permitted or the file's signature doesn't match the file's extension.";

                modelState.AddModelError(formFile.Name,
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) file " +
                    "type isn't permitted or the file's signature doesn't match the file's extension.");
            }
            else
                return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            Result = $"{fieldDisplayName}({trustedFileNameForDisplay}) upload failed. Error: {ex.HResult}";
            modelState.AddModelError(formFile.Name, $"{fieldDisplayName}({trustedFileNameForDisplay}) upload failed. Error: {ex.HResult}");
        }

        return [];
    }

    private static bool IsValidFileExtensionAndSignature(string fileName, Stream data, string[] permittedExtensions)
    {
        if (string.IsNullOrEmpty(fileName) || data is null || data.Length == 0)
            return false;

        var ext = Path.GetExtension(fileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            return false;

        return true;
    }
}