using Meadow.Foundation.Sensors.Temperature;
using Meadow.Units;
using System;

namespace MeadowBleTemperature.Controllers
{
    public class TemperatureController
    {
        private static readonly Lazy<TemperatureController> instance =
            new Lazy<TemperatureController>(() => new TemperatureController());
        public static TemperatureController Instance => instance.Value;

        public event EventHandler<Temperature> TemperatureUpdated = delegate { };

        AnalogTemperature analogTemperature;

        private TemperatureController()
        {
            Initialize();
        }

        private void Initialize()
        {
            analogTemperature = new AnalogTemperature(MeadowApp.Device.Pins.A01,
                AnalogTemperature.KnownSensorType.LM35);
            analogTemperature.Updated += AnalogTemperatureUpdated;
        }

        void AnalogTemperatureUpdated(object sender, Meadow.IChangeResult<Temperature> e)
        {
            TemperatureUpdated.Invoke(this, e.New);
        }

        public void StartUpdating(TimeSpan timeSpan)
        {
            analogTemperature.StartUpdating(timeSpan);
        }
    }
}