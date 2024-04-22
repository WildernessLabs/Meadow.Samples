using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace GnssTrackerConnectivity.Controllers;

public class DisplayController
{
    // Screen height is 122 but buffer is 128
    private readonly int OFFSET_Y = 6;

    private DisplayScreen displayScreen;

    public DisplayController(IPixelDisplay display)
    {
        displayScreen = new DisplayScreen(display, RotationType._90Degrees)
        {
            BackgroundColor = Color.White
        };
    }

    public void ShowMapleReady(string ipAddress)
    {
        displayScreen.BeginUpdate();

        displayScreen.Controls.Add(new Box(
            0,
            0 + OFFSET_Y,
            displayScreen.Width,
            displayScreen.Height - OFFSET_Y)
        {
            IsFilled = true,
            ForeColor = Color.Red
        });

        displayScreen.Controls.Add(new Box(
            5,
            5 + OFFSET_Y,
            displayScreen.Width - 10,
            displayScreen.Height - 10 - OFFSET_Y)
        {
            IsFilled = true,
            ForeColor = Color.White
        });

        var ble = Image.LoadFromResource("GnssTrackerConnectivity.Resources.img-wifi.bmp");
        displayScreen.Controls.Add(new Picture(
            15,
            22 + OFFSET_Y,
            60,
            78,
            ble));

        displayScreen.Controls.Add(new Label(
            84,
            22 + OFFSET_Y,
            151,
            16)
        {
            Text = "WiFi (Maple)",
            TextColor = Color.Black,
            Font = new Font12x16()
        });

        displayScreen.Controls.Add(new Label(
            84,
            55 + OFFSET_Y,
            151,
            16)
        {
            Text = $"{ipAddress}",
            TextColor = Color.Black,
            Font = new Font8x12()
        });

        displayScreen.Controls.Add(new Label(
            84,
            84 + OFFSET_Y,
            151,
            16)
        {
            Text = "Ready",
            TextColor = Color.Black,
            Font = new Font12x16()
        });

        displayScreen.EndUpdate();
    }
}