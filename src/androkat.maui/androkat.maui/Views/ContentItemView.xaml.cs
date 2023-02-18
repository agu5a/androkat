using androkat.hu.ViewModels;
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
            typeof(ContentViewModel),
            typeof(ContentItemView),
            default(ContentViewModel));

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

    public ContentViewModel SubscriptionCommandParameter
    {
        get { return (ContentViewModel)GetValue(SubscriptionCommandParameterProperty); }
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
}