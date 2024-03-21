using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Motors;
using Meadow.Peripherals.Leds;
using System.Threading.Tasks;

namespace MeadowLedRover
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        Led up, down, left, right;
        CarController carController;

        public override Task Initialize()
        {
            var led = new RgbLed(
                Device.Pins.OnboardLedRed, 
                Device.Pins.OnboardLedGreen, 
                Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLedColors.Red);

            up = new Led(Device.Pins.D13);
            down = new Led(Device.Pins.D10);
            left = new Led(Device.Pins.D11);
            right = new Led(Device.Pins.D12);
            up.IsOn = down.IsOn = left.IsOn = right.IsOn = false;

            var motorLeft = new HBridgeMotor
            (   
                a1Pin: Device.Pins.D07,
                a2Pin: Device.Pins.D08,
                enablePin: Device.Pins.D09
            );
            var motorRight = new HBridgeMotor
            (
                a1Pin: Device.Pins.D02,
                a2Pin: Device.Pins.D03,
                enablePin: Device.Pins.D04
            );

            carController = new CarController(motorLeft, motorRight);

            led.SetColor(RgbLedColors.Green);

            return base.Initialize();
        }

        public override async Task Run()
        {
            while (true)
            {
                up.IsOn = true;
                carController.MoveForward();
                await Task.Delay(1000);
                up.IsOn = false;

                carController.Stop();
                await Task.Delay(500);

                down.IsOn = true;
                carController.MoveBackward();
                await Task.Delay(1000);
                down.IsOn = false;

                carController.Stop();
                await Task.Delay(500);

                left.IsOn = true;
                carController.TurnLeft();
                await Task.Delay(1000);
                left.IsOn = false;

                carController.Stop();
                await Task.Delay(500);

                right.IsOn = true;
                carController.TurnRight();
                await Task.Delay(1000);
                right.IsOn = false;

                carController.Stop();
                await Task.Delay(500);
            }
        }
    }
}