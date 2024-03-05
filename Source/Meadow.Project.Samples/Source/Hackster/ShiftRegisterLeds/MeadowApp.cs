using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using System.Threading;
using System.Threading.Tasks;

namespace ShiftRegisterLeds
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        x74595 shiftRegister;

        public override Task Initialize() 
        {
            var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            shiftRegister = new x74595(
                spiBus: Device.CreateSpiBus(),
                pinChipSelect: Device.Pins.D03,
                pins: 8);

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        public override Task Run()
        {
            shiftRegister.Clear(true);

            while (true)
            {
                shiftRegister.Clear();
                for(int i = 0; i < shiftRegister.Pins.AllPins.Count; i++)
                {
                    shiftRegister.WriteToPin(shiftRegister.Pins.AllPins[i], true);
                    Thread.Sleep(500);
                    shiftRegister.WriteToPin(shiftRegister.Pins.AllPins[i], false);
                }
            }
        }
    }
}