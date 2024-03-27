using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Accelerometers;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Light;
using Meadow.Units;
using MeadowConnectedSample.Connectivity;
using System;
using System.Threading.Tasks;

namespace MeadowConnectedSample.Controller
{
    public class MainController
    {
        private static readonly Lazy<MainController> instance =
            new Lazy<MainController>(() => new MainController());
        public static MainController Instance => instance.Value;

        private Bme688 environmentalSensor;

        private ILightSensor lightSensor;

        private Bmi270 motionSensor;

        public bool UseWiFi { get; set; } = true;

        public (Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance) EnvironmentalReading { get; private set; }

        public Illuminance? LightReading { get; private set; }

        public (Acceleration3D? acceleration3D, AngularVelocity3D? angularVelocity3D, Temperature? temperature) MotionReading { get; private set; }

        public MainController() { }

        public void Initialize(IProjectLabHardware hardware)
        {
            lightSensor = hardware.LightSensor;
            motionSensor = (hardware as ProjectLabHardwareBase).MotionSensor;
            environmentalSensor = (hardware as ProjectLabHardwareBase).AtmosphericSensor;
        }

        public async Task StartUpdating(TimeSpan updateInterval)
        {
            while (true)
            {
                EnvironmentalReading = await environmentalSensor.Read();
                LightReading = await lightSensor.Read();
                MotionReading = await motionSensor.Read();

                Resolver.Log.Info($"" +
                    $"Temperature: {EnvironmentalReading.Temperature.Value.Celsius} | " +
                    $"Light: {LightReading.Value.Lux} | " +
                    $"Motion: ({MotionReading.angularVelocity3D.Value.X},{MotionReading.angularVelocity3D.Value.Y},{MotionReading.angularVelocity3D.Value.Z}) ");

                if (!UseWiFi)
                {
                    BluetoothServer.Instance.SetEnvironmentalCharacteristicValue(EnvironmentalReading);
                    BluetoothServer.Instance.SetLightCharacteristicValue(LightReading);
                    BluetoothServer.Instance.SetMotionCharacteristicValue(MotionReading);
                }

                await Task.Delay(updateInterval);
            }
        }
    }
}