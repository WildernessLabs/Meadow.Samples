using MobileWifi.ViewModel;

namespace MobileWifi.View
{
    public partial class MeadowConnectPage : ContentPage
    {
        public MeadowConnectPage()
        {
            InitializeComponent();
            BindingContext = new MeadowConnectViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!(BindingContext as BaseViewModel).IsBlePaired)
            {
                (BindingContext as BaseViewModel).CmdSearchForDevices.Execute(null);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}