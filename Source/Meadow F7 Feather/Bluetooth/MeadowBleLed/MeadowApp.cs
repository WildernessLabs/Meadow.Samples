using Meadow;
using Meadow.Devices;
using MeadowBleLed.Controllers;
using System.Threading.Tasks;

namespace MeadowBleLed;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var ble = Device.BluetoothAdapter;

        var mainController = new MainController();

        return base.Initialize();
    }
}