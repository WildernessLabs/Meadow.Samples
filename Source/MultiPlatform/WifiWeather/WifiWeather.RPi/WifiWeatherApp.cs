using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Pinouts;
using System;
using System.Threading.Tasks;
using wifiweather.Core;

namespace wifiweather.RPi
{
    internal class wifiweatherApp : App<RaspberryPi>
    {
        private wifiweatherHardware hardware;
        private MainController mainController;

        public bool SupportDisplay { get; set; } = false;

        public override Task Initialize()
        {
            hardware = new wifiweatherHardware(Device, SupportDisplay);
            mainController = new MainController();
            return mainController.Initialize(hardware);
        }

        public override Task Run()
        {
            if (hardware.Display is GtkDisplay gtk)
            {
                _ = mainController.Run();
                gtk.Run();
            }

            return mainController.Run();
        }
    }
}