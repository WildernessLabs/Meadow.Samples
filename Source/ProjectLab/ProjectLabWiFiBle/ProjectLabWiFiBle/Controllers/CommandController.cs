using Meadow;
using System;

namespace MeadowConnectedSample.Controllers;

public class CommandController
{
    public event EventHandler<bool> PairingValueSet = default!;
    public event EventHandler<bool> LedToggleValueSet = default!;
    public event EventHandler<bool> LedBlinkValueSet = default!;
    public event EventHandler<bool> LedPulseValueSet = default!;

    public CommandController()
    {
        Resolver.Services.Add(this);
    }

    public void FirePairing()
    {
        PairingValueSet?.Invoke(this, true);
    }

    public void FireLedToggle()
    {
        LedToggleValueSet?.Invoke(this, true);
    }

    public void FireLedBlink()
    {
        LedBlinkValueSet?.Invoke(this, true);
    }

    public void FireLedPulse()
    {
        LedPulseValueSet?.Invoke(this, true);
    }
}
