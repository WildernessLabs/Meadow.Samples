using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Ethernet_Basics
{
    public class MeadowApp : App<F7CoreComputeV2>
    {
        public override async Task Run()
        {
            Resolver.Log.Info("Run...");

            var ethernet = Device.NetworkAdapters.Primary<IWiredNetworkAdapter>();

            // connected event test.
            ethernet.NetworkConnected += EthernetAdapterNetworkConnected;

            if (ethernet.IsConnected)
            {
                DisplayNetworkInformation();

                while (true)
                {
                    await GetWebPageViaHttpClient("https://postman-echo.com/get?foo1=bar1&foo2=bar2");
                }
            }
        }

        void EthernetAdapterNetworkConnected(INetworkAdapter networkAdapter, NetworkConnectionEventArgs e)
        {
            Resolver.Log.Info("Connection request completed");
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
            Resolver.Log.Info($"Requesting {uri} - {DateTime.Now}");

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                HttpResponseMessage response = await client.GetAsync(uri);

                try
                {
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
                }
            }
        }
    }
}