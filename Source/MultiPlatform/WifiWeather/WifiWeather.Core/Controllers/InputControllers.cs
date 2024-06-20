using System;
using WifiWeather.Core.Contracts;

namespace WifiWeather.Core.Controllers;

public class InputController
{
    public event EventHandler? UpButtonPressed;

    public event EventHandler? DownButtonPressed;

    public InputController(IWifiWeatherHardware platform)
    {
        if (platform.UpButton is { } upButton)
        {
            upButton.PressStarted += (s, e) => UpButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        if (platform.DownButton is { } downButton)
        {
            downButton.PressStarted += (s, e) => DownButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}