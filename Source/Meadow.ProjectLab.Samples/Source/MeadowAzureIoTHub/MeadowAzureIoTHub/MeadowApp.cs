using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using MeadowAzureIoTHub.Hardware;
using System.Threading.Tasks;

namespace MeadowAzureIoTHub
{
    // Change F7CoreComputeV2 to F7FeatherV2 for ProjectLab v2
    public class MeadowApp : App<F7CoreComputeV2>
    {
        MainController mainController;

        public override async Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            var hardware = new MeadowAzureIoTHubHardware();
            var network = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            mainController = new MainController(hardware, network);
            await mainController.Initialize();
        }

        public override async Task Run()
        {
            Resolver.Log.Info("Run...");

            await mainController.Run();
        }
    }
}