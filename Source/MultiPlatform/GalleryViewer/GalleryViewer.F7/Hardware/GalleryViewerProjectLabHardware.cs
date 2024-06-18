﻿using GalleryViewer.Core.Contracts;
using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;

namespace GalleryViewer.F7;

internal class GalleryViewerProjectLabHardware : IGalleryViewerHardware
{
    private readonly IProjectLabHardware projLab;

    public IPixelDisplay? Display => projLab.Display;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public IButton? LeftButton => projLab.LeftButton;

    public IButton? RightButton => projLab.RightButton;

    public GalleryViewerProjectLabHardware(F7CoreComputeV2 device)
    {
        projLab = ProjectLab.Create();
    }
}