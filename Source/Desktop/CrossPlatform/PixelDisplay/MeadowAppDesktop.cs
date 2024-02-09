using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;

public class Program
{
    public static async Task Main(string[] args)
    {
#if WINDOWS
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();
#endif
        await MeadowOS.Start(args);
    }
}

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
        var screen = new DisplayScreen(Device.Display);

        var title = new Meadow.Foundation.Graphics.MicroLayout.Label(0, (screen.Height - 15) / 2, screen.Width, 30)
        {
            Font = new Font12x20(),
            TextColor = Meadow.Color.LightBlue,
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

    private void ExecutePlatformDisplayRunner()
    {
#if WINDOWS
        System.Windows.Forms.Application.Run(Device.Display as System.Windows.Forms.Form);
#endif
        if (Device.Display is GtkDisplay gtk)
        {
            gtk.Run();
        }
    }
}
