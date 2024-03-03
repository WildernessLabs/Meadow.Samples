using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Distance;
using Meadow.Gateways.Bluetooth;

namespace RainWaterMonitor.Hardware
{
    public class LabProtoWaterMonitorHardware : WaterMonitorHardwareBase
    {
        public LabProtoWaterMonitorHardware() : base()
        {
            F7FeatherV2 device = Resolver.Device as F7FeatherV2;
            //---- instantiate the distance sensor
            Logger?.Info("Instantiating distance sensor.");
            DistanceSensor = new MaxBotix(Resolver.Device, device.SerialPortNames.Com1, MaxBotix.SensorType.XL);
            Logger?.Info("Distance sensor up.");
        }
    }
}