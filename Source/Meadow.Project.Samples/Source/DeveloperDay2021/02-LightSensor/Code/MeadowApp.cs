using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Light;
using Meadow.Gateway.WiFi;

namespace LightSensor
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        /// <summary>
        /// TSL2591 light sensor.
        /// </summary>
        Tsl2591 _sensor;

        /// <summary>
        /// Provide logging to Adafruit.IO
        /// </summary>
        AdafruitLogger _adafruitLogger;

        /// <summary>
        /// Default cnstructor.
        /// </summary>
        public MeadowApp()
        {
            ConnectToNetwork().Wait();

            var i2c = Device.CreateI2cBus();
            _sensor = new Tsl2591(i2c);
            _sensor.Updated += Sensor_Updated;

            _adafruitLogger = new AdafruitLogger(Secrets.APIO_USER_NAME, Secrets.APIO_KEY);
            //
            //  Take a new sensor reading every 30 seconds.  Note that Adafruit IO has limits on the
            //  number of readings (per minute) that can be sent to their free service.
            //
            _sensor.StartUpdating(TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Process the latest sensor data.
        /// </summary>
        private void Sensor_Updated(object sender, IChangeResult<(Meadow.Units.Illuminance? FullSpectrum, Meadow.Units.Illuminance? Infrared, Meadow.Units.Illuminance? VisibleLight, Meadow.Units.Illuminance? Integrated)> result)
        {
            float fullSpectrum = (float) result.New.FullSpectrum?.Lux;
            Console.WriteLine($"  Full Spectrum Light: {fullSpectrum:N2}Lux");
            Console.WriteLine($"  Infrared Light: {result.New.Infrared?.Lux:N2}Lux");
            Console.WriteLine($"  Visible Light: {result.New.VisibleLight?.Lux:N2}Lux");
            Console.WriteLine($"  Integrated Light: {result.New.Integrated?.Lux:N2}Lux");
            try
            {
                _adafruitLogger.Send(Secrets.APIO_FEED_NAME, fullSpectrum);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Connect to the WiFi network using the credentials in the Secrets class.
        /// </summary>
        public async Task ConnectToNetwork()
        {
            Console.WriteLine("Connecting to WiFi.");
            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }
            Console.WriteLine("Connection complete.");
        }
    }
}
