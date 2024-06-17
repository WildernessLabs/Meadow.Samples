using GalleryViewer.Core.Contracts;
using System;

namespace GalleryViewer.Core;

public class InputController
{
    public event EventHandler? leftButtonPressed;

    public event EventHandler? rightButtonPressed;

    public InputController(IGalleryViewerHardware platform)
    {
        if (platform.LeftButton is { } leftButton)
        {
            leftButton.PressStarted += (s, e) => leftButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        if (platform.RightButton is { } rightButton)
        {
            rightButton.PressStarted += (s, e) => rightButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}