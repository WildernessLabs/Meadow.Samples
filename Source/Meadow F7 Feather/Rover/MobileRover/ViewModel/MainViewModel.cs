using MobileRover.Utils;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace MobileRover.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        byte[] MOVE = new byte[1] { 1 };
        byte[] STOP = new byte[1] { 0 };

        ushort DEVICE_ID = 253;

        IAdapter adapter;
        IService service;
        ICharacteristic up, down, left, right;

        public ObservableCollection<IDevice> DeviceList { get; set; }

        IDevice deviceSelected;
        public IDevice DeviceSelected
        {
            get => deviceSelected;
            set { deviceSelected = value; OnPropertyChanged(nameof(DeviceSelected)); }
        }

        bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set { isConnected = value; OnPropertyChanged(nameof(IsConnected)); }
        }

        bool isDeviceListEmpty;
        public bool IsDeviceListEmpty
        {
            get => isDeviceListEmpty;
            set { isDeviceListEmpty = value; OnPropertyChanged(nameof(IsDeviceListEmpty)); }
        }

        public ICommand CmdSearchForDevices { get; set; }

        public MainViewModel()
        {
            IBluetoothLE ble = CrossBluetoothLE.Current;

            DeviceList = new ObservableCollection<IDevice>();

            adapter = CrossBluetoothLE.Current.Adapter;
            adapter.ScanMode = ScanMode.LowLatency;
            adapter.DeviceDiscovered += DeviceDiscovered;
            adapter.DeviceConnected += DeviceConnected;
            adapter.DeviceDisconnected += (s, e) =>
            {
                IsConnected = false;
            };

            CmdSearchForDevices = new Command(async () => await DiscoverDevices());
        }

        int UuidToUshort(string uuid)
        {
            int result;

            string id = uuid.Substring(4, 4);

            result = int.Parse(id, System.Globalization.NumberStyles.HexNumber);

            return result;
        }

        async void DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            if (DeviceList.FirstOrDefault(x => x.Name == e.Device.Name) == null &&
                !string.IsNullOrEmpty(e.Device.Name))
            {
                DeviceList.Add(e.Device);
            }

            if (e.Device.Name == "MeadowRover")
            {
                IsDeviceListEmpty = false;
                DeviceSelected = e.Device;
                await adapter.ConnectToDeviceAsync(DeviceSelected);
            }
        }

        async void DeviceConnected(object sender, DeviceEventArgs e)
        {
            IsConnected = true;

            var services = await DeviceSelected.GetServicesAsync();
            foreach (var serviceItem in services)
            {
                if (UuidToUshort(serviceItem.Id.ToString()) == DEVICE_ID)
                {
                    service = serviceItem;
                }
            }

            up = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.UP));
            down = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.DOWN));
            left = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.LEFT));
            right = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.RIGHT));
        }

        async Task DiscoverDevices()
        {
            try
            {
                await adapter.StartScanningForDevicesAsync();
            }
            catch (DeviceConnectionException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task MoveForward(bool go)
        {
            try
            {
                await up.WriteAsync(go ? MOVE : STOP);
            }
            catch (Exception ex)
            {
                await up.WriteAsync(STOP);
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task MoveBackward(bool go)
        {
            try
            {
                await down.WriteAsync(go ? MOVE : STOP);
            }
            catch (Exception ex)
            {
                await down.WriteAsync(STOP);
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task TurnLeft(bool go)
        {
            try
            {
                await left.WriteAsync(go ? MOVE : STOP);
            }
            catch (Exception e)
            {
                await left.WriteAsync(STOP);
            }
        }

        public async Task TurnRight(bool go)
        {
            try
            {
                await right.WriteAsync(go ? MOVE : STOP);
            }
            catch (Exception e)
            {
                await right.WriteAsync(STOP);
            }
        }
    }
}
