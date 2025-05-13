using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Leds;
using Meadow.Units;

namespace Meadow.Blazor.Services
{
    public class SensorViewModel : IDisposable
    {
        private readonly ILed _led;
        private readonly Bme680 _bme680;

        public string TemperatureValue { get; private set; } = "0°C";
        public string HumidityValue { get; private set; } = "0%";
        public string PressureValue { get; private set; } = "0atm";

        public event Action? StateChanged;

        public SensorViewModel()
        {
            _led = Resolver.Services.Get<ILed>() ?? throw new Exception("LED not found");

            _bme680 = Resolver.Services.Get<Bme680>() ?? throw new Exception("BME68x not found");
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
            _bme680?.StopUpdating();
        }
    }
}
