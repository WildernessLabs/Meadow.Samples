using WiFinder.Core.Contracts;

namespace WiFinder.Core;

public class InputController
{
    public event EventHandler? UpButtonPressed;
    public event EventHandler? DownButtonPressed;
    public event EventHandler? SelectButtonPressed;
    public event EventHandler? BackButtonPressed;

    public InputController(IWiFinderHardware platform)
    {
        if (platform.UpButton is { } ub)
        {
            ub.PressStarted += (s, e) => UpButtonPressed?.Invoke(this, EventArgs.Empty);
        }
        if (platform.DownButton is { } db)
        {
            db.PressStarted += (s, e) => DownButtonPressed?.Invoke(this, EventArgs.Empty);
        }
        if (platform.LeftButton is { } lb)
        {
            lb.PressStarted += (s, e) => BackButtonPressed?.Invoke(this, EventArgs.Empty);
        }
        if (platform.RightButton is { } rb)
        {
            rb.PressStarted += (s, e) => SelectButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}
