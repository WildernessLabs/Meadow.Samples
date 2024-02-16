using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace BatteryLevel
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override async Task Run()
        {
            while (true)
            {

                Resolver.Log.Info($"Battery Voltage: {Device.GetBatteryInfo().Voltage.Value.Volts:N2}V");

                await Task.Delay(3000);
            }
        }
    }
}