using androkat.maui.library.ViewModels;
using System.Windows.Input;

namespace androkat.hu.Views;

public partial class ContentItemView
{
    public static readonly BindableProperty SubscriptionCommandProperty =
        BindableProperty.Create(
            nameof(SubscriptionCommand),
            typeof(ICommand),
            typeof(ContentItemView),
            default(string));

    public static readonly BindableProperty SubscriptionCommandParameterProperty =
        BindableProperty.Create(
            nameof(SubscriptionCommandParameter),
            typeof(ContentItemViewModel),
            typeof(ContentItemView),
            default(ContentItemViewModel));

    public static readonly BindableProperty IsLoadingProperty =
        BindableProperty.Create(
            nameof(IsLoading),
            typeof(bool),
            typeof(ContentItemView),
            true);

    public ICommand SubscriptionCommand
    {
        get { return (ICommand)GetValue(SubscriptionCommandProperty); }
        set { SetValue(SubscriptionCommandProperty, value); }
    }

    public ContentItemViewModel SubscriptionCommandParameter
    {
        get { return (ContentItemViewModel)GetValue(SubscriptionCommandParameterProperty); }
        set { SetValue(SubscriptionCommandParameterProperty, value); }
    }

    public bool IsLoading
    {
        get { return (bool)GetValue(IsLoadingProperty); }
        set { SetValue(IsLoadingProperty, value); }
    }

    public ContentItemView()
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

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

#pragma warning disable S1481 // Unused local variables should be removed
#pragma warning disable IDE0059 // Unnecessary assignment of a value
        if (BindingContext is not ContentItemViewModel viewModel)
        {
            return;
        }
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning restore S1481 // Unused local variables should be removed

        //viewModel.InitializeCommand.Execute(null)
    }
}