using Meadow;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using Meadow.Peripherals.Leds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowBleLed.Controllers;

public class LedController
{
    IRgbPwmLed rgbPwmLed;

    Task animationTask = null;
    CancellationTokenSource cancellationTokenSource = null;

    public LedController(IPin redPwmPin, IPin greenPwmPin, IPin bluePwmPin)
    {
        rgbPwmLed = new RgbPwmLed(redPwmPin, greenPwmPin, bluePwmPin);
    }

    void Stop()
    {
        rgbPwmLed.StopAnimation();
        cancellationTokenSource?.Cancel();
    }

    public void SetColor(Color color)
    {
        Stop();
        rgbPwmLed.SetColor(color);
    }

    public void TurnOn()
    {
        Stop();
        rgbPwmLed.SetColor(GetRandomColor());
        rgbPwmLed.IsOn = true;
    }

    public void TurnOff()
    {
        Stop();
        rgbPwmLed.IsOn = false;
    }

    public void StartBlink()
    {
        rgbPwmLed.StartBlink(GetRandomColor());
    }

    public void StartPulse()
    {
        rgbPwmLed.StartPulse(GetRandomColor());
    }

    public void StartRunningColors()
    {
        rgbPwmLed.StopAnimation();

        animationTask = new Task(async () =>
        {
            cancellationTokenSource = new CancellationTokenSource();
            await StartRunningColors(cancellationTokenSource.Token);
        });
        animationTask.Start();
    }

    protected async Task StartRunningColors(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            rgbPwmLed.SetColor(GetRandomColor());
            await Task.Delay(1000);
        }
    }

    protected Color GetRandomColor()
    {
        var random = new Random();
        return Color.FromHsba((float)random.NextDouble(), 1, 1);
    }
}