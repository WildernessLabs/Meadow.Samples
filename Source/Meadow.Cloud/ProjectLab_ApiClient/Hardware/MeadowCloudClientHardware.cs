using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors.Buttons;

namespace ProjectLab_ApiClient.Hardware;

internal class MeadowCloudClientHardware : IMeadowCloudClientHardware
{
    protected IProjectLabHardware ProjLab { get; }

    public IPixelDisplay Display { get; set; }

    public IButton RightButton { get; set; }

    public IButton LeftButton { get; set; }

    public IRgbPwmLed RgbPwmLed { get; set; }

    public MeadowCloudClientHardware(IProjectLabHardware projLab)
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