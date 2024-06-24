using MagicEightMeadow.Hardware;
using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace MagicEightMeadow;

public class MeadowApp : ProjectLabCoreComputeApp
{
    private MainController mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new MagicEightMeadowHardware(Hardware);

        mainController = new MainController(hardware);
        mainController.Initialize();

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        mainController.Run();

        return Task.CompletedTask;
    }
}