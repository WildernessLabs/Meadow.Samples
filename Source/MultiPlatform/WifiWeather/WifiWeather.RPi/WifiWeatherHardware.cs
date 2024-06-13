using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using System;
using System.Threading.Tasks;
using wifiweather.Core;
using wifiweather.Core.Contracts;

namespace wifiweather.RPi
{
    internal class wifiweatherHardware : IwifiweatherHardware
    {
        private readonly RaspberryPi device;
        private readonly IPixelDisplay? display = null;
        private readonly ITemperatureSensor temperatureSimulator;
        private readonly IOutputController outputService;

        public RotationType DisplayRotation => RotationType.Default;
        public IPixelDisplay? Display => display;
        public IOutputController OutputController => outputService;
        public ITemperatureSensor? TemperatureSensor => temperatureSimulator;
        public IButton? RightButton => null;
        public IButton? LeftButton => null;
        public INetworkController NetworkController { get; }


        public wifiweatherHardware(RaspberryPi device, bool supportDisplay)
        {
            this.device = device;

            if (supportDisplay)
            { // only if we have a display attached
                display = new GtkDisplay(ColorMode.Format16bppRgb565);
            }
        }
    }
}