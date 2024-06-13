using Meadow;
using Meadow.Devices;
using System;
using System.Threading.Tasks;
using wifiweather.Core;

namespace wifiweather.F7
{
    public class wifiweatherProjectLabApp : App<F7CoreComputeV2>
    {
        private MainController mainController;

        public override Task Initialize()
        {
            var hardware = new wifiweatherProjectLabHardware(Device);
            mainController = new MainController();
            return mainController.Initialize(hardware);
        }

        public override Task Run()
        {
            return mainController.Run();
        }
    }
}