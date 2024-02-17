using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Update_Sample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private Stopwatch _stopWatch;

        private IDigitalOutputPort green;

        public override Task Initialize()
        {
            Resolver.UpdateService.OnUpdateAvailable += (s, e) =>
            {
                Resolver.Log.Info($"An {e.UpdateType} update is available! Version: {e.Version} Size: {e.DownloadSize}");

                Resolver.Log.Info("Retrieving update...");
                _stopWatch = Stopwatch.StartNew();
                Resolver.UpdateService.RetrieveUpdate(e);
            };

            Resolver.UpdateService.OnUpdateRetrieved += async (s, e) =>
            {
                _stopWatch.Stop();
                Resolver.Log.Info($"Update {e.Version} has been retrieved, which took {_stopWatch.Elapsed.TotalSeconds} seconds.");

                // wait a little while to allow us to see output, etc.
                await Task.Delay(TimeSpan.FromSeconds(5));

                Resolver.Log.Info("Applying update...");
                Resolver.UpdateService.ApplyUpdate(e);
            };

            green = Device.Pins.OnboardLedGreen.CreateDigitalOutputPort();

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            var wifi = Resolver.Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            wifi.NetworkConnected += (s, e) =>
            {
                Resolver.Log.Info($"Network connected! (IP is {wifi.IpAddress})");

                //                green.State = true;

                //Task.Run(() => DirectMqttTest());
                //                Task.Run(() => ServerPingProc());
            };

            Resolver.Log.Info("Connecting to the network...");
            await wifi.Connect("BOBS_YOUR_UNCLE", "1234567890");

            while (true)
            {
                green.State = !green.State;
                await Task.Delay(1000);
            }

        }

        private async Task DirectMqttTest()
        {
            while (!await IsInternetAvailable())
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            Resolver.Log.Info("Running MQTT test...");

            var opts = new MqttClientOptionsBuilder()
                            .WithClientId("Test_Client")
                            .WithTcpServer("20.253.228.77", 1883)
                            //.WithCleanSession()
                            .Build();

            var factory = new MqttFactory();
            var client = factory.CreateMqttClient();

            client.ConnectedHandler = new MqttClientConnectedHandlerDelegate((f) =>
            {
                Resolver.Log.Info("MQTT connected");
            });

            try
            {
                await client.ConnectAsync(opts);
            }
            catch (Exception ex)
            {
                Resolver.Log.Debug($"MQTT exception: {ex.Message}");
            }
        }

        public async Task<bool> IsInternetAvailable()
        {
            try
            {
                Resolver.Log.Info("Querying google.com...");

                var client = new HttpClient();
                var response = await client.GetStringAsync("http://google.com");
                Resolver.Log.Debug($"Google responded with: {response}");
                return true;
            }
            catch (Exception ex)
            {
                Resolver.Log.Debug($"Error checking internet connection: {ex.Message}");
            }

            return false;
        }

        private async Task ServerPingProc()
        {
            var ping = new Ping();

            while (true)
            {
                try
                {
                    var result = ping.Send("192.168.1.133");

                    switch (result.Status)
                    {
                        case IPStatus.Success:
                            Resolver.Log.Info($"Ping response in: {result.RoundtripTime} ms");
                            break;
                        default:
                            Resolver.Log.Info($"Ping failed: {result.Status}");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Resolver.Log.Warn($"Ping failed: {e.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}