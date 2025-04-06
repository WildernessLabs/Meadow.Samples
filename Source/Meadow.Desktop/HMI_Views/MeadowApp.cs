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
        Device.Display.Resize(320, 240, 3);

        // Default WinForms screen (800,600)
        //_display = new WinFormsDisplay();
        //var view = new RotatingCube(_display);

        // Screen size of a ILI9488 display
        //Device.Display.Resize(250, 122);
        //var view = new GnssTrackerConnectivityView(Device.Display);

        // Screen size of a EPD4IN2bV2 e-paper display
        //Device.Display.Resize(300, 400);
        //var view = new HomeWidget(Device.Display);

        // Screen size of a ILI9341 display
        //var view = new CultivarView(Device.Display);
        //var view = new ProjectLabDemoView(Device.Display);
        //var view = new AtmosphericHMI(Device.Display);
        //var view = new WifiWeatherV2(Device.Display);
        //var view = new HistogramView(Device.Display);
        //var view = new DockLayoutView(Device.Display);
        //var view = new StackLayoutView(Device.Display);
        var view = new GridLayoutView(Device.Display);

        _ = Task.Run(() =>
        {
            Thread.Sleep(2000);
            _ = view.Run();
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