using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace McuTemp
{
    /// <summary>
    /// This sample prints the MCU temp to the console.
    /// </summary>
    public class MeadowApp : App<F7FeatherV2>
    {
        public override async Task Run()
        {
            while (true)
            {
                // get the temp
                //Resolver.Log.Info($"Processor Temp: {Device.GetProcessorTemperature().Celsius:n2}C");

                await Task.Delay(1000);
            }
        }
    }
}