using GalleryViewer.Core.Contracts;
using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;

namespace GalleryViewer.F7;

internal class GalleryViewerF7FeatherHardware : IGalleryViewerHardware
{
    private readonly ITemperatureSensor temperatureSensor;

    public RotationType DisplayRotation => RotationType.Default;
    public ITemperatureSensor? TemperatureSensor => temperatureSensor;
    public IButton? RightButton => null;
    public IButton? LeftButton => null;
    public IPixelDisplay? Display => null;

    public GalleryViewerF7FeatherHardware(F7FeatherBase device)
    {

    }
}