using GalleryViewer.Core.Contracts;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;

namespace GalleryViewer.RPi.Hardware;

internal class GalleryViewerHardware : IGalleryViewerHardware
{
    private readonly RaspberryPi device;
    private readonly IPixelDisplay? display = null;

    public IPixelDisplay? Display => display;

    public RotationType DisplayRotation => RotationType.Default;

    public IButton? RightButton => null;

    public IButton? LeftButton => null;

    public GalleryViewerHardware(RaspberryPi device, bool supportDisplay)
    {
        this.device = device;

        if (supportDisplay)
        { // only if we have a display attached
            display = new GtkDisplay(ColorMode.Format16bppRgb565);
        }
    }
}