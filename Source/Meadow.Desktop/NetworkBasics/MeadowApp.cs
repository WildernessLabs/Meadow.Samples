using Meadow;
using System.Threading.Tasks;

namespace WiFi_Basics
{
    public class MeadowApp : App<Desktop>
    {
        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }

        public override Task Run()
        {
            Resolver.Log.Info($"Meadow.Windows Network Sample");

            Resolver.Log.Info($"{Device.NetworkAdapters.Count} network adapters detected");
            Resolver.Log.Info($"----------------------------");

            foreach (var adapter in Device.NetworkAdapters)
            {
                Resolver.Log.Info($"  {adapter.Name}  {adapter.IpAddress}");
            }

            return Task.CompletedTask;
        }
    }
}
