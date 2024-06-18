﻿using AmbientMonitor.Core.Contracts;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;

namespace AmbientMonitor.F7;

internal class AmbientMonitorProjectLabHardware : IAmbientMonitorHardware
{
    private readonly IProjectLabHardware projLab;

    public IButton? LeftButton => projLab.LeftButton;

    public IButton? RightButton => projLab.RightButton;

    public IPixelDisplay? Display => projLab.Display;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public INetworkAdapter? NetworkAdapter { get; }

    public ITemperatureSensor? TemperatureSensor => projLab.TemperatureSensor;

    public IBarometricPressureSensor? BarometricPressureSensor => projLab.BarometricPressureSensor;

    public IHumiditySensor? HumiditySensor => projLab.HumiditySensor;

    public AmbientMonitorProjectLabHardware(F7CoreComputeV2 device)
    {
        projLab = ProjectLab.Create();

        NetworkAdapter = device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
    }
}