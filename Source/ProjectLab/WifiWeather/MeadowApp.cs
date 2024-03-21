using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading.Tasks;
using WifiWeather.Hardware;

namespace WifiWeather
{
    public class MeadowApp : App<F7CoreComputeV2>
    {
        MainController mainController;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            var hardware = new WifiWeatherHardware();
            var network = Device.NetworkAdapters.Primary<INetworkAdapter>();

            mainController = new MainController(hardware, network);
            mainController.Initialize();

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            Resolver.Log.Info("Run...");

            await mainController.Run();
        }
    }
}