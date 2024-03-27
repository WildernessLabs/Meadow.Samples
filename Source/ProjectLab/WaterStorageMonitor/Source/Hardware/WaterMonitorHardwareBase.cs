using Meadow;
using Meadow.Devices;
using Meadow.Peripherals.Sensors;

namespace RainWaterMonitor.Hardware
{
    public class WaterMonitorHardwareBase : ProjectLab, IWaterMonitorHardware
    {
        public IRangeFinder DistanceSensor { get; protected set; }

        public WaterMonitorHardwareBase()
        {
        }
    }
}