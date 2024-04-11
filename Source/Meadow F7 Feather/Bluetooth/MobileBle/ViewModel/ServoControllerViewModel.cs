using MobileBle.Utils;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Windows.Input;

namespace MobileBle.ViewModel
{
    public class ServoControllerViewModel : BaseViewModel
    {
        ICharacteristic toggleServo;
        ICharacteristic setAngle;

        int _angleDegrees;
        public int AngleDegrees
        {
            get => _angleDegrees;
            set { _angleDegrees = value; OnPropertyChanged(nameof(AngleDegrees)); }
        }

        bool _isRotateTo;
        public bool IsRotateTo
        {
            get => _isRotateTo;
            set { _isRotateTo = value; OnPropertyChanged(nameof(IsRotateTo)); }
        }

        bool _isSweeping;
        public bool IsSweeping
        {
            get => _isSweeping;
            set { _isSweeping = value; OnPropertyChanged(nameof(IsSweeping)); }
        }

        public ICommand CmdSetAngle { get; set; }

        public ICommand CmdToggle { get; set; }

        public ServoControllerViewModel()
        {
            IsRotateTo = true;

            adapter.DeviceConnected += AdapterDeviceConnected;
            adapter.DeviceDisconnected += AdapterDeviceDisconnected;

            CmdToggle = new Command(async () => await ServoToggle());

            CmdSetAngle = new Command(async () => await ServoSetAngle());
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

            toggleServo = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.IS_CYCLING));
            setAngle = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.ANGLE));
        }

        async Task ServoToggle()
        {
            IsRotateTo = false;

            byte[] array = new byte[1];
            array[0] = IsSweeping ? (byte)1 : (byte)0;

            var result = await toggleServo.WriteAsync(array);
            if (result == 0)
            {
                IsSweeping = !IsSweeping;
            }
        }

        async Task ServoSetAngle()
        {
            if (IsSweeping)
            {
                await ServoToggle();
            }
            IsRotateTo = true;

            byte[] array = new byte[1];
            array[0] = (byte)AngleDegrees;

            await setAngle.WriteAsync(array);
        }
    }
}