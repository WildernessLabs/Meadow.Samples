using Meadow;
using System;

namespace MeadowMapleServo.Controllers;

public class CommandController
{
    public event EventHandler<int> ServoRotateTo = default!;
    public event EventHandler ServoStartSweep = default!;
    public event EventHandler ServoStopSweep = default!;

    public CommandController()
    {
        Resolver.Services.Add(this);
    }

    public void FireServoRotateTo(int degrees)
    {
        ServoRotateTo?.Invoke(this, degrees);
    }

    public void FireServoStartSweep()
    {
        ServoStartSweep?.Invoke(this, null);
    }

    public void FireServoStopSweep()
    {
        ServoStopSweep?.Invoke(this, null);
    }
}