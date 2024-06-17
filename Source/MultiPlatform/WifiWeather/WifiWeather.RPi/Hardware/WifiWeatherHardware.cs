using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Peripherals.Displays;
using WifiWeather.Core.Contracts;
using WifiWeather.RPi.Controllers;

namespace WifiWeather.RPi.Hardware;

internal class WifiWeatherHardware : IWifiWeatherHardware
{
    private readonly RaspberryPi device;
    private readonly IPixelDisplay? display = null;

    public IPixelDisplay? Display => display;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public INetworkController NetworkController { get; }

    public WifiWeatherHardware(RaspberryPi device)
    {
        this.device = device;

        var spiBus = device.CreateSpiBus(
            device.Pins.SPI0_SCLK,
            device.Pins.SPI0_MOSI,
            device.Pins.SPI0_MISO,
            new Meadow.Units.Frequency(48, Meadow.Units.Frequency.UnitType.Megahertz));

        display = new Ili9341
        (
            spiBus,
            chipSelectPin: device.Pins.GPIO16,
            dcPin: device.Pins.GPIO21,
            resetPin: device.Pins.GPIO20,
            width: 240, height: 320
        );

        NetworkController = new NetworkController();
    }
}