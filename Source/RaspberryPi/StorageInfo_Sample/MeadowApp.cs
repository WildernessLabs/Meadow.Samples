using Meadow;

namespace StorageInfo_Sample;

public class MeadowApp : App<RaspberryPi>
{
    public override Task Run()
    {
        Resolver.Log.Info($"\nMeadow file system info");
        Resolver.Log.Info($"-----------------------");

        Resolver.Log.Info($"Meadow root: {Device.PlatformOS.FileSystem.FileSystemRoot}");
        Resolver.Log.Info($"Data dir:    {Device.PlatformOS.FileSystem.DataDirectory}");
        Resolver.Log.Info($"Temp dir:    {Device.PlatformOS.FileSystem.TempDirectory}");


        Resolver.Log.Info($"\n{"Filesystem",-15}{"1-K blocks",-15}{"Available",-15}Drive Name");
        foreach (LinuxStorageInformation drive in Device.PlatformOS.FileSystem.Drives)
        {
            Resolver.Log.Info($"{drive.Filesystem,-15}{drive.Size.KibiBytes,-15}{drive.SpaceAvailable.KibiBytes,-15}{drive.Name}");
        }

        return base.Run();
    }
}