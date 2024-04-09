using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Web.Maple;
using Meadow.Gateways;
using Meadow.Hardware;
using MeadowConnectedSample.Connectivity;
using System;
using System.Threading.Tasks;

namespace MeadowConnectedSample.Controllers;

public class MainController
{
    // Connect via Maple (WiFi) or Bluetooth? 
    private bool useWifi = true;

    private IProjectLabHardware hardware;
    private IWiFiNetworkAdapter wifi;
    private IBluetoothAdapter bluetooth;

    private SensorController sensorController;
    private CommandController commandController;
    private BluetoothServer bluetoothServer;

    private DisplayController displayController;
    private LedController ledController;

    public MainController(IProjectLabHardware hardware, IWiFiNetworkAdapter wifi, IBluetoothAdapter bluetooth)
    {
        this.hardware = hardware;
        this.wifi = wifi;
        this.bluetooth = bluetooth;
    }

    public async Task Initialize()
    {
        sensorController = new SensorController(hardware);
        commandController = new CommandController();
        SubscribeLedCommands();

        displayController = new DisplayController(hardware.Display);
        displayController.ShowSplashScreen();

        ledController = new LedController(hardware.RgbLed);

        _ = displayController.StartConnectingAnimation(useWifi);

        if (useWifi)
        {
            await StartMapleServer();
        }
        else
        {
            StartBluetoothServer();
        }
    }

    private async Task StartMapleServer()
    {
        wifi.NetworkConnected += (s, e) =>
        {
            displayController.StopConnectingAnimation();

            _ = sensorController.StartUpdating(TimeSpan.FromSeconds(15));

            var mapleServer = new MapleServer(s.IpAddress, 5417, advertise: true, logger: Resolver.Log);
            mapleServer.Start();

            displayController.ShowMapleReady(s.IpAddress.ToString());

            ledController.SetColor(Color.Green);
        };

        await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
    }

    private void StartBluetoothServer()
    {
        bluetoothServer = new BluetoothServer();

        commandController.PairingValueSet += (s, e) =>
        {
            Resolver.Log.Info("PairingValueSet");

            if (e)
            {
                displayController.ShowBluetoothPaired();
            }
            else
            {
                _ = displayController.StartConnectingAnimation(false);
            }
        };

        var definition = bluetoothServer.GetDefinition();
        bluetooth.StartBluetoothServer(definition);

        _ = sensorController.StartUpdating(TimeSpan.FromSeconds(15));

        ledController.SetColor(Color.Green);
    }

    private void SubscribeLedCommands()
    {
        var commandController = Resolver.Services.Get<CommandController>();
        commandController.LedToggleValueSet += (s, e) =>
        {
            Resolver.Log.Info("LedToggleValueSet");
            _ = ledController.Toggle();
        };
        commandController.LedBlinkValueSet += (s, e) =>
        {
            Resolver.Log.Info("LedBlinkValueSet");
            _ = ledController.StartBlink();
        };
        commandController.LedPulseValueSet += (s, e) =>
        {
            Resolver.Log.Info("LedPulseValueSet");
            _ = ledController.StartPulse();
        };
    }
}