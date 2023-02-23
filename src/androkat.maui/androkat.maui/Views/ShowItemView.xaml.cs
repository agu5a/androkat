using androkat.hu.ViewModels;
using System.Windows.Input;

namespace androkat.hu.Views;

public partial class ShowItemView
{
    public static readonly BindableProperty SubscriptionCommandProperty =
        BindableProperty.Create(
            nameof(SubscriptionCommand),
            typeof(ICommand),
            typeof(ShowItemView),
            default(string));

    public static readonly BindableProperty SubscriptionCommandParameterProperty =
        BindableProperty.Create(
            nameof(SubscriptionCommandParameter),
            typeof(NavigationViewModel),
            typeof(ShowItemView),
            default(NavigationViewModel));

    public ICommand SubscriptionCommand
    {
        get { return (ICommand)GetValue(SubscriptionCommandProperty); }
        set { SetValue(SubscriptionCommandProperty, value); }
    }

    public NavigationViewModel SubscriptionCommandParameter
    {
        get { return (NavigationViewModel)GetValue(SubscriptionCommandParameterProperty); }
        set { SetValue(SubscriptionCommandParameterProperty, value); }
    }    

    public ShowItemView()
    {
        InitializeComponent();
    }    
}