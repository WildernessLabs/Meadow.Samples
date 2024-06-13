using Meadow;
using Meadow.Devices;
using System;
using System.Threading.Tasks;
using wifiweather.Core;

namespace wifiweather.F7
{
    public class wifiweatherF7FeatherApp : App<F7FeatherV2>
    {
        private MainController mainController;

        public override Task Initialize()
        {
            var hardware = new wifiweatherF7FeatherHardware(Device);
            mainController = new MainController();
            return mainController.Initialize(hardware);
        }

        public override Task Run()
        {
            return mainController.Run();
        }
    }
}