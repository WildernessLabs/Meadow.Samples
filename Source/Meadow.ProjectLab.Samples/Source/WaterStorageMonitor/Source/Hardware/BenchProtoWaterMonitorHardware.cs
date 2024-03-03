using Meadow;
using Meadow.Foundation.Sensors.Distance;

namespace RainWaterMonitor.Hardware
{
    public class BenchProtoWaterMonitorHardware : WaterMonitorHardwareBase
    {
        public BenchProtoWaterMonitorHardware() : base()
        {
            //---- instantiate the distance sensor
            Logger?.Info("Instantiating distance sensor.");
            DistanceSensor = new Vl53l0x(Resolver.Device, base.Hardware.I2cBus);
            Logger?.Info("Distance sensor up.");
        }
    }
}