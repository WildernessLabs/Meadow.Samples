using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors.Buttons;

namespace Meadow.Cloud_Client.Hardware;

internal interface IMeadowCloudClientHardware
{
    public IPixelDisplay Display { get; }

    public IButton RightButton { get; }

    public IButton LeftButton { get; }

    public IRgbPwmLed RgbPwmLed { get; }

    public void Initialize();
}