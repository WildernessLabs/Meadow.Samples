using Meadow;
using Meadow.Devices;
using Meadow.Gateways;
using Meadow.Units;
using MeadowBleServo.Connectivity;
using MeadowBleServo.Controllers;
using System.Threading.Tasks;

namespace MeadowBleServo;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    private IBluetoothAdapter ble;

    private ServoController servoController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        servoController = new ServoController();
        servoController.RotateTo(new Angle(0));

        var bluetoothServer = new BluetoothServer();
        ble = Device.BluetoothAdapter;
        ble.StartBluetoothServer(bluetoothServer.GetDefinition());

        return base.Initialize();
    }
}