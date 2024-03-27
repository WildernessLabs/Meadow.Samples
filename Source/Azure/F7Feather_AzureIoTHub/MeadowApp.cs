using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace MeadowAzureIoTHub
{
    // Change F7MicroV2 to F7Micro for V1.x boards
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
}