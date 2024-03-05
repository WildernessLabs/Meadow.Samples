using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Light;
using Meadow.Foundation.Sensors.Atmospheric;

namespace LightSensor
{
    public class MeadowApp2 : App<F7Micro, MeadowApp2>
    {
        /// <summary>
        /// TSL2591 light sensor.
        /// </summary>
        Tsl2591 _lightSensor;

        /// <summary>
        /// BME280 Atmospheric (temperaure, humidity and air pressure) sensor.
        /// </summary>
        Bme280 _atmosphericSensor;

        /// <summary>
        /// Default cnstructor.
        /// </summary>
        public MeadowApp2()
        {
            var i2c = Device.CreateI2cBus();
            _lightSensor = new Tsl2591(i2c);
            _lightSensor.Updated += LightSensor_Updated;

            _atmosphericSensor = new Bme280(i2c, (byte) Bme280.Addresses.Address1);
            //
            //  Take a new sensor reading every 30 seconds.  Note that Adafruit IO has limits on the
            //  number of readings (per minute) that can be sent to their free service.
            //
            _lightSensor.StartUpdating(TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Process the latest sensor data.
        /// </summary>
        private void LightSensor_Updated(object sender, IChangeResult<(Meadow.Units.Illuminance? FullSpectrum, Meadow.Units.Illuminance? Infrared, Meadow.Units.Illuminance? VisibleLight, Meadow.Units.Illuminance? Integrated)> result)
        {
            float fullSpectrum = (float) result.New.FullSpectrum?.Lux;
            Console.WriteLine($"  Full Spectrum Light: {fullSpectrum:N2} Lux");
            Console.WriteLine($"  Infrared Light: {result.New.Infrared?.Lux:N2} Lux");
            Console.WriteLine($"  Visible Light: {result.New.VisibleLight?.Lux:N2} Lux");
            Console.WriteLine($"  Integrated Light: {result.New.Integrated?.Lux:N2} Lux");

            ReadAtmosphericConditions().Wait();
        }

        /// <summary>
        /// Read the atmospheric sensor.
        /// </summary>
        private async Task ReadAtmosphericConditions()
        {
            var conditions = await _atmosphericSensor.Read();
            Console.WriteLine("Atmospheric Readings:");
            Console.WriteLine($"  Temperature: {conditions.Temperature?.Celsius:N2}C");
            Console.WriteLine($"  Pressure: {conditions.Pressure?.Bar:N2}hPa");
            Console.WriteLine($"  Relative Humidity: {conditions.Humidity?.Percent:N2}%");
        }
    }
}
