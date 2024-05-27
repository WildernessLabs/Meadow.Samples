using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WiFi_Config;

public class CoreComputeApp : MeadowApp<F7CoreComputeV2> { }
public class F7FeatherV2App : MeadowApp<F7FeatherV2> { }
public class F7FeatherV1App : MeadowApp<F7FeatherV1> { }

public class MeadowApp<T> : App<T>
    where T : F7MicroBase
{
    IWiFiNetworkAdapter wifi;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        // get the wifi adapter
        wifi = Resolver.Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        // set initial state
        Resolver.Log.Info(wifi.IsConnected
            ? "Already connected to WiFi."
            : "Not connected to WiFi yet.");

        wifi.NetworkConnected += NetworkConnected;

        wifi.NetworkConnecting += NetworkConnecting;

        wifi.NetworkDisconnected += NetworkDisconnected;

        wifi.NetworkConnectFailed += NetworkConnectFailed;

        return Task.CompletedTask;
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

    private void NetworkConnectFailed(INetworkAdapter sender)
    {
        Resolver.Log.Info($"Network reconnection failed. Tried max number of times.");
    }

    private async Task GetWebPageViaHttpClient(string uri)
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

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        while (true)
        {
            if (wifi.IsConnected)
            {
                await GetWebPageViaHttpClient("https://postman-echo.com/get?foo1=bar1&foo2=bar2");
            }
            else
            {
                Resolver.Log.Info("Network not connected. Checking in 10s");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}