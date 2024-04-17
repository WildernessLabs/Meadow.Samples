using Meadow;
using Meadow.Devices;
using Meadow.Gateways;
using MeadowBleLed.Connectivity;

namespace MeadowBleLed.Controllers;

public class MainController
{
    private IBluetoothAdapter bluetooth;

    private LedController ledController;
    private BluetoothServer bluetoothServer;
    private CommandController commandController;

    public MainController(F7FeatherV2 hardware, IBluetoothAdapter bluetooth)
    {
        this.bluetooth = bluetooth;

        commandController = new CommandController();

        ledController = new LedController(hardware.Pins.OnboardLedRed, hardware.Pins.OnboardLedGreen, hardware.Pins.OnboardLedBlue);
        //ledController = new LedController(hardware.Pins.D12, hardware.Pins.D11, hardware.Pins.D12);

        StartBluetoothServer();
    }

    private void StartBluetoothServer()
    {
        bluetoothServer = new BluetoothServer();

        var definition = bluetoothServer.GetDefinition();
        bluetooth.StartBluetoothServer(definition);

        commandController.LedOnValueSet += (s, e) =>
        {
            Resolver.Log.Info("LedOnValueSet");
            ledController.TurnOn();
        };
        commandController.LedOffValueSet += (s, e) =>
        {
            Resolver.Log.Info("LedOffValueSet");
            ledController.TurnOff();
        };
        commandController.LedBlinkValueSet += (s, e) =>
        {
            Resolver.Log.Info("LedBlinkValueSet");
            ledController.StartBlink();
        };
        commandController.LedPulseValueSet += (s, e) =>
        {
            Resolver.Log.Info("LedPulseValueSet");
            ledController.StartPulse();
        };
        commandController.LedRunColorsValueSet += (s, e) =>
        {
            Resolver.Log.Info("LedRunColorsValueSet");
            ledController.StartRunningColors();
        };
    }
}