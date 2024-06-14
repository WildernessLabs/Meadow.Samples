using Meadow;
using Meadow.Foundation.Displays;
using System.Threading.Tasks;
using WifiWeather.Core;

namespace WifiWeather.RPi;

internal class wifiweatherApp : App<RaspberryPi>
{
    private WifiWeatherHardware hardware;
    private MainController? mainController;

    public bool SupportDisplay { get; set; } = false;

    public override Task Initialize()
    {
        hardware = new WifiWeatherHardware(Device, SupportDisplay);
        mainController = new MainController(hardware);

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        if (hardware.Display is GtkDisplay gtk)
        {
            _ = mainController.Run();
            gtk.Run();
        }

        mainController!.Run();

        return Task.CompletedTask;
    }
}