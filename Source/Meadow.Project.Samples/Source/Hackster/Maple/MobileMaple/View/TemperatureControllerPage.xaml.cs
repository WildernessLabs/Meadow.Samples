using MobileMaple.ViewModel;

namespace MobileMaple.View;

public partial class TemperatureControllerPage : ContentPage
{
    public TemperatureControllerPage()
    {
        InitializeComponent();
        BindingContext = new TemperatureControllerViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await (BindingContext as TemperatureControllerViewModel).GetServers();
    }
}