using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace Touchscreen_Demo;

public class MeadowAppDesktop : App<Desktop>
{
    private CircleDemo _demo;

    public override Task Initialize()
    {
        var touchscreen = Device.Display as ITouchScreen;
        var display = new DisplayScreen(
                    Device.Display,
                    touchScreen: touchscreen);

        _demo = new CircleDemo(display, touchscreen);

        return base.Initialize();
    }

    public override Task Run()
    {
        _demo.Start();

        // NOTE: this will not return until the display is closed
        ExecutePlatformDisplayRunner();

        return Task.CompletedTask;
    }

    private void ExecutePlatformDisplayRunner()
    {
        (Device.Display as SilkDisplay).Run();
    }
}