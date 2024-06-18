using AmbientMonitor.Core.Contracts;
using System;

namespace AmbientMonitor.Core.Controllers;

public class InputController
{
    public event EventHandler? leftButtonPressed;

    public event EventHandler? rightButtonPressed;

    public InputController(IAmbientMonitorHardware hardware)
    {
        if (hardware.LeftButton is { } leftButton)
        {
            leftButton.PressStarted += (s, e) => leftButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        if (hardware.RightButton is { } rightButton)
        {
            rightButton.PressStarted += (s, e) => rightButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}
