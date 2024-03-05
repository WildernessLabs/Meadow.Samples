using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Peripherals.Leds;
using Meadow.Units;
using MeadowAzureServo.Azure;
using MeadowAzureServo.Models;
using MeadowAzureServo.Views;
using System;
using System.Threading.Tasks;

namespace MeadowAzureServo
{
    // Change F7CoreComputeV2 to F7FeatherV2 for ProjectLab v2
    public class MeadowApp : App<F7CoreComputeV2>
    {
        private IRgbPwmLed onboardLed;
        private IoTHubManager amqpController;
        private IProjectLabHardware projectLab;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            projectLab = ProjectLab.Create();
            Resolver.Log.Info($"Running on ProjectLab Hardware {projectLab.RevisionString}");

            onboardLed = projectLab.RgbLed;
            onboardLed.SetColor(Color.Red);

            try
            {
                amqpController = new IoTHubManager();

                var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
                wifi.NetworkConnected += NetworkConnected;
            }
            catch (Exception ex)
            {
                Resolver.Log.Error($"Failed to Connect: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private async void NetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
        {
            await amqpController.Initialize();
            amqpController.StartSweeping += StartSweeping;
            amqpController.StopSweeping += StopSweeping;
            amqpController.RotateTo += RotateTo;

            (projectLab as ProjectLabHardwareBase).AtmosphericSensor.StartUpdating(TimeSpan.FromSeconds(15));

            onboardLed.SetColor(Color.Green);
        }

        private void StartSweeping(object sender, EventArgs e)
        {
            ServoController.Instance.StartSweep();
        }

        private void StopSweeping(object sender, EventArgs e)
        {
            ServoController.Instance.StopSweep();
        }

        private void RotateTo(object sender, RotateToEventArgs e)
        {
            ServoController.Instance.RotateTo(new Angle(e.AngleInDegrees));
        }
    }
}