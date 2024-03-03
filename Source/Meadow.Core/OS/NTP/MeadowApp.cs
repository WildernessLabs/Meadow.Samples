using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace NtpSample;

public class MeadowApp : App<F7CoreComputeV2>
{
    private bool _ntpTimeArrived = false;

    public override async Task Run()
    {
        Resolver.Log.Info($"Time pre-sync: {DateTime.Now}");

        //        Device.NetworkConnected += OnNetworkConnected;
        Device.PlatformOS.NtpClient.TimeChanged += OnTimeChanged;

        Resolver.Log.Info($"connecting to network");

        var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        _ = wifi.Connect("Solution-Family", "1234567890");
        wifi.NetworkConnected += OnNetworkConnected;

        while (!_ntpTimeArrived)
        {
            await Task.Delay(2500);

            if (!wifi.IsConnected)
            {
                Resolver.Log.Info($"waiting for network");
            }
            else
            {
                Resolver.Log.Info($"waiting for NTP...");
            }
        }

        Resolver.Log.Info($"Time Updated to: {DateTime.Now}");
    }

    private void OnNetworkConnected(Meadow.Hardware.INetworkAdapter sender, Meadow.Hardware.NetworkConnectionEventArgs args)
    {
        Resolver.Log.Info($"NTP Sync");
        // fire and forget
        _ = Device.PlatformOS.NtpClient.Synchronize();
    }

    private void OnTimeChanged(DateTime utcTime)
    {
        Resolver.Log.Info($"NTP Returned: {DateTime.Now}");
        _ntpTimeArrived = true;
    }
}