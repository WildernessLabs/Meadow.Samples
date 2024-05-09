using Meadow;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using WiFinder.Core;
using WiFinder.Core.Contracts;

namespace WiFinder.Windows;

internal class DesktopHardware : IWiFinderHardware
{
    private readonly Desktop device;
    private readonly Keyboard keyboard;

    public RotationType DisplayRotation => RotationType.Default;
    public INetworkController? NetworkController { get; }
    public IPixelDisplay Display => device.Display;
    public ITemperatureSensor? TemperatureSensor { get; }
    public IButton? UpButton { get; }
    public IButton? DownButton { get; }
    public IButton? LeftButton { get; }
    public IButton? RightButton { get; }

    public DesktopHardware(Desktop device)
    {
        this.device = device;

        keyboard = new Keyboard();

        var wifi = device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        NetworkController = new NetworkController(wifi!);

        LeftButton = new PushButton(keyboard.Pins.Left.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));
        RightButton = new PushButton(keyboard.Pins.Right.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));
        UpButton = new PushButton(keyboard.Pins.Up.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));
        DownButton = new PushButton(keyboard.Pins.Down.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));

    }
}
