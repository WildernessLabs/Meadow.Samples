using MobileWifi.Utils;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Text;
using System.Windows.Input;

namespace MobileWifi.ViewModel
{
    public class MeadowConnectViewModel : BaseViewModel
    {
        ICharacteristic CharacteristicSsid;
        ICharacteristic CharacteristicPassword;
        ICharacteristic CharacteristicHasJoinedWifi;

        bool _showPassword;
        public bool ShowPassword
        {
            get => _showPassword;
            set { _showPassword = value; OnPropertyChanged(nameof(ShowPassword)); }
        }

        bool _hasJoinedWifi;
        public bool HasJoinedWifi
        {
            get => _hasJoinedWifi;
            set { _hasJoinedWifi = value; OnPropertyChanged(nameof(HasJoinedWifi)); }
        }

        string _ssid;
        public string Ssid
        {
            get => _ssid;
            set { _ssid = value; OnPropertyChanged(nameof(Ssid)); }
        }

        string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public ICommand ToggleWifiConnectionCommand { get; set; }

        public ICommand TogglePasswordVisibility { get; set; }

        public MeadowConnectViewModel()
        {
            adapter.DeviceConnected += AdapterDeviceConnected;
            adapter.DeviceDisconnected += AdapterDeviceDisconnected;

            ToggleWifiConnectionCommand = new Command(async () =>
            {
                if (!IsBlePaired)
                {
                    return;
                }

                if (!HasJoinedWifi)
                {
                    var ssid = Encoding.ASCII.GetBytes(Ssid);
                    await CharacteristicSsid.WriteAsync(ssid);

                    await Task.Delay(500);

                    var password = Encoding.ASCII.GetBytes(Password);
                    await CharacteristicPassword.WriteAsync(password);

                    await Task.Delay(500);

                    await CharacteristicHasJoinedWifi.WriteAsync(TRUE);

                    HasJoinedWifi = true;
                }
                else
                {
                    Ssid = string.Empty;
                    Password = string.Empty;

                    await CharacteristicHasJoinedWifi.WriteAsync(FALSE);

                    IsBlePaired = false;
                    HasJoinedWifi = false;
                }
            });

            TogglePasswordVisibility = new Command(() => ShowPassword = !ShowPassword);
        }

        void AdapterDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            Ssid = string.Empty;
            Password = string.Empty;

            HasJoinedWifi = false;
            IsBlePaired = false;
        }

        async void AdapterDeviceConnected(object sender, DeviceEventArgs e)
        {
            IsBlePaired = true;

            IDevice device = e.Device;

            var services = await device.GetServicesAsync();

            foreach (var serviceItem in services)
            {
                if (UuidToUshort(serviceItem.Id.ToString()) == DEVICE_ID)
                {
                    service = serviceItem;
                }
            }

            CharacteristicSsid = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.SSID));
            CharacteristicPassword = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.PASSWORD));
            CharacteristicIsBlePaired = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.IS_BLE_PAIRED));
            CharacteristicHasJoinedWifi = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.HAS_JOINED_WIFI));

            await Task.Delay(1000);

            await CharacteristicIsBlePaired.WriteAsync(TRUE);

            HasJoinedWifi = (await CharacteristicHasJoinedWifi.ReadAsync())[0] == 1;
        }
    }
}