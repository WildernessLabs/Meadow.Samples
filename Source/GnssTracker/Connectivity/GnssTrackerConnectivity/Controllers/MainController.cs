using GnssTrackerConnectivity.Connectivity;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Web.Maple;
using Meadow.Gateways;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace GnssTrackerConnectivity.Controllers;

public class MainController
{
    // Connect via Maple (WiFi) or Bluetooth? 
    //private ConnectionType connectionType = ConnectionType.Bluetooth;
    private ConnectionType connectionType = ConnectionType.WiFi;

    private IGnssTrackerHardware hardware;
    private IWiFiNetworkAdapter wifi;
    private IBluetoothAdapter bluetooth;

    private SensorController sensorController;
    private CommandController commandController;
    private BluetoothServer bluetoothServer;

    private DisplayController displayController;
    private LedController ledController;

    public MainController(IGnssTrackerHardware hardware, IWiFiNetworkAdapter wifi, IBluetoothAdapter bluetooth)
    {
        this.hardware = hardware;
        this.wifi = wifi;
        this.bluetooth = bluetooth;
    }

    public async Task Initialize()
    {
        sensorController = new SensorController(hardware);
        _ = sensorController.StartUpdating(TimeSpan.FromSeconds(15));

        commandController = new CommandController();
        SubscribeLedCommands();

        displayController = new DisplayController(hardware.Display);

        ledController = new LedController(hardware.OnboardRgbLed);

        if (connectionType == ConnectionType.WiFi)
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
            var mapleServer = new MapleServer(s.IpAddress, 5417, advertise: true, logger: Resolver.Log);
            mapleServer.Start();

            displayController.ShowMapleReady(e.IpAddress.ToString());
            ledController.SetColor(Color.Green);
        };

        await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
    }

    private void StartBluetoothServer()
    {
        bluetoothServer = new BluetoothServer();

        commandController.PairingValueSet += (s, e) =>
        {
            if (e)
            {

            }
            else
            {

            }
        };

        var definition = bluetoothServer.GetDefinition();
        bluetooth.StartBluetoothServer(definition);

        ledController.SetColor(Color.Green);
    }

    private void SubscribeLedCommands()
    {
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