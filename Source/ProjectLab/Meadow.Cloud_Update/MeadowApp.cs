using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Meadow.Cloud_Update
{
    // Change F7FeatherV2 to F7FeatherV1 if using Feather V1 Meadow boards
    // Change to F7CoreComputeV2 for Project Lab V3.x
    public class MeadowApp : App<F7CoreComputeV2>
    {
        private DisplayController displayController;
        private MicroAudio audio;
        private IProjectLabHardware projLab;
        private Stopwatch stopwatch;

        private const string WIFI_NAME = "[SSID]";
        private const string WIFI_PASSWORD = "[Password]";

        public override Task Initialize()
        {
            Resolver.Log.LogLevel = Meadow.Logging.LogLevel.Trace;

            Resolver.Log.Info("Initialize hardware...");

            // connect to the wifi network
            ConnectToWiFi();

            //==== instantiate the project lab hardware
            projLab = ProjectLab.Create();

            Resolver.Log.Info($"Running on ProjectLab Hardware {projLab.RevisionString}");

            //projLab.RgbLed?.SetColor(Color.Blue);
            projLab.RgbLed?.SetColor(Color.Red);

            projLab.Speaker.SetVolume(0.5f);
            audio = new MicroAudio(projLab.Speaker);

            //---- display controller (handles display updates)
            if (projLab.Display is { } display)
            {
                Resolver.Log.Trace("Creating DisplayController");
                displayController = new DisplayController(display);
                Resolver.Log.Trace("DisplayController up");
            }

            //---- BH1750 Light Sensor
            if (projLab.LightSensor is { } bh1750)
            {
                bh1750.Updated += Bh1750Updated;
            }

            //---- BME688 Atmospheric sensor
            if ((projLab as ProjectLabHardwareBase).AtmosphericSensor is { } bme688)
            {
                bme688.Updated += Bme688Updated;
            }

            //---- BMI270 Accel/IMU
            if ((projLab as ProjectLabHardwareBase).MotionSensor is { } bmi270)
            {
                bmi270.Updated += Bmi270Updated;
            }

            //---- buttons
            if (projLab.RightButton is { } rightButton)
            {
                rightButton.PressStarted += (s, e) => displayController.RightButtonState = true;
                rightButton.PressEnded += (s, e) => displayController.RightButtonState = false;
            }

            if (projLab.DownButton is { } downButton)
            {
                downButton.PressStarted += (s, e) => displayController.DownButtonState = true;
                downButton.PressEnded += (s, e) => displayController.DownButtonState = false;
            }
            if (projLab.LeftButton is { } leftButton)
            {
                leftButton.PressStarted += (s, e) => displayController.LeftButtonState = true;
                leftButton.PressEnded += (s, e) => displayController.LeftButtonState = false;
            }
            if (projLab.UpButton is { } upButton)
            {
                upButton.PressStarted += (s, e) => displayController.UpButtonState = true;
                upButton.PressEnded += (s, e) => displayController.UpButtonState = false;
            }

            //==== OtA Updates
            //Resolver.UpdateService.OnUpdateAvailable += UpdateService_OnUpdateAvailable;
            //Resolver.UpdateService.OnUpdateRetrieved += UpdateService_OnUpdateRetrieved;

            //var svc = Resolver.Services.Get<IUpdateService>() as Meadow.Update.UpdateService;
            Resolver.UpdateService.ClearUpdates(); // comment to preserve persisted info

            Resolver.UpdateService.UpdateAvailable += (updateService, info) =>
            {
                Resolver.Log.Info($"Update available!");
                displayController.Notification = "Update Available!";

                _ = audio.PlaySystemSound(SystemSoundEffect.Notification);

                // queue it for retreival "later"
                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    updateService.RetrieveUpdate(info);
                });
            };

            Resolver.UpdateService.UpdateRetrieved += (updateService, info) =>
            {
                Resolver.Log.Info("Update retrieved!");
                displayController.Notification = "Applying update!";

                _ = audio.PlaySystemSound(SystemSoundEffect.Success);

                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    updateService.ApplyUpdate(info);
                });
            };

            Resolver.Log.Info("Initialization complete");

            return base.Initialize();
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            //_ = audio.PlaySystemSound(SystemSoundEffect.Success);

            //---- BH1750 Light Sensor
            if (projLab.LightSensor is { } bh1750)
            {
                bh1750.StartUpdating(TimeSpan.FromSeconds(5));
            }

            //---- BME688 Atmospheric sensor
            if ((projLab as ProjectLabHardwareBase).AtmosphericSensor is { } bme688)
            {
                bme688.StartUpdating(TimeSpan.FromSeconds(5));
            }

            //---- BMI270 Accel/IMU
            if ((projLab as ProjectLabHardwareBase).MotionSensor is { } bmi270)
            {
                bmi270.StartUpdating(TimeSpan.FromSeconds(5));
            }

            displayController?.Update();

            //Resolver.Log.Info("starting blink");
            //_ = projLab.RgbLed.StartBlink(WildernessLabsColors.PearGreen, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(2000), 0.5f);

            return base.Run();
        }

        //==== WiFi
        public async void ConnectToWiFi()
        {
            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            // connected event test.
            wifi.NetworkConnected += WiFiAdapter_NetworkConnected;

            try
            {
                // connect to the wifi network.
                Resolver.Log.Info($"Connecting to WiFi Network {WIFI_NAME}");
                await wifi.Connect(WIFI_NAME, WIFI_PASSWORD, TimeSpan.FromSeconds(45));
            }
            catch (Exception ex)
            {
                Resolver.Log.Error($"Failed to Connect: {ex.Message}");
            }

        }

        private void WiFiAdapter_NetworkConnected(INetworkAdapter networkAdapter, NetworkConnectionEventArgs e)
        {
            Resolver.Log.Info("Connection request completed");
            displayController.Notification = "WiFi Connected!";
        }
        //====

        //==== UPDATE SERVICE
        private void UpdateService_OnUpdateAvailable(Update.IUpdateService updateService, Update.UpdateInfo info)
        {
            Resolver.Log.Info($"An {info.UpdateType} update is available! Version: {info.Version} Size: {info.FileSize}");

            displayController.Notification = "Downloading update!";
            Resolver.Log.Info("Retrieving update...");
            stopwatch = Stopwatch.StartNew();
            Resolver.UpdateService.RetrieveUpdate(info);

        }

        private async void UpdateService_OnUpdateRetrieved(Update.IUpdateService updateService, Update.UpdateInfo info)
        {
            stopwatch.Stop();
            Resolver.Log.Info($"Update {info.Version} has been retrieved, which took {stopwatch.Elapsed.TotalSeconds} seconds.");

            // wait a little while to allow us to see output, etc.
            await Task.Delay(TimeSpan.FromSeconds(5));

            Resolver.Log.Info("Applying update...");
            Resolver.UpdateService.ApplyUpdate(info);
        }
        //====

        //==== SENSORS
        private void Bmi270Updated(object sender, IChangeResult<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, Temperature? Temperature)> e)
        {
            //Resolver.Log.Info($"BMI270: {e.New.Acceleration3D.Value.X.Gravity:0.0},{e.New.Acceleration3D.Value.Y.Gravity:0.0},{e.New.Acceleration3D.Value.Z.Gravity:0.0}g");
            if (displayController != null) { displayController.AccelerationConditions = e.New; }
        }

        private void Bme688Updated(object sender, IChangeResult<(Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance)> e)
        {
            //Resolver.Log.Info($"BME688: {(int)e.New.Temperature?.Celsius}C - {(int)e.New.Humidity?.Percent}% - {(int)e.New.Pressure?.Millibar}mbar");
            if (displayController != null) { displayController.AtmosphericConditions = e.New; }
        }

        private void Bh1750Updated(object sender, IChangeResult<Illuminance> e)
        {
            //Resolver.Log.Info($"BH1750: {e.New.Lux}");
            if (displayController != null) { displayController.LightConditions = e.New; }
        }
        //====
    }
}
