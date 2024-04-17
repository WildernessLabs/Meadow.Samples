using Meadow;
using Meadow.Devices;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace McpLeds;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    List<Led> leds;
    Mcp23008 mcp;

    public override Task Initialize()
    {
        var onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue);
        onboardLed.SetColor(Color.Red);

        mcp = new Mcp23008(Device.CreateI2cBus());

        leds = new List<Led>
        {
            new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP0)),
            new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP1)),
            new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP2)),
            new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP3)),
            new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP4)),
            new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP5)),
            new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP6)),
            new Led(mcp.CreateDigitalOutputPort(mcp.Pins.GP7))
        };

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }

    public override async Task Run()
    {
        while (true)
        {
            foreach (var led in leds)
            {
                led.IsOn = true;
                await Task.Delay(500);
            }

            await Task.Delay(1000);

            foreach (var led in leds)
            {
                led.IsOn = false;
                await Task.Delay(500);
            }
        }
    }
}