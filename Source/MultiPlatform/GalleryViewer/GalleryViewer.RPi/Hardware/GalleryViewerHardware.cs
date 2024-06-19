using GalleryViewer.Core.Contracts;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;

namespace GalleryViewer.RPi.Hardware;

internal class GalleryViewerHardware : IGalleryViewerHardware
{
    private readonly RaspberryPi device;
    private readonly IButton? leftButton;
    private readonly IButton? rightButton;
    private readonly IPixelDisplay? display;

    public IButton? LeftButton => leftButton;

    public IButton? RightButton => rightButton;

    public IPixelDisplay? Display => display;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public GalleryViewerHardware(RaspberryPi device)
    {
        this.device = device;

        leftButton = new PushButton(device.Pins.GPIO26, ResistorMode.ExternalPullUp);

        rightButton = new PushButton(device.Pins.GPIO19, ResistorMode.ExternalPullUp);

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
    }
}