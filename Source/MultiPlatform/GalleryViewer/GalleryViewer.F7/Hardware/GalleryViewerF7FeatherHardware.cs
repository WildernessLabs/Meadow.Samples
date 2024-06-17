using GalleryViewer.Core.Contracts;
using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;

namespace GalleryViewer.F7;

internal class GalleryViewerF7FeatherHardware : IGalleryViewerHardware
{
    public IPixelDisplay? Display => null;

    public RotationType DisplayRotation => RotationType.Default;

    public IButton? RightButton => null;

    public IButton? LeftButton => null;

    public GalleryViewerF7FeatherHardware(F7FeatherBase device)
    {

    }
}