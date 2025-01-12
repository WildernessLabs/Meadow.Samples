using Meadow.Foundation.MotorControllers.StepperOnline;
using Meadow.Foundation.Motors.StepperOnline;
using Meadow.Hardware;
using Meadow.Modbus;

namespace StepperMotor_Sample;

internal class Program
{
    private static async Task Main(string[] _)
    {
        var serialPort = "COM12";
        byte controllerAddress = BLD510B.DefaultModbusAddress;

        using (var port = new SerialPortShim(serialPort, BLD510B.DefaultBaudRate, Parity.None, 8, StopBits.One))
        {
            port.ReadTimeout = TimeSpan.FromSeconds(15);
            port.Open();

            var client = new ModbusRtuClient(port);
            var controller = new BLD510B(client);
            var motor = new F55B150_24GL_30S(controller);
            motor.SetSpeed(new Meadow.Units.AngularVelocity(500, Meadow.Units.AngularVelocity.UnitType.RevolutionsPerMinute));

            while (true)
            {
                await motor.RunFor(TimeSpan.FromSeconds(5), Meadow.Peripherals.RotationDirection.Clockwise);
                await motor.Stop();
                await Task.Delay(1000);
                await motor.RunFor(TimeSpan.FromSeconds(5), Meadow.Peripherals.RotationDirection.CounterClockwise);
                await motor.Stop();
                await Task.Delay(1000);
            }
        }
    }
}