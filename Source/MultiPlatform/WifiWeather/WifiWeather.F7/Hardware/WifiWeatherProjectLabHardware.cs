using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using System;
using System.Threading.Tasks;
using wifiweather.Core;
using wifiweather.Core.Contracts;

namespace wifiweather.F7
{
    internal class wifiweatherProjectLabHardware : IwifiweatherHardware
    {
        private readonly IProjectLabHardware projLab;

        public RotationType DisplayRotation => RotationType._270Degrees;
        public IOutputController OutputController { get; }
        public IButton? LeftButton => projLab.LeftButton;
        public IButton? RightButton => projLab.RightButton;
        public ITemperatureSensor? TemperatureSensor => projLab.TemperatureSensor;
        public IPixelDisplay? Display => projLab.Display;
        public INetworkController NetworkController { get; }

        public wifiweatherProjectLabHardware(F7CoreComputeV2 device)
        {
            projLab = ProjectLab.Create();

            OutputController = new OutputController(projLab.RgbLed);
            NetworkController = new NetworkController(device);
        }
    }
}