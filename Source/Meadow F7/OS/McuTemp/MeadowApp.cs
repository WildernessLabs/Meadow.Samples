using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace McuTemp;

public class MeadowApp : App<F7FeatherV2>
{
    public override async Task Run()
    {
        while (true)
        {
            // get the temp
            Resolver.Log.Info($"Processor Temp: {Device.PlatformOS.GetCpuTemperature().Celsius:n2}C");

            await Task.Delay(1000);
        }
    }
}