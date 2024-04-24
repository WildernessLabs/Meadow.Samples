using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Sensors.Motion;
using System.Diagnostics;
using System.Threading.Tasks;

public class MeadowApp : App<Desktop>
{
    private FtdiExpander _expander;
    private Mpu6050 _mpu;

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }

    public override Task Initialize()
    {
        Resolver.Log.Info("Checking for FT232H-compatible expander...");

        if (FtdiExpanderCollection.Devices.Count == 0)
        {
            Resolver.Log.Info($"No expanders found!");
            return base.Initialize();
        }

        Resolver.Log.Info($"{FtdiExpanderCollection.Devices.Count} expanders found");

        _expander = FtdiExpanderCollection.Devices[0];

        Resolver.Log.Info("Creating Outputs");

        var bus = _expander.CreateI2cBus();

        _mpu = new Mpu6050(bus);
        _mpu.Updated += _mpu_TemperatureUpdated;
        _mpu.StartUpdating();

        return base.Initialize();
    }

    private void _mpu_TemperatureUpdated(object? sender, IChangeResult<(Meadow.Units.Acceleration3D? Acceleration3D, Meadow.Units.AngularVelocity3D? AngularVelocity3D, Meadow.Units.Temperature? Temperature)> e)
    {
        Debug.WriteLine($"Temp: {e.New.Temperature.Value.Fahrenheit}");
    }

    public override async Task Run()
    {
    }
}