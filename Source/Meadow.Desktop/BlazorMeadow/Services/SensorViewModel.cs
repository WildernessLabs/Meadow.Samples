using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Leds;
using Meadow.Units;

namespace Meadow.Blazor.Services
{
    public class SensorViewModel : IDisposable
    {
        private ILed _led;
        private Bme680? _bme680;
        private System.Timers.Timer? _updateTimer;

        public string TemperatureValue { get; private set; } = "0°C";
        public string HumidityValue { get; private set; } = "0%";
        public string PressureValue { get; private set; } = "0atm";

        public event Action? StateChanged;

        public SensorViewModel()
        {
            Task.Run(InitializeHardware);
        }

        private async Task InitializeHardware()
        {
            while (_led == null || _bme680 == null)
            {
                _bme680 = Resolver.Services.Get<Bme680>();
                _led = Resolver.Services.Get<ILed>();
                await Task.Delay(100);
            }

            _bme680.Updated += Bme680Updated;
            _bme680.StartUpdating(TimeSpan.FromSeconds(2));
        }

        private void Bme680Updated(object? sender, IChangeResult<(Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance)> e)
        {
            _led.IsOn = true;
            Thread.Sleep(1000);

            TemperatureValue = $"{e.New.Temperature?.Celsius:n0}°C";
            HumidityValue = $"{e.New.Humidity?.Percent:n0}%";
            PressureValue = $"{e.New.Pressure?.StandardAtmosphere:n2}atm";

            _led.IsOn = false;

            StateChanged?.Invoke();
        }

        public void Dispose()
        {
            _updateTimer?.Dispose();
            _bme680?.StopUpdating();
        }
    }
}
