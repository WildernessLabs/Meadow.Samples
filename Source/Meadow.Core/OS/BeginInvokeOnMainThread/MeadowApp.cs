using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Threading.Tasks;

namespace BeginInvokeOnMainThread
{
    // this sample illustrates how to use the `BeginInvokeOnMainThread()` method
    // to ensure that code gets queued to run on the main execution thread.
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize hardware...");

            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);

            onboardLed.SetColor(WildernessLabsColors.PearGreen);

            return Task.CompletedTask;
        }


        public override Task Run()
        {
            //Task.Run(() =>
            //{
            //    MeadowOS.BeginInvokeOnMainThread(
            //        () =>
            //        {
            //            for (int i = 0; i < 10; i++)
            //            {
            //                onboardLed.SetColor(WildernessLabsColors.AzureBlue);
            //                Thread.Sleep(1000);
            //                onboardLed.SetColor(WildernessLabsColors.ChileanFire);
            //                Thread.Sleep(1000);
            //            }
            //        });
            //});

            return Task.CompletedTask;
        }
    }
}
