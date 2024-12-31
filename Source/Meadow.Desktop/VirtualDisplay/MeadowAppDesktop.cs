using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using System.Threading.Tasks;

public class MeadowAppDesktop : App<Desktop>
{
    public override Task Initialize()
    {
        Resolver.Log.Info($"Initializing {this.GetType().Name}");
        Resolver.Log.Info($" Platform OS is a {Device.PlatformOS.GetType().Name}");
        Resolver.Log.Info($" Platform: {Device.Information.Platform}");
        Resolver.Log.Info($" OS: {Device.Information.OSVersion}");
        Resolver.Log.Info($" Model: {Device.Information.Model}");
        Resolver.Log.Info($" Processor: {Device.Information.ProcessorType}");

        GenerateLayout();

        return base.Initialize();
    }

    private void GenerateLayout()
    {
        Device.DisplayFactory = () => new VirtualIli9341();

        var screen = new DisplayScreen(Device.Display!);

        var title = new Label(0, (screen.Height - 15) / 2, screen.Width, 30)
        {
            Font = new Font12x20(),
            TextColor = Color.LightBlue,
            Text = "Hello Meadow!"
        };

        screen.Controls.Add(title);
    }

    public override Task Run()
    {
        // NOTE: this will not return until the display is closed
        ExecutePlatformDisplayRunner();

        return Task.CompletedTask;
    }

    public override Task OnShutdown()
    {
        return Task.CompletedTask;
    }

    private void ExecutePlatformDisplayRunner()
    {
        if (Device.Display is SilkDisplay sd)
        {
            sd.Run();
        }
        MeadowOS.TerminateRun();
        System.Environment.Exit(0);
    }
}
