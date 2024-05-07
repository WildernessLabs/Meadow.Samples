using MobileGnssTrackerConnectivity.ViewModel;

namespace MobileGnssTrackerConnectivity.View
{
    public partial class BluetoothPage : ContentPage
    {
        BluetoothViewModel vm;

        public BluetoothPage()
        {
            InitializeComponent();
            BindingContext = vm = new BluetoothViewModel();


        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            PermissionStatus location = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            PermissionStatus mearby = await Permissions.RequestAsync<Permissions.Bluetooth>();

            vm.CmdSearchForDevices.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (vm.IsConnected)
            {
                vm.CmdToggleConnection.Execute(null);
            }
        }
    }
}