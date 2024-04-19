using Meadow;
using Meadow.Devices;
using Meadow.Gateways;
using MeadowBleTemperature.Connectivity;
using MeadowBleTemperature.Controllers;
using System;
using System.Threading.Tasks;

namespace MeadowBleTemperature;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    private IBluetoothAdapter ble;

    TemperatureController temperatureController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        temperatureController = new TemperatureController();
        temperatureController.StartUpdating(TimeSpan.FromSeconds(5));

        var bluetoothServer = new BluetoothServer();
        ble = Device.BluetoothAdapter;
        ble.StartBluetoothServer(bluetoothServer.GetDefinition());

        return base.Initialize();
    }

    //private void TemperatureUpdated(object sender, Meadow.Units.Temperature e)
    //{
    //    temperatureCharacteristic.SetValue($"{e.Celsius:N2}°C;");
    //}
}