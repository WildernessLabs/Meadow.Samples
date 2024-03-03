using MobileBle.ViewModel;

namespace MobileBle.View
{
    public partial class LedControllerPage : ContentPage
    {
        public LedControllerPage()
        {
            InitializeComponent();
            BindingContext = new LedControllerViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as BaseViewModel).CmdSearchForDevices.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if ((BindingContext as BaseViewModel).IsConnected)
            {
                (BindingContext as BaseViewModel).CmdToggleConnection.Execute(null);
            }
        }
    }
}