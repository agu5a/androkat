using androkat.application.Interfaces;
using androkat.domain;
using androkat.domain.Model.AdminPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace androkat.web.Pages.Ad;

[Authorize]
public class FilesModel : PageModel
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FilesModel> _logger;
    private readonly IAdminRepository _adminRepository;
    private readonly ICronService _cronService;

    public FilesModel(
        ILogger<FilesModel> logger,
        IWebHostEnvironment environment,
        IAdminRepository adminRepository,
        ICronService cronService)
    {
        _logger = logger;
        _environment = environment;
        _adminRepository = adminRepository;
        _cronService = cronService;
    }

    [BindProperty]
    public List<FileList> FileNames { get; set; } = [];

    [BindProperty]
    public List<FileList> AudioFileNames { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public string FileName { get; set; }

    [BindProperty]
    public string Deletable { get; set; }

    public ActionResult OnGet()
    {
        if (!string.IsNullOrWhiteSpace(FileName) && System.IO.File.Exists(FileName))
        {
            System.IO.File.Delete(FileName);
        }

        FillData();
        return Page();
    }

    public ActionResult OnPostGetDeletable()
    {
        try
        {
            Deletable = string.Join("<br>", _cronService.DeleteFiles(_environment.WebRootPath));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        FillData();
        return Page();
    }

    public ActionResult OnPostDelete()
    {
        try
        {
            _cronService.DeleteFiles(_environment.WebRootPath, true);
            Deletable = string.Join("<br>", _cronService.DeleteFiles(_environment.WebRootPath));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
        }

        FillData();
        return Page();
    }

    private void FillData()
    {
        try
        {
            var res = _adminRepository.GetImgList();
            var filePath = Path.Combine(_environment.WebRootPath, "images/ajanlatok");
            var files = Directory.GetFiles(filePath).OrderBy(o => o).ToList();

            if (files.Count != 0)
            {
                files.ForEach(f =>
                {
                    var fileName = f.Replace(filePath, string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty);
                    var imgData = res.FirstOrDefault(i => i.Img == fileName);
                    FileNames.Add(new FileList
                    {
                        FullFileName = f,
                        FileName = fileName,
                        Cim = imgData?.Cim,
                        FileDate = System.IO.File.GetLastWriteTime(f).ToString("yyyy-MM-dd HH:mm:ss"),
                        Tipus = imgData?.Tipus ?? string.Empty
                    });
                });
            }

            var filePaths = _adminRepository.GetAudioList();
            filePath = Path.Combine(_environment.WebRootPath, "download");
            files = [.. Directory.GetFiles(filePath)];

            if (files.Count != 0)
            {
                var audioFileNames = new List<FileList>();
                files.ForEach(f =>
                {
                    var fileName = f.Replace(filePath, string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty);
                    var fileData = filePaths.FirstOrDefault(d => d.Path.Contains(fileName));
                    audioFileNames.Add(new FileList
                    {
                        FullFileName = f,
                        FileName = fileName,
                        Cim = fileData?.Cim,
                        FileDate = System.IO.File.GetLastWriteTime(f).ToString("yyyy-MM-dd HH:mm:ss"),
                        Tipus = fileData?.Tipus ?? string.Empty
                    });
                });

                AudioFileNames.AddRange(audioFileNames.OrderBy(o => o.FileDate));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
        }
    }
}