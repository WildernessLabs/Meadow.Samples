using Meadow.Devices;
using Meadow.Foundation.Grove.Relays;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;

namespace ProjectLab_Command.Hardware;

internal class MeadowCloudCommandHardware : IMeadowCloudCommandHardware
{
    protected IProjectLabHardware ProjLab { get; }

    public IPixelDisplay Display { get; set; }

    public IRgbPwmLed RgbPwmLed { get; set; }

    public FourChannelSpdtRelay FourChannelRelay { get; set; }

    public MeadowCloudCommandHardware(IProjectLabHardware projLab)
    {
        ProjLab = projLab;
    }

    public void Initialize()
    {
        Display = ProjLab.Display;

        RgbPwmLed = ProjLab.RgbLed;

        FourChannelRelay = new FourChannelSpdtRelay(ProjLab.Qwiic.I2cBus, 0x11);
    }
}