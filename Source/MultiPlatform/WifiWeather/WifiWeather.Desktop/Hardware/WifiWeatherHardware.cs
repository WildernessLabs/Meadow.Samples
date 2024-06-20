using Meadow;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using WifiWeather.Core.Contracts;

namespace WifiWeather.DesktopApp.Hardware;

public class WifiWeatherHardware : IWifiWeatherHardware
{
    private readonly Desktop device;
    private readonly IButton? upButton;
    private readonly IButton? downButton;
    private readonly INetworkAdapter? networkAdapter;

    public IButton? UpButton => upButton;

    public IButton? DownButton => downButton;

    public IPixelDisplay? Display => device.Display;

    public RotationType DisplayRotation => RotationType.Default;

    public INetworkAdapter? NetworkAdapter => networkAdapter;

    public WifiWeatherHardware(Desktop device)
    {
        this.device = device;

        var keyboard = new Keyboard();

        upButton = new PushButton(keyboard.Pins.Up.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));

        downButton = new PushButton(keyboard.Pins.Down.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));

        if (MeadowApp.Device.NetworkAdapters.Count > 0)
        {
            networkAdapter = MeadowApp.Device.NetworkAdapters[0];
        }
    }
}