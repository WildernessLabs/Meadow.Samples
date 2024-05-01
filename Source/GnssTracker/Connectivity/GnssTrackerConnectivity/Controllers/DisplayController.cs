using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace GnssTrackerConnectivity.Controllers;

public class DisplayController
{
    // Screen height is 122 but buffer is 128
    private readonly int OFFSET_Y = 6;

    private Image ble = Image.LoadFromResource("GnssTrackerConnectivity.Resources.img-ble.bmp");
    private Image wifi = Image.LoadFromResource("GnssTrackerConnectivity.Resources.img-wifi.bmp");

    private readonly DisplayScreen displayScreen;
    private readonly AbsoluteLayout dataLayout;

    private Picture connectivityIcon;
    private Label Line1;
    private Label Line2;
    private Label Line3;

    public DisplayController(IPixelDisplay display)
    {
        displayScreen = new DisplayScreen(display, RotationType._90Degrees);

        displayScreen.BeginUpdate();

        dataLayout = new AbsoluteLayout(displayScreen, 0, 0, displayScreen.Width, displayScreen.Height);

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

        connectivityIcon = new Picture(
            15,
            22 + OFFSET_Y,
            60,
            78,
            wifi);
        dataLayout.Controls.Add(connectivityIcon);

        Line1 = new Label(
            84,
            22 + OFFSET_Y,
            151,
            16)
        {
            Text = "-",
            TextColor = Color.Black,
            Font = new Font12x16()
        };
        dataLayout.Controls.Add(Line1);

        Line2 = new Label(
            84,
            55 + OFFSET_Y,
            151,
            16)
        {
            Text = "-",
            TextColor = Color.Black,
            Font = new Font8x12()
        };
        dataLayout.Controls.Add(Line2);

        Line3 = new Label(
            84,
            84 + OFFSET_Y,
            151,
            16)
        {
            Text = "-",
            TextColor = Color.Black,
            Font = new Font12x16()
        };
        dataLayout.Controls.Add(Line3);

        displayScreen.Controls.Add(dataLayout);
    }

    public void ShowBluetoothReady()
    {
        connectivityIcon.Image = ble;
        Line1.Text = "Bluetooth";
        Line2.Text = "Discoverable";
        Line3.Text = "Ready";

        displayScreen.EndUpdate();
    }

    public void ShowMapleReady(string ipAddress)
    {
        connectivityIcon.Image = wifi;
        Line1.Text = "WiFi (Maple)";
        Line2.Text = $"{ipAddress}";
        Line3.Text = "Ready";

        displayScreen.EndUpdate();
    }
}