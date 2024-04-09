using System;

namespace MeadowConnectedSample.Controllers;

public interface ICommandController
{
    event EventHandler<bool> PairingValueSet;
    event EventHandler<bool> LedToggleValueSet;
    event EventHandler<bool> LedBlinkValueSet;
    event EventHandler<bool> LedPulseValueSet;
}
