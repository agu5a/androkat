namespace androkat.hu.Views;

public partial class VideoItemView
{
    public static readonly BindableProperty IsLoadingProperty =
        BindableProperty.Create(
            nameof(IsLoading),
            typeof(bool),
            typeof(VideoItemView),
            true);

    public bool IsLoading
    {
        get { return (bool)GetValue(IsLoadingProperty); }
        set { SetValue(IsLoadingProperty, value); }
    }

    public VideoItemView()
    {
        InitializeComponent();
    }

    private void Image_Loaded(object sender, EventArgs e)
    {
        Task.Run(async () =>
        {
            await Task.Delay(2000);
            IsLoading = false;
        });
    }

#pragma warning disable S1185 // Overriding members should do more than simply call the same member in the base class
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        //if (BindingContext is not VideoItemViewModel viewModel)

        //viewModel .InitializeCommand .Execute(null)
    }
#pragma warning restore S1185 // Overriding members should do more than simply call the same member in the base class
}