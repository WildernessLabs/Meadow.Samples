using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace OS_Telemetry;

public class FeatherV2App : MeadowApp<F7FeatherV2> { }
public class CoreComputeApp : MeadowApp<F7CoreComputeV2> { }

public class MeadowApp<T> : App<T>
    where T : F7MicroBase
{
    public override async Task Run()
    {
        Resolver.Log.Info("===== Meadow OS Telemetry =====");
        var i = 0;

        while (true)
        {
            if (Device.PlatformOS.GetCpuTemperature().Celsius < 0)
            {
                Resolver.Log.Info($"   BAD TEMP: {Device.PlatformOS.GetCpuTemperature()}C");
            }

            if (i++ % 10 == 0)
            {
                Resolver.Log.Info($"   Temp: {Device.PlatformOS.GetCpuTemperature()}C");
            }

            /*
            var memoryInfo = (Device.PlatformOS as F7PlatformOS)?.GetMemoryAllocationInfo();
            var gcAlloc = GC.GetTotalMemory(false);

            if (memoryInfo.HasValue)
            {
                Resolver.Log.Info($" Memory");
                Resolver.Log.Info($"   Total memory: {memoryInfo.Value.Arena:n0}");
                Resolver.Log.Info($"   Total allocated: {memoryInfo.Value.TotalAllocated:n0}");
                Resolver.Log.Info($"   Total free: {memoryInfo.Value.TotalFree:n0}");
                Resolver.Log.Info($"   GC Allocated: {gcAlloc:n0}");
            }


            var load = Device.PlatformOS?.GetProcessorUtilization().Average();
            Resolver.Log.Info($" Processor");
            Resolver.Log.Info($"   Usage: {load}%");
            Resolver.Log.Info($"   Temp: {Device.PlatformOS.GetCpuTemperature()}C");

            Resolver.Log.Info($" Storage");
            foreach (var drive in Device.PlatformOS?.FileSystem.Drives)
            {
                Resolver.Log.Info($"  {drive.Name}");
                Resolver.Log.Info($"    {drive.SpaceAvailable.MegaBytes:0.00}/{drive.Size.MegaBytes:0.0}MB");
            }
            */
            await Task.Delay(500);
        }
    }
}