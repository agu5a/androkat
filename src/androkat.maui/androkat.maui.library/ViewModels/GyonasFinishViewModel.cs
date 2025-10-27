using androkat.maui.library.Abstraction;
using androkat.maui.library.Data;
using androkat.maui.library.Helpers;
using androkat.maui.library.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Text;

namespace androkat.maui.library.ViewModels;

public partial class GyonasFinishViewModel : ViewModelBase
{
    private readonly IResourceData _resourceData;
    private readonly IRepository _repository;
    private readonly IPageService _pageService;
    private readonly IHelperSharedPreferences _helperSharedPreferences;

    public GyonasFinishViewModel(IResourceData resourceData, IRepository repository, IPageService pageService, IHelperSharedPreferences helperSharedPreferences)
    {
        _resourceData = resourceData;
        _repository = repository;
        _pageService = pageService;
        _helperSharedPreferences = helperSharedPreferences;
    }

    [ObservableProperty]
    string gyonasSzoveg;

    [ObservableProperty]
    bool isChecked = true;

    public async Task InitializeAsync()
    {
        // Load saved checkbox state
        IsChecked = _helperSharedPreferences.GetSharedPreferencesBoolean("gyonasTeljes", true);

        await FetchAsync();
    }

    partial void OnIsCheckedChanged(bool value)
    {
        // Save checkbox state when changed
        _helperSharedPreferences.PutSharedPreferencesBoolean("gyonasTeljes", value);

        // Refresh content when checkbox changes
        _ = FetchAsync();
    }

    async Task FetchAsync()
    {
        try
        {
            // Load the base HTML template
            string baseHtml;
            if (IsChecked)
            {
                baseHtml = await _resourceData.GetResourceAsString("gyonas.html");
            }
            else
            {
                baseHtml = await _resourceData.GetResourceAsString("gyonasrovid.html");
            }

            // Generate the selected confessions and notes content
            string bunokHtml = await GenerateBunokHtml();

            // Replace the placeholder with the actual content
            var htmlWithBunok = baseHtml.Replace("{{bunok}}", bunokHtml);

            // Apply font scaling
            GyonasSzoveg = HtmlHelper.WrapHtmlWithFontScale(htmlWithBunok);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in GyonasFinishViewModel.FetchAsync: {ex.Message}");
            // Fallback to basic template
            if (IsChecked)
            {
                GyonasSzoveg = await _resourceData.GetResourceAsString("gyonas.html");
            }
            else
            {
                GyonasSzoveg = await _resourceData.GetResourceAsString("gyonasrovid.html");
            }
            var htmlWithBunok = GyonasSzoveg.Replace("{{bunok}}", "");
            GyonasSzoveg = HtmlHelper.WrapHtmlWithFontScale(htmlWithBunok);
        }
    }

    private async Task<string> GenerateBunokHtml()
    {
        var sb = new StringBuilder();

        try
        {
            // Load selected confessions from database
            var selectedBunok = await _repository.GetBunok();

            if (selectedBunok?.Any() == true)
            {
                // Load questions from CSV to get the text for each selected confession
                var allQuestions = await LoadQuestionsFromCsv();

                foreach (var selectedBun in selectedBunok)
                {
                    var question = allQuestions.FirstOrDefault(q =>
                        q.BunId == selectedBun.BunId && q.ParancsId == selectedBun.ParancsId);

                    if (question != null)
                    {
                        sb.Append($"<li><b>{question.ParancsId}. parancs</b>: {question.Text}</li>");
                    }
                }
            }

            // Load notes from database
            var jegyzet = await _pageService.GetGyonasiJegyzet();
            if (jegyzet != null && !string.IsNullOrWhiteSpace(jegyzet.Jegyzet))
            {
                sb.Append($"<li>{jegyzet.Jegyzet}</li>");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error generating bunok HTML: {ex.Message}");
        }

        return sb.ToString();
    }

    private async Task<List<GyonasQuestion>> LoadQuestionsFromCsv()
    {
        var questions = new List<GyonasQuestion>();

        try
        {
            var csvContent = await _resourceData.GetResourceAsString("gyonas.csv");
            var lines = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length >= 10 && // Ensure we have enough columns
                    int.TryParse(parts[0], out int parancsId) &&
                    int.TryParse(parts[1], out int bunId))
                {
                    questions.Add(new GyonasQuestion
                    {
                        ParancsId = parancsId,
                        BunId = bunId,
                        Text = parts[9].Trim('"') // Remove quotes if present
                    });
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading questions from CSV: {ex.Message}");
        }

        return questions;
    }
}

// Helper class for CSV questions
internal sealed class GyonasQuestion
{
    public int ParancsId { get; set; }
    public int BunId { get; set; }
    public string Text { get; set; } = string.Empty;
}