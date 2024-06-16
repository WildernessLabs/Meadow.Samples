using GalleryViewer.Core.Contracts;
using System;

namespace GalleryViewer.Core;

public class InputController
{
    public event EventHandler? UnitDownRequested;
    public event EventHandler? UnitUpRequested;

    public InputController(IGalleryViewerHardware platform)
    {
        if (platform.LeftButton is { } ub)
        {
            ub.PressStarted += (s, e) => UnitDownRequested?.Invoke(this, EventArgs.Empty);
        }
        if (platform.RightButton is { } db)
        {
            db.PressStarted += (s, e) => UnitDownRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
