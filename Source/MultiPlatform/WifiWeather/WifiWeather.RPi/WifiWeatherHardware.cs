using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using WifiWeather.Core;
using WifiWeather.Core.Contracts;

namespace WifiWeather.RPi;

internal class WifiWeatherHardware : IWifiWeatherHardware
{
    private readonly RaspberryPi device;
    private readonly IPixelDisplay? display = null;
    private readonly ITemperatureSensor temperatureSimulator;

    public RotationType DisplayRotation => RotationType.Default;
    public IPixelDisplay? Display => display;
    public ITemperatureSensor? TemperatureSensor => temperatureSimulator;
    public IButton? RightButton => null;
    public IButton? LeftButton => null;
    public INetworkController NetworkController { get; }


    public WifiWeatherHardware(RaspberryPi device, bool supportDisplay)
    {
        this.device = device;

        if (supportDisplay)
        { // only if we have a display attached
            display = new GtkDisplay(ColorMode.Format16bppRgb565);
        }
    }
}