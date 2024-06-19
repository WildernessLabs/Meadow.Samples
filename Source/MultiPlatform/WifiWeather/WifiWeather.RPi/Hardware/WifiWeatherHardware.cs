using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using WifiWeather.Core.Contracts;

namespace WifiWeather.RPi.Hardware;

internal class WifiWeatherHardware : IWifiWeatherHardware
{
    private readonly RaspberryPi device;
    private readonly IButton? upButton;
    private readonly IButton? downButton;
    private readonly IPixelDisplay? display;
    private readonly INetworkAdapter? networkAdapter;

    public IButton? UpButton => upButton;

    public IButton? DownButton => downButton;

    public IPixelDisplay? Display => display;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public INetworkAdapter? NetworkAdapter => networkAdapter;

    public WifiWeatherHardware(RaspberryPi device)
    {
        this.device = device;

        upButton = new PushButton(device.Pins.GPIO19, ResistorMode.ExternalPullUp);

        downButton = new PushButton(device.Pins.GPIO26, ResistorMode.ExternalPullUp);

        var spiBus = device.CreateSpiBus(
            device.Pins.SPI0_SCLK,
            device.Pins.SPI0_MOSI,
            device.Pins.SPI0_MISO,
            new Frequency(48, Frequency.UnitType.Megahertz));

        display = new Ili9341
        (
            spiBus,
            chipSelectPin: device.Pins.GPIO16,
            dcPin: device.Pins.GPIO21,
            resetPin: device.Pins.GPIO20,
            width: 240, height: 320
        );

        if (MeadowApp.Device.NetworkAdapters.Count > 0)
        {
            networkAdapter = MeadowApp.Device.NetworkAdapters[0];
        }
    }
}