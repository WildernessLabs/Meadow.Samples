using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors.Buttons;

namespace GalleryViewer.Hardware;

internal class GalleryViewerHardware : IGalleryViewerHardware
{
    protected IProjectLabHardware ProjLab { get; }

    public IPixelDisplay Display { get; set; }

    public IButton RightButton { get; set; }

    public IButton LeftButton { get; set; }

    public IRgbPwmLed RgbPwmLed { get; set; }

    public GalleryViewerHardware(IProjectLabHardware projLab)
    {
        ProjLab = projLab;
    }

    public void Initialize()
    {
        Display = ProjLab.Display;

        RightButton = ProjLab.RightButton;

        LeftButton = ProjLab.LeftButton;

        RgbPwmLed = ProjLab.RgbLed;
    }
}
