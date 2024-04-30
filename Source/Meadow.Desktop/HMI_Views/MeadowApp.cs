using HMI_Views.Views;
using Meadow;
using Meadow.Foundation.Displays;
using System.Threading;
using System.Threading.Tasks;

namespace HMI_Views;

public class MeadowApp : App<Desktop>
{
    public override Task Initialize()
    {
        // Default WinForms screen (800,600)
        //_display = new WinFormsDisplay();
        //var views = new RotatingCube(_display);

        // Screen size of a ILI9488 display
        Device.Display.Resize(250, 122);
        var views = new GnssTrackerConnectivityView(Device.Display);

        // Screen size of a EPD4IN2bV2 e-paper display
        //Device.Display.Resize(300, 400);
        //var views = new HomeWidget(Device.Display);

        // Screen size of a ILI9341 display
        //var views = new CultivarView(Device.Display);
        //var views = new ProjectLabDemoView(Device.Display);
        //var views = new AtmosphericHMI(Device.Display);
        //var views = new WifiWeatherV2(Device.Display);

        _ = Task.Run(() =>
        {
            Thread.Sleep(2000);
            views.Run();
        });

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        // NOTE: this will not return until the display is closed
        ExecutePlatformDisplayRunner();

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