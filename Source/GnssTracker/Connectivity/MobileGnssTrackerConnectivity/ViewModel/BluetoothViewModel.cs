using GnssTrackerConnectivity.Common.Bluetooth;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace MobileGnssTrackerConnectivity.ViewModel
{
    public class BluetoothViewModel : BaseViewModel
    {
        int listenTimeout = 5000;

        ushort DEVICE_ID = 253;

        IAdapter adapter;
        IService service;

        ICharacteristic pairingCharacteristic;
        ICharacteristic ledToggleCharacteristic;
        ICharacteristic ledBlinkCharacteristic;
        ICharacteristic ledPulseCharacteristic;
        ICharacteristic environmentalDataCharacteristic;
        ICharacteristic lightDataCharacteristic;
        ICharacteristic motionAccelerationDataCharacteristic;
        ICharacteristic motionAngularVelocityDataCharacteristic;

        public ObservableCollection<IDevice> DeviceList { get; set; }

        IDevice deviceSelected;
        public IDevice DeviceSelected
        {
            get => deviceSelected;
            set { deviceSelected = value; OnPropertyChanged(nameof(DeviceSelected)); }
        }

        bool isScanning;
        public bool IsScanning
        {
            get => isScanning;
            set { isScanning = value; OnPropertyChanged(nameof(IsScanning)); }
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

        public ICommand CmdToggleConnection { get; set; }

        public ICommand CmdSearchForDevices { get; set; }

        // Onboard RGB LED
        string ledStatus;
        public string LedStatus
        {
            get => ledStatus;
            set { ledStatus = value; OnPropertyChanged(nameof(LedStatus)); }
        }
        public ICommand CmdSetOnboardLed { get; private set; }

        // Environmental Sensor
        public string Temperature
        {
            get => temperature;
            set { temperature = value; OnPropertyChanged(nameof(Temperature)); }
        }
        string temperature = "0";
        public string Humidity
        {
            get => humidity;
            set { humidity = value; OnPropertyChanged(nameof(Humidity)); }
        }
        string humidity = "0";
        public string Pressure
        {
            get => pressure;
            set { pressure = value; OnPropertyChanged(nameof(Pressure)); }
        }
        string pressure = "0";
        public ICommand CmdEnvironmentData { get; private set; }

        // Light Sensor
        public string Illuminance
        {
            get => illuminance;
            set { illuminance = value; OnPropertyChanged(nameof(Illuminance)); }
        }
        string illuminance = "0";
        public ICommand CmdGetLightData { get; private set; }

        // Motion Sensor
        public string Acceleration3dX
        {
            get => acceleration3dX;
            set { acceleration3dX = value; OnPropertyChanged(nameof(Acceleration3dX)); }
        }
        string acceleration3dX = "0";
        public string Acceleration3dY
        {
            get => acceleration3dY;
            set { acceleration3dY = value; OnPropertyChanged(nameof(Acceleration3dY)); }
        }
        string acceleration3dY = "0";
        public string Acceleration3dZ
        {
            get => acceleration3dZ;
            set { acceleration3dZ = value; OnPropertyChanged(nameof(Acceleration3dZ)); }
        }
        string acceleration3dZ = "0";
        public string AngularVelocity3dX
        {
            get => angularVelocity3dX;
            set { angularVelocity3dX = value; OnPropertyChanged(nameof(AngularVelocity3dX)); }
        }
        string angularVelocity3dX = "0";
        public string AngularVelocity3dY
        {
            get => angularVelocity3dY;
            set { angularVelocity3dY = value; OnPropertyChanged(nameof(AngularVelocity3dY)); }
        }
        string angularVelocity3dY = "0";
        public string AngularVelocity3dZ
        {
            get => angularVelocity3dZ;
            set { angularVelocity3dZ = value; OnPropertyChanged(nameof(AngularVelocity3dZ)); }
        }
        string angularVelocity3dZ = "0";

        public ICommand CmdGetMotionData { get; private set; }

        public BluetoothViewModel()
        {
            DeviceList = new ObservableCollection<IDevice>();

            adapter = CrossBluetoothLE.Current.Adapter;
            adapter.ScanTimeout = listenTimeout;
            adapter.ScanMode = ScanMode.LowLatency;
            adapter.DeviceConnected += AdapterDeviceConnected;
            adapter.DeviceDiscovered += AdapterDeviceDiscovered;
            adapter.DeviceDisconnected += AdapterDeviceDisconnected;

            CmdToggleConnection = new Command(async () => await ToggleConnection());

            CmdSearchForDevices = new Command(async () => await SearchForDevices());

            CmdSetOnboardLed = new Command(async (obj) => await SetOnboardLed(obj as string));

            CmdEnvironmentData = new Command(async () => await GetEnvironmentalData());

            CmdGetLightData = new Command(async () => await GetLightData());

            CmdGetMotionData = new Command(async () => await GetMotionData());
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

            pairingCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.PAIRING));
            ledToggleCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.LED_TOGGLE));
            ledBlinkCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.LED_BLINK));
            ledPulseCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.LED_PULSE));
            environmentalDataCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.ENVIRONMENTAL_DATA));
            lightDataCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.LIGHT_DATA));
            motionAccelerationDataCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.MOTION_ACCELERATION));
            motionAngularVelocityDataCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.MOTION_ANGULAR_VELOCITY));

            await SetPairingStatus();
            await GetEnvironmentalData();
            await GetLightData();
            await GetMotionData();
        }

        async void AdapterDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            if (DeviceList.FirstOrDefault(x => x.Name == e.Device.Name) == null &&
                !string.IsNullOrEmpty(e.Device.Name))
            {
                DeviceList.Add(e.Device);
            }

            if (e.Device.Name == "ProjectLab")
            {
                await adapter.StopScanningForDevicesAsync();
                IsDeviceListEmpty = false;
                DeviceSelected = e.Device;
            }
        }

        async Task ScanTimeoutTask()
        {
            await Task.Delay(listenTimeout);
            await adapter.StopScanningForDevicesAsync();
            IsScanning = false;
        }

        async Task ToggleConnection()
        {
            try
            {
                if (IsConnected)
                {
                    IsConnected = false;
                    await SetPairingStatus();
                    await adapter.DisconnectDeviceAsync(DeviceSelected);

                }
                else
                {
                    await adapter.ConnectToDeviceAsync(DeviceSelected);
                    IsConnected = true;
                }
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

        async Task SearchForDevices()
        {
            try
            {
                IsScanning = true;

                var tasks = new Task[]
                {
                    ScanTimeoutTask(),
                    adapter.StartScanningForDevicesAsync()
                };

                await Task.WhenAny(tasks);
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

        async Task SetOnboardLed(string command)
        {
            byte[] array = new byte[1];

            switch (command)
            {
                case "toggle":
                    array[0] = 1;
                    await ledToggleCharacteristic.WriteAsync(array);
                    LedStatus = "Toggled";
                    break;

                case "blink":
                    await ledBlinkCharacteristic.WriteAsync(array);
                    LedStatus = "Blinking";
                    break;

                case "pulse":
                    await ledPulseCharacteristic.WriteAsync(array);
                    LedStatus = "Pulsing";
                    break;
            }
        }

        async Task GetEnvironmentalData()
        {
            var eDC = await environmentalDataCharacteristic.ReadAsync();
            var value = Encoding.Default.GetString(eDC.data).Split(';');

            Temperature = value[0];
            Humidity = value[1];
            Pressure = value[2];
        }

        async Task GetLightData()
        {
            var lDC = await lightDataCharacteristic.ReadAsync();
            Illuminance = Encoding.Default.GetString(lDC.data).Split(';')[0];
        }

        async Task GetMotionData()
        {
            var mADC = await motionAccelerationDataCharacteristic.ReadAsync();
            var accelerationValue = Encoding.Default.GetString(mADC.data).Split(';');
            Acceleration3dX = accelerationValue[0];
            Acceleration3dY = accelerationValue[1];
            Acceleration3dZ = accelerationValue[2];

            var mAVDC = await motionAngularVelocityDataCharacteristic.ReadAsync();
            var angularVelocityValue = Encoding.Default.GetString(mAVDC.data).Split(';');
            AngularVelocity3dX = angularVelocityValue[0];
            AngularVelocity3dY = angularVelocityValue[1];
            AngularVelocity3dZ = angularVelocityValue[2];
        }

        async Task SetPairingStatus()
        {
            byte[] array = new byte[1];
            array[0] = IsConnected ? (byte)1 : (byte)0;

            await pairingCharacteristic.WriteAsync(array);
        }

        protected int UuidToUshort(string uuid)
        {
            return int.Parse(uuid.Substring(4, 4), System.Globalization.NumberStyles.HexNumber); ;
        }
    }
}