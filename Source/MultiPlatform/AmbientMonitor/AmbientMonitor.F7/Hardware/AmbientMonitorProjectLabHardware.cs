using AmbientMonitor.Core.Contracts;
using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;

namespace AmbientMonitor.F7;

internal class AmbientMonitorProjectLabHardware : IAmbientMonitorHardware
{
    private readonly IProjectLabHardware projLab;

    public IPixelDisplay? Display => projLab.Display;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public ITemperatureSensor? TemperatureSensor => projLab.TemperatureSensor;

    public AmbientMonitorProjectLabHardware(F7CoreComputeV2 device)
    {
        projLab = ProjectLab.Create();
    }
}