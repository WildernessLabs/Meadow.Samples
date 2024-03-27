using Meadow;
using System.Threading.Tasks;

namespace WifiWeather;

public class MeadowApp : App<RaspberryPi>
{
    private MainController mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        mainController = new MainController();
        mainController.Initialize();

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        await mainController.Run();
    }
}