using F7Feather_AzureIoTHub.Controllers;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace F7Feather_AzureIoTHub;

internal class MainController
{
    bool useMQTT = true;

    Htu21d atmosphericSensor;
    DisplayController displayController;

    IWiFiNetworkAdapter network;
    IIoTHubController iotHubController;

    public MainController(IWiFiNetworkAdapter network)
    {
        this.network = network;
    }

    public async Task Initialize()
    {
        var display = new St7789
        (
            spiBus: MeadowApp.Device.CreateSpiBus(),
            chipSelectPin: MeadowApp.Device.Pins.D02,
            dcPin: MeadowApp.Device.Pins.D01,
            resetPin: MeadowApp.Device.Pins.D00,
            width: 240, height: 240
        );

        displayController = new DisplayController(display);
        displayController.ShowSplashScreen();
        Thread.Sleep(5000);
        displayController.ShowDataScreen();

        if (useMQTT)
        {
            displayController.UpdateTitle("MQTT");
            iotHubController = new IoTHubMqttController();
        }
        else
        {
            displayController.UpdateTitle("AMQP");
            iotHubController = new IoTHubAmqpController();
        }

        await InitializeIoTHub();

        atmosphericSensor = new Htu21d(MeadowApp.Device.CreateI2cBus());
        atmosphericSensor.Updated += AtmosphericSensorUpdated;
    }

    private async Task InitializeIoTHub()
    {
        while (!iotHubController.isAuthenticated)
        {
            displayController.UpdateWiFiStatus(network.IsConnected);

            if (network.IsConnected)
            {
                bool authenticated = await iotHubController.Initialize();

                if (authenticated)
                {
                    Resolver.Log.Info("Authenticated");

                }
                else
                {
                    Resolver.Log.Info("Not Authenticated");
                }
            }
            else
            {
                Resolver.Log.Info("Offline");
            }

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }

    private async void AtmosphericSensorUpdated(object sender, IChangeResult<(Temperature? Temperature, RelativeHumidity? Humidity)> e)
    {
        displayController.UpdateWiFiStatus(network.IsConnected);

        if (network.IsConnected && iotHubController.isAuthenticated)
        {
            displayController.UpdateSyncStatus(true);

            await iotHubController.SendEnvironmentalReading(e.New);
            displayController.UpdateReadings(e.New);

            displayController.UpdateSyncStatus(false);
        }
    }

    public Task Run()
    {
        atmosphericSensor.StartUpdating(TimeSpan.FromSeconds(15));

        return Task.CompletedTask;
    }
}