using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;
using WifiWeather.Core;

namespace WifiWeather.F7
{
    public class WifiWeatherProjectLabApp : App<F7CoreComputeV2>
    {
        private MainController mainController;

        public override Task Initialize()
        {
            var hardware = new wifiweatherProjectLabHardware(Device);
            mainController = new MainController(hardware);
            mainController.Initialize();

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            _ = mainController!.Run();

            return Task.CompletedTask;
        }
    }
}