using MobileProjectLab.ViewModel;

namespace MobileProjectLab.View
{
    public partial class BluetoothPage : ContentPage
    {
        BluetoothViewModel vm;

        public BluetoothPage()
        {
            InitializeComponent();
            BindingContext = vm = new BluetoothViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
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