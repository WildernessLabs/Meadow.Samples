using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors.Buttons;

namespace GalleryViewer.Hardware;

internal interface IGalleryViewerHardware
{
    public IPixelDisplay Display { get; }

    public IButton RightButton { get; }

    public IButton LeftButton { get; }

    public IRgbPwmLed RgbPwmLed { get; }

    public void Initialize();
}