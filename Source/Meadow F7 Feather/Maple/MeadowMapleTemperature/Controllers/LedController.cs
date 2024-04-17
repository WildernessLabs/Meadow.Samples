using Meadow;
using Meadow.Foundation.Leds;
using System;
using System.Threading.Tasks;

namespace MeadowMapleTemperature.Controllers;

public class LedController
{
    RgbPwmLed led;

    private static readonly Lazy<LedController> instance =
        new Lazy<LedController>(() => new LedController());
    public static LedController Instance => instance.Value;

    private LedController()
    {
        Initialize();
    }

    private void Initialize()
    {
        led = new RgbPwmLed(
            MeadowApp.Device.Pins.OnboardLedRed,
            MeadowApp.Device.Pins.OnboardLedGreen,
            MeadowApp.Device.Pins.OnboardLedBlue
        );
    }

    public async Task SetColor(Color color)
    {
        await led.StopAnimation();
        led.SetColor(color);
    }

    public async Task StartBlink(Color color)
    {
        await led.StopAnimation();
        await led.StartBlink(color);
    }
}
