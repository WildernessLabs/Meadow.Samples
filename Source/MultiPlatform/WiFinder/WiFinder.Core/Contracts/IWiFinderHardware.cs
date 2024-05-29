using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;

namespace WiFinder.Core.Contracts;

public interface IWiFinderHardware
{
    // basic hardware
    IButton? LeftButton { get; }
    IButton? RightButton { get; }
    IButton? UpButton { get; }
    IButton? DownButton { get; }

    // complex hardware
    IPixelDisplay Display { get; }
    RotationType DisplayRotation { get; }

    // platform-dependent services
    INetworkController NetworkController { get; }
}
