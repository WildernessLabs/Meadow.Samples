using GnssTrackerConnectivity.Common.Models;
using Meadow.Foundation.Web.Maple;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace MobileGnssTrackerConnectivity.ViewModel
{
    public class MapleViewModel : BaseViewModel
    {
        public MapleClient client { get; private set; }

        int _serverPort;
        public int ServerPort
        {
            get => _serverPort;
            set { _serverPort = value; OnPropertyChanged(nameof(ServerPort)); }
        }

        bool _isScanning;
        public bool IsScanning
        {
            get => _isScanning;
            set { _isScanning = value; OnPropertyChanged(nameof(IsScanning)); }
        }

        bool _isServerListEmpty;
        public bool IsServerListEmpty
        {
            get => _isServerListEmpty;
            set { _isServerListEmpty = value; OnPropertyChanged(nameof(IsServerListEmpty)); }
        }

        string ipAddress;
        public string IpAddress
        {
            get => ipAddress;
            set { ipAddress = value; OnPropertyChanged(nameof(IpAddress)); }
        }

        ServerModel _selectedServer;
        public ServerModel SelectedServer
        {
            get => _selectedServer;
            set
            {
                if (value == null) return;
                _selectedServer = value;
                IpAddress = _selectedServer.IpAddress;
                OnPropertyChanged(nameof(SelectedServer));
            }
        }

        public ObservableCollection<ServerModel> HostList { get; set; }

        public ICommand CmdSearchServers { get; private set; }

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
        public string MotionTemperature
        {
            get => motionTemperature;
            set { motionTemperature = value; OnPropertyChanged(nameof(MotionTemperature)); }
        }
        string motionTemperature = "0";
        public ICommand CmdGetMotionData { get; private set; }

        public MapleViewModel()
        {
            HostList = new ObservableCollection<ServerModel>();

            ServerPort = 5417;

            client = new MapleClient();
            client.Servers.CollectionChanged += ServersCollectionChanged;

            CmdSearchServers = new Command(async () => await GetServers());

            CmdSetOnboardLed = new Command(async (obj) => await SetOnboardLed(obj as string));

            CmdEnvironmentData = new Command(async () => await GetEnvironmentalData());

            //CmdGetLightData = new Command(async () => await GetLightData());

            CmdGetMotionData = new Command(async () => await GetMotionData());
        }

        void ServersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ServerModel server in e.NewItems)
                    {
                        HostList.Add(new ServerModel() { Name = $"{server.Name} ({server.IpAddress})", IpAddress = server.IpAddress });
                        Console.WriteLine($"'{server.Name}' @ ip:[{server.IpAddress}]");
                    }
                    break;
            }
        }

        async Task GetServers()
        {
            if (IsScanning)
                return;
            IsScanning = true;

            try
            {
                IsServerListEmpty = false;

                await client.StartScanningForAdvertisingServers();

                //HostList.Add(new ServerModel() { Name = "Meadow (192.168.1.81)", IpAddress = "192.168.1.81" });

                if (HostList.Count == 0)
                {
                    IsServerListEmpty = true;
                }
                else
                {
                    IsServerListEmpty = false;
                    SelectedServer = HostList[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsScanning = false;
            }
        }

        async Task SetOnboardLed(string command)
        {
            if (SelectedServer == null && string.IsNullOrEmpty(IpAddress))
                return;

            try
            {
                bool response = await client.PostAsync(SelectedServer != null ? SelectedServer.IpAddress : IpAddress, ServerPort, command, string.Empty);

                if (response)
                {
                    switch (command)
                    {
                        case "toggle": LedStatus = "Toggled"; break;
                        case "blink": LedStatus = "Blinking"; break;
                        case "pulse": LedStatus = "Pulsing"; break;
                    }
                }
                else
                {
                    Console.WriteLine("Request failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task GetEnvironmentalData()
        {
            if (SelectedServer == null && string.IsNullOrEmpty(IpAddress))
                return;

            try
            {
                var response = await client.GetAsync(SelectedServer != null ? SelectedServer.IpAddress : IpAddress, ServerPort, "getEnvironmentalData");

                if (response == null)
                {
                    return;
                }

                var value = System.Text.Json.JsonSerializer.Deserialize<AtmosphericReadingsDTO>(response);

                Temperature = value.Temperature;
                Humidity = value.Humidity;
                Pressure = value.Pressure;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task GetMotionData()
        {
            if (SelectedServer == null && string.IsNullOrEmpty(IpAddress))
                return;

            try
            {
                var response = await client.GetAsync(SelectedServer != null ? SelectedServer.IpAddress : IpAddress, ServerPort, "getMotionData");

                if (response == null)
                {
                    return;
                }

                var value = System.Text.Json.JsonSerializer.Deserialize<MotionReadingsDTO>(response);

                Acceleration3dX = value.Acceleration3dX;
                Acceleration3dY = value.Acceleration3dY;
                Acceleration3dZ = value.Acceleration3dZ;
                AngularVelocity3dX = value.AngularVelocity3dX;
                AngularVelocity3dY = value.AngularVelocity3dY;
                AngularVelocity3dZ = value.AngularVelocity3dZ;
                MotionTemperature = value.Temperature;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}