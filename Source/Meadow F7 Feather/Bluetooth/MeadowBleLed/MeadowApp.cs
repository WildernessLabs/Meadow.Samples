using Meadow;
using Meadow.Devices;
using Meadow.Gateways;
using MeadowBleLed.Connectivity;
using MeadowBleLed.Controllers;
using System.Threading.Tasks;

namespace MeadowBleLed;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    private IBluetoothAdapter ble;

    private LedController ledController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        ledController = new LedController();
        ledController.SetColor(Color.Red);

        var bluetoothServer = new BluetoothServer();
        ble = Device.BluetoothAdapter;
        ble.StartBluetoothServer(bluetoothServer.GetDefinition());

        return base.Initialize();
    }
}