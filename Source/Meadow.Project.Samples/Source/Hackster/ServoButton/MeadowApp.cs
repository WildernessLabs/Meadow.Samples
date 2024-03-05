using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Servos;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;
using AU = Meadow.Units.Angle.UnitType;

namespace ServoButton
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        Servo servo;
        PushButton button;

        Angle ANGLE_ZERO = new Angle(0, AU.Degrees);
        Angle ANGLE_NINETY = new Angle(90, AU.Degrees);

        public override Task Initialize() 
        {
            var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            servo = new Servo(Device.Pins.D10, NamedServoConfigs.SG90);
            servo.RotateTo(NamedServoConfigs.SG90.MinimumAngle);

            button = new PushButton(Device.Pins.D04);
            button.Clicked += ButtonClicked;

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        async void ButtonClicked(object sender, EventArgs e)
        {
            servo.RotateTo(ANGLE_ZERO);
            await Task.Delay(1000);
            servo.RotateTo(ANGLE_NINETY);
        }
    }
}