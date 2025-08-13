using Meadow;
using Meadow.Devices;
using Meadow.Foundation.MotorControllers.StepperOnline;
using Meadow.Foundation.Motors.StepperOnline;
using Meadow.Peripherals;
using System;
using System.Threading.Tasks;

namespace StepperMotor_Sample;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private F55B150_24GL_30S motor;

    public override async Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        Resolver.Log.Info($"Running on ProjectLab Hardware {Hardware.RevisionString}");

        var client = Hardware.GetModbusRtuClient(BLD510B.DefaultBaudRate);

        BLD510B? controller = null;
        F55B150_24GL_30S? motor = null;
        do
        {
            try
            {
                controller ??= new BLD510B(client, BLD510B.DefaultModbusAddress);
                motor ??= new F55B150_24GL_30S(controller);
                motor.SetSpeed(new Meadow.Units.AngularVelocity(500, Meadow.Units.AngularVelocity.UnitType.RevolutionsPerMinute));
            }
            catch (Exception ex)
            {
                Resolver.Log.Error($"Failed to create motor/controller: {ex.Message}");
                await Task.Delay(2000);
            }
        } while (motor == null || controller == null);

        this.motor = motor;
    }

    public override async Task Run()
    {
        while (true)
        {
            await motor.RunFor(TimeSpan.FromSeconds(5), RotationDirection.Clockwise);
            await motor.Stop();
            await Task.Delay(1000);

            await motor.RunFor(TimeSpan.FromSeconds(5), RotationDirection.CounterClockwise);
            await motor.Stop();
            await Task.Delay(1000);
        }
    }
}