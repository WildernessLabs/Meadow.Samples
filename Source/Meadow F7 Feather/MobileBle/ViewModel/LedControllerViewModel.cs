using MobileBle.Utils;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Windows.Input;

namespace MobileBle.ViewModel
{
    public class LedControllerViewModel : BaseViewModel
    {
        ICharacteristic On;
        ICharacteristic Off;
        ICharacteristic StartPulsing;
        ICharacteristic StartBlinking;
        ICharacteristic StartRunningColors;

        bool _isOn;
        public bool IsOn
        {
            get => _isOn;
            set { _isOn = value; OnPropertyChanged(nameof(IsOn)); }
        }

        bool _isOff;
        public bool IsOff
        {
            get => _isOff;
            set { _isOff = value; OnPropertyChanged(nameof(IsOff)); }
        }

        bool _isPulsing;
        public bool IsPulsing
        {
            get => _isPulsing;
            set { _isPulsing = value; OnPropertyChanged(nameof(IsPulsing)); }
        }

        bool _isBlinking;
        public bool IsBlinking
        {
            get => _isBlinking;
            set { _isBlinking = value; OnPropertyChanged(nameof(IsBlinking)); }
        }

        bool _isRunningColors;
        public bool IsRunningColors
        {
            get => _isRunningColors;
            set { _isRunningColors = value; OnPropertyChanged(nameof(IsRunningColors)); }
        }

        public ICommand SendCommand { get; set; }

        public LedControllerViewModel()
        {
            IsOn = true;

            adapter.DeviceConnected += AdapterDeviceConnected;
            adapter.DeviceDisconnected += AdapterDeviceDisconnected;

            SendCommand = new Command(async (obj) => await SendLedCommand(obj as string));
        }

        void AdapterDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            IsConnected = false;
        }

        async void AdapterDeviceConnected(object sender, DeviceEventArgs e)
        {
            IsConnected = true;

            IDevice device = e.Device;

            var services = await device.GetServicesAsync();

            foreach (var serviceItem in services)
            {
                if (UuidToUshort(serviceItem.Id.ToString()) == DEVICE_ID)
                {
                    service = serviceItem;
                }
            }

            On = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.ON));
            Off = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.OFF));
            StartBlinking = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.BLINKING));
            StartPulsing = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.PULSING));
            StartRunningColors = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.RUNNING_COLORS));
        }

        async Task SendLedCommand(string command)
        {
            byte[] value = new byte[1] { 1 };

            try
            {
                IsOn = IsOff = IsBlinking = IsPulsing = IsRunningColors = false;

                switch (command)
                {
                    case "TurnOn": 
                        IsOn = true;
                        await On.WriteAsync(value);
                        break;

                    case "TurnOff": 
                        IsOff = true;
                        await Off.WriteAsync(value);
                        break;

                    case "StartBlink": 
                        IsBlinking = true;
                        await StartBlinking.WriteAsync(value);
                        break;

                    case "StartPulse": 
                        IsPulsing = true;
                        await StartPulsing.WriteAsync(value);
                        break;

                    case "StartRunningColors": 
                        IsRunningColors = true;
                        await StartRunningColors.WriteAsync(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}