using GalleryViewer.Core.Contracts;
using Meadow;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;

namespace GalleryViewer.DesktopApp.Hardware;

internal class GalleryViewerHardware : IGalleryViewerHardware
{
    private readonly Meadow.Desktop device;
    private readonly Keyboard keyboard;

    public RotationType DisplayRotation => RotationType.Default;

    public IPixelDisplay? Display => device.Display;

    public IButton? RightButton { get; }

    public IButton? LeftButton { get; }

    public GalleryViewerHardware(Meadow.Desktop device)
    {
        this.device = device;

        keyboard = new Keyboard();

        LeftButton = new PushButton(keyboard.Pins.Left.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));
        RightButton = new PushButton(keyboard.Pins.Right.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));
    }
}