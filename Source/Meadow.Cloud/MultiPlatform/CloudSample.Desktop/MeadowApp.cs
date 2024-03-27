using Meadow;

namespace CloudSample;

public class MeadowApp : App<Desktop>
{
    private MainController mainController;

    public override Task Initialize()
    {
        //Resolver.Log.ShowGroups.Add("Cloud");

        mainController = new MainController();

        return base.Initialize();
    }

    public override Task Run()
    {
        mainController.Start();

        return base.Run();
    }
}
