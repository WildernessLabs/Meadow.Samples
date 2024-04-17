using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Peripherals.Sensors.Hid;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace LedJoystick;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    AnalogJoystick joystick;
    Led Up, Down, Left, Right;

    public override Task Initialize()
    {
        var onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue);
        onboardLed.SetColor(Color.Red);

        Up = new Led(Device.Pins.D15); Up.IsOn = true;
        Down = new Led(Device.Pins.D12); Down.IsOn = true;
        Left = new Led(Device.Pins.D13); Left.IsOn = true;
        Right = new Led(Device.Pins.D14); Right.IsOn = true;

        joystick = new AnalogJoystick(
            Device.CreateAnalogInputPort(Device.Pins.A01, 1, TimeSpan.FromMilliseconds(10), new Voltage(3.3)),
            Device.CreateAnalogInputPort(Device.Pins.A00, 1, TimeSpan.FromMilliseconds(10), new Voltage(3.3)),
            null);
        joystick.IsHorizontalInverted = true;
        joystick.IsVerticalInverted = true;

        _ = joystick?.SetCenterPosition();
        joystick.Updated += JoystickUpdated;
        joystick.StartUpdating(TimeSpan.FromMilliseconds(20));

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }

    void JoystickUpdated(object sender, IChangeResult<AnalogJoystickPosition> e)
    {
        switch (joystick.DigitalPosition)
        {
            case DigitalJoystickPosition.Up:
                Up.IsOn = true; Down.IsOn = false; Left.IsOn = false; Right.IsOn = false;
                break;
            case DigitalJoystickPosition.UpRight:
                Up.IsOn = true; Down.IsOn = false; Left.IsOn = false; Right.IsOn = true;
                break;
            case DigitalJoystickPosition.Right:
                Up.IsOn = false; Down.IsOn = false; Left.IsOn = false; Right.IsOn = true;
                break;
            case DigitalJoystickPosition.DownRight:
                Up.IsOn = false; Down.IsOn = true; Left.IsOn = false; Right.IsOn = true;
                break;
            case DigitalJoystickPosition.Down:
                Up.IsOn = false; Down.IsOn = true; Left.IsOn = false; Right.IsOn = false;
                break;
            case DigitalJoystickPosition.DownLeft:
                Up.IsOn = false; Down.IsOn = true; Left.IsOn = true; Right.IsOn = false;
                break;
            case DigitalJoystickPosition.Left:
                Up.IsOn = false; Down.IsOn = false; Left.IsOn = true; Right.IsOn = false;
                break;
            case DigitalJoystickPosition.UpLeft:
                Up.IsOn = true; Down.IsOn = false; Left.IsOn = true; Right.IsOn = false;
                break;
            default:
                Up.IsOn = false; Down.IsOn = false; Left.IsOn = false; Right.IsOn = false;
                break;
        }
    }
}