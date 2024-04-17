using Meadow;
using System;

namespace MeadowBleLed.Controllers;

public class CommandController
{
    public event EventHandler<bool> LedOnValueSet = default!;
    public event EventHandler<bool> LedOffValueSet = default!;
    public event EventHandler<bool> LedBlinkValueSet = default!;
    public event EventHandler<bool> LedPulseValueSet = default!;
    public event EventHandler<bool> LedRunColorsValueSet = default!;

    public CommandController()
    {
        Resolver.Services.Add(this);
    }

    public void FireLedOn()
    {
        LedOnValueSet?.Invoke(this, true);
    }

    public void FireLedOff()
    {
        LedOffValueSet?.Invoke(this, true);
    }

    public void FireLedBlink()
    {
        LedBlinkValueSet?.Invoke(this, true);
    }

    public void FireLedPulse()
    {
        LedPulseValueSet?.Invoke(this, true);
    }

    public void FireLedRunColors()
    {
        LedRunColorsValueSet?.Invoke(this, true);
    }
}