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

        ICharacteristic ledPairingCharacteristic;
        ICharacteristic ledToggleCharacteristic;
        ICharacteristic ledBlinkCharacteristic;
        ICharacteristic ledPulseCharacteristic;
        ICharacteristic atmosphericDataCharacteristic;
        ICharacteristic motionDataCharacteristic;
        ICharacteristic voltageDataCharacteristic;

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

        public string GasResistance
        {
            get => gasResistance;
            set { gasResistance = value; OnPropertyChanged(nameof(GasResistance)); }
        }
        string gasResistance = "0";

        public string Co2Concentration
        {
            get => cO2Concentration;
            set { cO2Concentration = value; OnPropertyChanged(nameof(Co2Concentration)); }
        }
        string cO2Concentration = "0";

        public ICommand CmdEnvironmentData { get; private set; }

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

        // Voltages
        public string BatteryVoltage
        {
            get => batteryVoltage;
            set { batteryVoltage = value; OnPropertyChanged(nameof(BatteryVoltage)); }
        }
        string batteryVoltage = "0";

        public string SolarVoltage
        {
            get => solarVoltage;
            set { solarVoltage = value; OnPropertyChanged(nameof(SolarVoltage)); }
        }
        string solarVoltage = "0";

        public ICommand CmdGetVoltageData { get; private set; }

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

            CmdGetMotionData = new Command(async () => await GetMotionData());

            CmdGetVoltageData = new Command(async () => await GetVoltageData());
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

            ledPairingCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.LED_PAIRING));
            ledToggleCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.LED_TOGGLE));
            ledBlinkCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.LED_BLINK));
            ledPulseCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.LED_PULSE));
            atmosphericDataCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.ATMOSPHERIC_DATA));
            motionDataCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.MOTION_DATA));
            voltageDataCharacteristic = await service.GetCharacteristicAsync(Guid.Parse(CharacteristicsConstants.VOLTAGE_DATA));

            await SetPairingStatus();
            await GetEnvironmentalData();
            await GetMotionData();
            await GetVoltageData();
        }

        async void AdapterDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            if (DeviceList.FirstOrDefault(x => x.Name == e.Device.Name) == null &&
                !string.IsNullOrEmpty(e.Device.Name))
            {
                DeviceList.Add(e.Device);
            }

            if (e.Device.Name == "GnssTracker")
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
            var eDC = await atmosphericDataCharacteristic.ReadAsync();
            var value = Encoding.Default.GetString(eDC.data).Split(';');

            Temperature = value[0];
            Humidity = value[1];
            Pressure = value[2];
            GasResistance = value[3];
            Co2Concentration = "0";
        }

        async Task GetMotionData()
        {
            var mADC = await motionDataCharacteristic.ReadAsync();
            var motionValues = Encoding.Default.GetString(mADC.data).Split(';');
            Acceleration3dX = motionValues[0];
            Acceleration3dY = motionValues[1];
            Acceleration3dZ = motionValues[2];
            AngularVelocity3dX = motionValues[3];
            AngularVelocity3dY = motionValues[4];
            AngularVelocity3dZ = motionValues[5];
        }

        async Task GetVoltageData()
        {
            var vDC = await voltageDataCharacteristic.ReadAsync();
            var values = Encoding.Default.GetString(vDC.data).Split(';');
            BatteryVoltage = values[0];
            SolarVoltage = values[1];
        }

        async Task SetPairingStatus()
        {
            byte[] array = new byte[1];
            array[0] = IsConnected ? (byte)1 : (byte)0;

            await ledPairingCharacteristic.WriteAsync(array);
        }

        protected int UuidToUshort(string uuid)
        {
            return int.Parse(uuid.Substring(4, 4), System.Globalization.NumberStyles.HexNumber); ;
        }
    }
}