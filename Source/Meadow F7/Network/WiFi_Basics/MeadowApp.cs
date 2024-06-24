using Meadow;
using Meadow.Devices;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace WiFi_Basics;

public class CoreComputeApp : MeadowApp<F7CoreComputeV2> { }
public class F7FeatherV2App : MeadowApp<F7FeatherV2> { }
public class F7FeatherV1App : MeadowApp<F7FeatherV1> { }

public class MeadowApp<T> : App<T>
    where T : F7MicroBase
{
    private const string WIFI_NAME = "myWiFi";
    private const string WIFI_PASSWORD = "myPassword";
    private IWiFiNetworkAdapter wifi;

    public override async Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        // get the wifi adapter
        wifi = Resolver.Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        wifi.NetworkConnected += NetworkConnected;

        wifi.NetworkConnecting += NetworkConnecting;

        wifi.NetworkDisconnected += NetworkDisconnected;

        // Enumerate the public WiFi channels
        await ScanForAccessPoints(wifi);

        try
        {
            Resolver.Log.Info($"Connecting to WiFi Network {WIFI_NAME}");

            // connect to the wifi network.
            await wifi.Connect(WIFI_NAME, WIFI_PASSWORD, TimeSpan.FromSeconds(45));
        }
        catch (Exception ex)
        {
            Resolver.Log.Error($"Failed to Connect: {ex.Message}");
        }
    }

    private void NetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
    {
        Resolver.Log.Info($"Joined network - IP Address: {args.IpAddress}");
    }

    private void NetworkConnecting(INetworkAdapter sender)
    {
        Resolver.Log.Info($"Network connecting...");
    }

    private void NetworkDisconnected(INetworkAdapter sender, NetworkDisconnectionEventArgs args)
    {
        Resolver.Log.Info($"Network disconnected because {args.Reason}");
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        if (wifi.IsConnected)
        {
            DisplayNetworkInformation();

            while (true)
            {
                await GetWebPageViaHttpClient("https://postman-echo.com/get?foo1=bar1&foo2=bar2");
            }
        }
    }

    private async Task ScanForAccessPoints(IWiFiNetworkAdapter wifi)
    {
        Resolver.Log.Info("Getting list of access points");
        var networks = await wifi.Scan(TimeSpan.FromSeconds(60));

        if (networks.Count > 0)
        {
            Resolver.Log.Info("|-------------------------------------------------------------|---------|");
            Resolver.Log.Info("|         Network Name             | RSSI |       BSSID       | Channel |");
            Resolver.Log.Info("|-------------------------------------------------------------|---------|");

            foreach (WifiNetwork accessPoint in networks)
            {
                Resolver.Log.Info($"| {accessPoint.Ssid,-32} | {accessPoint.SignalDbStrength,4} | {accessPoint.Bssid,17} |   {accessPoint.ChannelCenterFrequency,3}   |");
            }
        }
        else
        {
            Resolver.Log.Info($"No access points detected");
        }
    }

    public void DisplayNetworkInformation()
    {
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

        if (adapters.Length == 0)
        {
            Resolver.Log.Warn("No adapters available");
        }
        else
        {
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                Resolver.Log.Info("");
                Resolver.Log.Info(adapter.Description);
                Resolver.Log.Info(string.Empty.PadLeft(adapter.Description.Length, '='));
                Resolver.Log.Info($"  Adapter name: {adapter.Name}");
                Resolver.Log.Info($"  Interface type .......................... : {adapter.NetworkInterfaceType}");
                Resolver.Log.Info($"  Physical Address ........................ : {adapter.GetPhysicalAddress()}");
                Resolver.Log.Info($"  Operational status ...................... : {adapter.OperationalStatus}");

                string versions = string.Empty;

                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    versions = "IPv4";
                }

                if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                {
                    if (versions.Length > 0)
                    {
                        versions += " ";
                    }
                    versions += "IPv6";
                }

                Resolver.Log.Info($"  IP version .............................. : {versions}");

                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                    Resolver.Log.Info($"  MTU ..................................... : {ipv4.Mtu}");
                }

                if ((adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) || (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            Resolver.Log.Info($"  IP address .............................. : {ip.Address}");
                            Resolver.Log.Info($"  Subnet mask ............................. : {ip.IPv4Mask}");
                        }
                    }
                }
            }
        }
    }

    public async Task GetWebPageViaHttpClient(string uri)
    {
        try
        {
            Resolver.Log.Info($"Requesting {uri} - {DateTime.Now}");

            using HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 5, 0);

            HttpResponseMessage response = await client.GetAsync(uri);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Resolver.Log.Info(responseBody);
        }
        catch (TaskCanceledException)
        {
            Resolver.Log.Info("Request time out.");
        }
        catch (Exception e)
        {
            Resolver.Log.Info($"Request went sideways: {e.Message}");
            await Task.Delay(5000);
        }
    }
}