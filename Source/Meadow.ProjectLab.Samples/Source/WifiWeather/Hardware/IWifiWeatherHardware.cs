using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;

namespace WifiWeather.Hardware
{
    internal interface IWifiWeatherHardware
    {
        IButton UpButton { get; }

        IButton DownButton { get; }

        IPixelDisplay Display { get; }

        ITemperatureSensor TemperatureSensor { get; }

        void Initialize();
    }
}