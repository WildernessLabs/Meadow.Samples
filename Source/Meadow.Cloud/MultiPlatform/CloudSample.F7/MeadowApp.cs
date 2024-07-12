using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace CloudSample.F7;

public class MeadowApp : App<F7CoreComputeV2>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Platform: Meadow F7");

        mainController = new MainController();

        return base.Initialize();
    }

    public override Task Run()
    {
        mainController?.Start();

        return base.Run();
    }
}