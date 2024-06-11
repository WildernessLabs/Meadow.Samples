using Meadow;

namespace CloudSample;

public class MeadowApp : App<RaspberryPi>
{
    private MainController? mainController;

    public override Task Initialize()
    {
        Resolver.Log.ShowGroups.Add("Platform: Raspberry Pi");

        mainController = new MainController();

        return base.Initialize();
    }

    public override Task Run()
    {
        mainController?.Start();

        return base.Run();
    }
}