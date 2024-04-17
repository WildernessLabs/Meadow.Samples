using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Motion;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace RotationDetector;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    Led up;
    Led down;
    Led left;
    Led right;
    Mpu6050 mpu;

    public override Task Initialize()
    {
        var onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue);
        onboardLed.SetColor(Color.Red);

        up = new Led(Device.CreateDigitalOutputPort(Device.Pins.D13));
        down = new Led(Device.CreateDigitalOutputPort(Device.Pins.D10));
        left = new Led(Device.CreateDigitalOutputPort(Device.Pins.D11));
        right = new Led(Device.CreateDigitalOutputPort(Device.Pins.D12));

        mpu = new Mpu6050(Device.CreateI2cBus());
        mpu.Updated += MpuUpdated;

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }

    void MpuUpdated(object sender, IChangeResult<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, Temperature? Temperature)> e)
    {
        up.IsOn = e.New.Acceleration3D?.Y.CentimetersPerSecondSquared < -50;
        down.IsOn = e.New.Acceleration3D?.Y.CentimetersPerSecondSquared > 100;
        left.IsOn = e.New.Acceleration3D?.X.CentimetersPerSecondSquared > 50;
        right.IsOn = e.New.Acceleration3D?.X.CentimetersPerSecondSquared < -100;
    }

    public override Task Run()
    {
        mpu.StartUpdating(TimeSpan.FromMilliseconds(100));

        return base.Run();
    }
}