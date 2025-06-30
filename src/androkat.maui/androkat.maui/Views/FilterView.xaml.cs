using CommunityToolkit.Maui.Views;
using androkat.maui.library.Models;
using androkat.maui.library.Helpers;

namespace androkat.hu.Views;

public partial class FilterView : Popup
{
    public event EventHandler<FilterChangedEventArgs>? FilterChanged;
    private readonly List<FilterOption> _filterOptions;
    private readonly Dictionary<string, CheckBox> _sourceCheckBoxes = new();

    public FilterView(bool showVisited, string pageId)
    {
        InitializeComponent();
        // Invert the logic: if showVisited is true, hideVisited should be false
        ShowVisitedCheckbox.IsChecked = !showVisited;

        _filterOptions = FilterOptionsHelper.GetFilterOptionsForPageId(pageId);
        CreateSourceFilters();
    }
    private void CreateSourceFilters()
    {
        if (_filterOptions.Count == 0)
        {
            // Hide the sources section if no filters available
            SourceFiltersContainer.IsVisible = false;
            return;
        }

        foreach (var option in _filterOptions)
        {
            var horizontalStack = new HorizontalStackLayout
            {
                Spacing = 10
            };

            var checkBox = new CheckBox
            {
                IsChecked = Settings.IsSourceEnabled(option.Key),
                VerticalOptions = LayoutOptions.Start
            };

            var label = new Label
            {
                Text = option.DisplayName,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 14,
                LineBreakMode = LineBreakMode.WordWrap,
                MaxLines = 2
            };

            // Use a flexible layout to allow text to wrap
            horizontalStack.Children.Add(checkBox);
            horizontalStack.Children.Add(label);

            SourceFiltersContainer.Children.Add(horizontalStack);
            _sourceCheckBoxes[option.Key] = checkBox;
        }
    }

    private void OnApplyClicked(object sender, EventArgs e)
    {
        // Save source filter preferences
        foreach (var kvp in _sourceCheckBoxes)
        {
            Settings.SetSourceEnabled(kvp.Key, kvp.Value.IsChecked);
        }

        var enabledSources = _sourceCheckBoxes
            .Where(kvp => kvp.Value.IsChecked)
            .Select(kvp => kvp.Key)
            .ToList();

        var args = new FilterChangedEventArgs
        {
            // Invert the logic: if hideVisited is checked, showVisited should be false
            ShowVisited = !ShowVisitedCheckbox.IsChecked,
            // Only pass enabled sources if there are filterable options available
            EnabledSources = _filterOptions.Count > 0 ? enabledSources : null
        };

        FilterChanged?.Invoke(this, args);
        CloseAsync();
    }
}

public class FilterChangedEventArgs : EventArgs
{
    public bool ShowVisited { get; set; }
    public List<string>? EnabledSources { get; set; } = new();
}
