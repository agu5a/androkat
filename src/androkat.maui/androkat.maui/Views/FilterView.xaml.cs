using CommunityToolkit.Maui.Views;

namespace androkat.hu.Views;

public partial class FilterView : Popup
{
    public event EventHandler<bool>? FilterChanged;

    public FilterView(bool showVisited)
    {
        InitializeComponent();
        ShowVisitedCheckbox.IsChecked = showVisited;
    }

    private void OnApplyClicked(object sender, EventArgs e)
    {
        FilterChanged?.Invoke(this, ShowVisitedCheckbox.IsChecked);
        Close();
    }
}
