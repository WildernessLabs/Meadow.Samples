using Meadow;
using Meadow.Foundation.Sensors.Atmospheric;
using System.Threading.Tasks;

namespace Bme280_Sample;

public class MeadowApp : App<RaspberryPi>
{
    private Bme280? _bme;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initializing...");

        // Note: raspberry pi doesn't have a Bus 0
        var bus = Device.CreateI2cBus(1);

        _bme = new Bme280(bus);

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        while (true)
        {
            // we could also use the driver's internal sampling instead
            var data = await _bme.Read();

            Resolver.Log.Info($"-- Conditions --");
            Resolver.Log.Info($"Temp: {data.Temperature?.Fahrenheit}F");
            Resolver.Log.Info($"Hum:  {data.Humidity?.Percent}%");
            Resolver.Log.Info($"Press: {data.Pressure?.Millibar}mb");

            await (Task.Delay(1000));
        }
    }
}