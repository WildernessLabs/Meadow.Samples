using MobileBle.Utils;
using Plugin.BLE.Abstractions.Contracts;
using System.Text;
using System.Windows.Input;

namespace MobileBle.ViewModel
{
    public class TemperatureControllerViewModel : BaseViewModel
    {
        ICharacteristic tempCharacteristic;

        string temperatureValue;
        public string TemperatureValue
        {
            get => temperatureValue;
            set { temperatureValue = value; OnPropertyChanged(nameof(TemperatureValue)); }
        }

        public ICommand CmdGetTemperature { get; set; }

        public TemperatureControllerViewModel()
        {
            adapter.DeviceConnected += AdapterDeviceConnected;
            adapter.DeviceDisconnected += AdapterDeviceDisconnected;

            CmdGetTemperature = new Command(async () => await GetTemperature());
        }

        async void AdapterDeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
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

            tempCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.TEMPERATURE));
        }

        void AdapterDeviceDisconnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            IsConnected = false;
        }

        async Task GetTemperature()
        {
            try
            {
                TemperatureValue = Encoding.Default.GetString(await tempCharacteristic.ReadAsync()).Split(';')[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
