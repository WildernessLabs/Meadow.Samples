using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace ConfigFiles;

public class MeadowApp : App<F7FeatherV2>
{
    private IWiFiNetworkAdapter wifi;

    public override Task Initialize()
    {
        Resolver.Log.Info($"Log level: {Resolver.Log.LogLevel}");

        Resolver.Log.Trace($"Trace Message");
        Resolver.Log.Debug($"Debug Message");
        Resolver.Log.Info($"Info Message");
        Resolver.Log.Warn($"Warn Message");
        Resolver.Log.Error($"Error Message");

        wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        if (wifi.IsConnected)
        {
            Resolver.Log.Info("WiFi adapter already connected.");
        }
        else
        {
            Resolver.Log.Info("WiFi adapter not connected.");
            wifi.NetworkConnected += (s, e) =>
            {
                Resolver.Log.Info("WiFi adapter connected.");
            };
        }

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Debug($"+Run");
        StartHeartbeat();

        OutputDeviceInfo();
        OutputNtpInfo();
        OutputMeadowOSInfo();

        OutputDeviceConfigurationInfo();

        return Task.CompletedTask;
    }

    void OutputDeviceInfo()
    {
        Resolver.Log.Info($"=========================OutputDeviceInfo==============================");
        Resolver.Log.Info($"Device name: {Device.Information.DeviceName}");
        Resolver.Log.Info($"Processor serial number: {Device.Information.ProcessorSerialNumber}");
        Resolver.Log.Info($"Processor ID: {Device.Information.UniqueID}");
        Resolver.Log.Info($"Model: {Device.Information.Model}");
        Resolver.Log.Info($"Processor type: {Device.Information.ProcessorType}");
        Resolver.Log.Info($"Product: {Device.Information.Model}");
        Resolver.Log.Info($"Coprocessor type: {Device.Information.CoprocessorType}");
        Resolver.Log.Info($"Coprocessor firmware version: {Device.Information.CoprocessorOSVersion}");
        Resolver.Log.Info($"=======================================================================");
    }

    void OutputNtpInfo()
    {
        Resolver.Log.Info($"=========================OutputMeadowOSInfo============================");
        Resolver.Log.Info($"NTP Client Enabled: {Device.PlatformOS.NtpClient.Enabled}");
        Resolver.Log.Info($"=======================================================================");
    }

    void OutputMeadowOSInfo()
    {
        Resolver.Log.Info($"=========================OutputMeadowOSInfo============================");
        Resolver.Log.Info($"OS version: {MeadowOS.SystemInformation.OSVersion}");
        Resolver.Log.Info($"Runtime version: {MeadowOS.SystemInformation.RuntimeVersion}");
        Resolver.Log.Info($"Build date: {MeadowOS.SystemInformation.OSBuildDate}");
        Resolver.Log.Info($"=======================================================================");
    }

    void OutputDeviceConfigurationInfo()
    {
        try
        {
            // Retrieve
            var isF7PlatformOS = Device.PlatformOS is F7PlatformOS;
            var esp32Wifi = wifi as Esp32Coprocessor;
            if (isF7PlatformOS && esp32Wifi != null)
            {
                Resolver.Log.Info($"====================OutputDeviceConfigurationInfo======================");
                Resolver.Log.Info($"Automatically connect to network: {F7PlatformOS.GetBoolean(IPlatformOS.ConfigurationValues.AutomaticallyStartNetwork)}");
                Resolver.Log.Info($"Get time at startup: {F7PlatformOS.GetBoolean(IPlatformOS.ConfigurationValues.GetTimeAtStartup)}");
                Resolver.Log.Info($"Default access point: {F7PlatformOS.GetString(IPlatformOS.ConfigurationValues.DefaultAccessPoint)}");
                // Note: You can also access the maximum retry count via the ESP32 coprocessor using `esp32Wifi.MaximumRetryCount`.
                Resolver.Log.Info($"Maximum retry count: {F7PlatformOS.GetUInt(IPlatformOS.ConfigurationValues.MaximumNetworkRetryCount)}");
                Resolver.Log.Info($"=======================================================================");
            }
        }
        catch (Exception e)
        {
            Resolver.Log.Error(e.Message);
        }
    }

    protected void StartHeartbeat()
    {
        Resolver.Log.Debug($"+StartHeartbeat");

        Task.Run(async () =>
        {
            Resolver.Log.Trace($"Heartbeat Task Started");
            var countToReset = 1;

            while (true)
            {
                Resolver.Log.Debug($"Count to reset: {countToReset}");
                Resolver.Log.Info($"{DateTime.Now} {wifi.IpAddress}");
                await Task.Delay(TimeSpan.FromSeconds(10));

                Resolver.Log.Trace($"Testing for throw");
                if (--countToReset <= 0) throw new Exception("Testing restart...");

            }
        });
    }

    protected string FormatMacAddressString(byte[] address)
    {
        string result = string.Empty;
        for (int index = 0; index < address.Length; index++)
        {
            result += address[index].ToString("X2");
            if (index != (address.Length - 1))
            {
                result += ":";
            }
        }
        return (result);
    }
}