using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace F7Feather_AzureIoTHub;

// Change F7FeatherV2 to F7FeatherV1 for V1.x boards
public class MeadowApp : App<F7FeatherV2>
{
    MainController mainController;

    public override async Task Initialize()
    {
        var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        mainController = new MainController(wifi);
        await mainController.Initialize();
    }

    public override Task Run()
    {
        mainController.Run();

        return Task.CompletedTask;
    }
}