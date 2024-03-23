using Meadow;
using Meadow.Peripherals.Leds;
using System;
using System.Threading.Tasks;

namespace MeadowConnectedSample.Controllers
{
    public class LedController
    {
        private static readonly Lazy<LedController> instance =
            new Lazy<LedController>(() => new LedController());
        public static LedController Instance => instance.Value;

        IRgbPwmLed rgbPwmLed;
        bool isAnimating;

        private LedController() { }

        public void Initialize(IRgbPwmLed rgbPwmLed)
        {
            this.rgbPwmLed = rgbPwmLed;
        }

        public void SetColor(Color color)
        {
            rgbPwmLed.SetColor(color);
        }

        public async Task Toggle()
        {
            if (rgbPwmLed.IsOn || isAnimating)
            {
                await rgbPwmLed.StopAnimation();
                rgbPwmLed.IsOn = false;
                isAnimating = false;
            }
            else
            {
                await rgbPwmLed.StopAnimation();
                rgbPwmLed.SetColor(GetRandomColor());
                rgbPwmLed.IsOn = true;
            }
        }

        public async Task StartBlink()
        {
            await rgbPwmLed.StopAnimation();
            await rgbPwmLed.StartBlink(GetRandomColor());
            isAnimating = true;
        }

        public async Task StartPulse()
        {
            await rgbPwmLed.StopAnimation();
            await rgbPwmLed.StartPulse(GetRandomColor());
            isAnimating = true;
        }

        protected Color GetRandomColor()
        {
            var random = new Random();
            return new Color(random.Next(256), random.Next(256), random.Next(256));
        }
    }
}