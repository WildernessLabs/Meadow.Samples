using GalleryViewer.Core.Contracts;
using System;

namespace GalleryViewer.Core.Controllers;

public class InputController
{
    public event EventHandler? LeftButtonPressed;

    public event EventHandler? RightButtonPressed;

    public InputController(IGalleryViewerHardware platform)
    {
        if (platform.LeftButton is { } leftButton)
        {
            leftButton.PressStarted += (s, e) => LeftButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        if (platform.RightButton is { } rightButton)
        {
            rightButton.PressStarted += (s, e) => RightButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}