using System;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Distance;
using Meadow.Peripherals.Sensors;

namespace RainWaterMonitor.Hardware
{
    public interface IWaterMonitorHardware : IProjectLabHardware
    {
        IRangeFinder DistanceSensor { get; }
    }
}