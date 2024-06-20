using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;

namespace GalleryViewer.Core.Contracts;

public interface IGalleryViewerHardware
{
    IButton? LeftButton { get; }

    IButton? RightButton { get; }

    IPixelDisplay? Display { get; }

    RotationType DisplayRotation { get; }
}