using androkat.maui.library.ViewModels;

namespace androkat.hu.Views;

public partial class ImaItemView : VerticalStackLayout
{
    public static readonly BindableProperty SubscriptionCommandProperty = BindableProperty.Create(
        nameof(SubscriptionCommand),
        typeof(Command<ImaContentViewModel>),
        typeof(ImaItemView));

    public static readonly BindableProperty SubscriptionCommandParameterProperty = BindableProperty.Create(
        nameof(SubscriptionCommandParameter),
        typeof(ImaContentViewModel),
        typeof(ImaItemView));

    public Command<ImaContentViewModel>? SubscriptionCommand
    {
        get => (Command<ImaContentViewModel>?)GetValue(SubscriptionCommandProperty);
        set => SetValue(SubscriptionCommandProperty, value);
    }

    public ImaContentViewModel? SubscriptionCommandParameter
    {
        get => (ImaContentViewModel?)GetValue(SubscriptionCommandParameterProperty);
        set => SetValue(SubscriptionCommandParameterProperty, value);
    }

    public ImaItemView()
    {
        InitializeComponent();
    }
}
