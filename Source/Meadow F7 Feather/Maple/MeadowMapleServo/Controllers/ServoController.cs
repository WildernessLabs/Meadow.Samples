using Meadow;
using Meadow.Foundation.Servos;
using Meadow.Units;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowMapleServo.Controllers;

public class ServoController
{
    private Servo servo;

    private Task animationTask = null;
    private CancellationTokenSource cancellationTokenSource = null;

    protected int _rotationAngle;

    public ServoController()
    {
        Resolver.Services.Add(this);

        servo = new Servo(
            pwmPin: MeadowApp.Device.Pins.D10,
            config: NamedServoConfigs.SG90);
    }

    public void RotateTo(Angle angle)
    {
        _ = servo.RotateTo(angle);
    }

    public void StopSweep()
    {
        cancellationTokenSource?.Cancel();
    }

    public void StartSweep()
    {
        animationTask = new Task(async () =>
        {
            cancellationTokenSource = new CancellationTokenSource();
            await StartSweep(cancellationTokenSource.Token);
        });
        animationTask.Start();
    }
    protected async Task StartSweep(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested) { break; }

            while (_rotationAngle < 180)
            {
                if (cancellationToken.IsCancellationRequested) { break; }

                _rotationAngle++;
                _ = servo.RotateTo(new Angle(_rotationAngle, Angle.UnitType.Degrees));
                await Task.Delay(50);
            }

            while (_rotationAngle > 0)
            {
                if (cancellationToken.IsCancellationRequested) { break; }

                _rotationAngle--;
                _ = servo.RotateTo(new Angle(_rotationAngle, Angle.UnitType.Degrees));
                await Task.Delay(50);
            }
        }
    }
}