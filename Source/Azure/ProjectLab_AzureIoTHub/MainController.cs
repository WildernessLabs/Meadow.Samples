using Meadow;
using Meadow.Hardware;
using Meadow.Units;
using MeadowAzureIoTHub.Controllers;
using MeadowAzureIoTHub.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowAzureIoTHub
{
    internal class MainController
    {
        bool useMQTT = true;

        int TIMEZONE_OFFSET = -8; // UTC-8

        IMeadowAzureIoTHubHardware hardware;
        IWiFiNetworkAdapter network;
        DisplayController displayController;
        IIoTHubController iotHubController;

        public MainController(IMeadowAzureIoTHubHardware hardware, IWiFiNetworkAdapter network)
        {
            this.hardware = hardware;
            this.network = network;
        }

        public async Task Initialize()
        {
            hardware.Initialize();

            displayController = new DisplayController(hardware.Display);
            displayController.ShowSplashScreen();
            Thread.Sleep(3000);
            displayController.ShowDataScreen();

            if (useMQTT)
            {
                displayController.UpdateType("MQTT");
                iotHubController = new IoTHubMqttController();
            }
            else
            {
                displayController.UpdateType("AMQP");
                iotHubController = new IoTHubAmqpController();
            }

            await InitializeIoTHub();

            hardware.EnvironmentalSensor.Updated += EnvironmentalSensorUpdated;
        }

        private async Task InitializeIoTHub()
        {
            while (!iotHubController.isAuthenticated)
            {
                displayController.UpdateWiFiStatus(network.IsConnected);

                if (network.IsConnected)
                {
                    displayController.UpdateStatus("Authenticating...");

                    bool authenticated = await iotHubController.Initialize();

                    if (authenticated)
                    {
                        displayController.UpdateStatus("Authenticated");
                        await Task.Delay(2000);
                        displayController.UpdateStatus(DateTime.Now.AddHours(TIMEZONE_OFFSET).ToString("hh:mm tt dd/MM/yy"));
                    }
                    else
                    {
                        displayController.UpdateStatus("Not Authenticated");
                    }
                }
                else
                {
                    displayController.UpdateStatus("Offline");
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        private async Task SendDataToIoTHub((Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance) data)
        {
            if (network.IsConnected && iotHubController.isAuthenticated)
            {
                displayController.UpdateSyncStatus(true);
                displayController.UpdateStatus("Sending data...");

                await iotHubController.SendEnvironmentalReading(data);

                displayController.UpdateStatus("Data sent!");
                Thread.Sleep(3000);

                displayController.UpdateLastUpdated(DateTime.Now.AddHours(TIMEZONE_OFFSET).ToString("hh:mm tt dd/MM/yy"));

                displayController.UpdateSyncStatus(false);
                displayController.UpdateStatus(DateTime.Now.AddHours(TIMEZONE_OFFSET).ToString("hh:mm tt dd/MM/yy"));
            }
        }

        private async void EnvironmentalSensorUpdated(object sender, IChangeResult<(Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance)> e)
        {
            hardware.RgbPwmLed.StartBlink(Color.Orange);

            displayController.UpdateAtmosphericConditions(
                temperature: e.New.Temperature.Value.Celsius,
                pressure: e.New.Pressure.Value.Millibar,
                humidity: e.New.Humidity.Value.Percent);

            await SendDataToIoTHub(e.New);

            hardware.RgbPwmLed.StartBlink(Color.Green);
        }

        public async Task Run()
        {
            hardware.EnvironmentalSensor.StartUpdating(TimeSpan.FromSeconds(15));

            while (true)
            {
                displayController.UpdateWiFiStatus(network.IsConnected);

                if (network.IsConnected)
                {
                    displayController.UpdateStatus(DateTime.Now.AddHours(TIMEZONE_OFFSET).ToString("hh:mm tt dd/MM/yy"));

                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
                else
                {
                    displayController.UpdateStatus("Offline...");

                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }
        }
    }
}