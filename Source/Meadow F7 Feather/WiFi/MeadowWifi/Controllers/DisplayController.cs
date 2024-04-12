using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using System;

namespace MeadowWifi;

public class DisplayControllers
{
    private static readonly Lazy<DisplayControllers> instance =
        new Lazy<DisplayControllers>(() => new DisplayControllers());
    public static DisplayControllers Instance => instance.Value;

    MicroGraphics graphics;

    public DisplayControllers()
    {
        Initialize();
    }

    void Initialize()
    {
        var display = new Ssd1306(
            i2cBus: MeadowApp.Device.CreateI2cBus(),
            displayType: Ssd130xBase.DisplayType.OLED128x64);

        graphics = new MicroGraphics(display)
        {
            Stroke = 1,
            CurrentFont = new Font8x12(),
        };

        UpdateStatus();
    }

    public void UpdateStatus()
    {
        graphics.Clear();
        graphics.DrawText(3, 3, "Bluetooth:");
        graphics.DrawText(7, 18, "Not Paired");
        graphics.DrawText(3, 33, "WIFI:");
        graphics.DrawText(7, 48, "Disconnected");
        graphics.Show();
    }

    public void UpdateBluetoothStatus(string status)
    {
        graphics.DrawRectangle(3, 18, 122, 12, false, true);
        graphics.DrawText(7, 18, status);
        graphics.Show();
    }

    public void UpdateWifiStatus(string status)
    {
        graphics.DrawRectangle(3, 48, 122, 12, false, true);
        graphics.DrawText(7, 48, status);
        graphics.Show();
    }
}