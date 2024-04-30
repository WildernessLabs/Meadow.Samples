using Meadow;
using Meadow.Foundation.Leds;
using System.Threading.Tasks;

namespace MeadowMapleTemperature.Controllers;

public class LedController
{
    private RgbPwmLed led;

    public LedController()
    {
        Resolver.Services.Add(this);

        led = new RgbPwmLed(
            MeadowApp.Device.Pins.OnboardLedRed,
            MeadowApp.Device.Pins.OnboardLedGreen,
            MeadowApp.Device.Pins.OnboardLedBlue
        );
    }

    public void SetColor(Color color)
    {
        led.SetColor(color);
    }

    public async Task StartBlink(Color color)
    {
        await led.StartBlink(color);
    }
}