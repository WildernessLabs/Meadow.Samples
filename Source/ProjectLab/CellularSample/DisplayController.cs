using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System;
using System.Threading.Tasks;

namespace CellularSample;

public class DisplayController
{
    DisplayScreen screen;

    private readonly Image imgSignal0Bar = Image.LoadFromResource("CellularSample.Assets.img-cell-0.bmp");
    private readonly Image imgSignal1Bar = Image.LoadFromResource("CellularSample.Assets.img-cell-1.bmp");
    private readonly Image imgSignal2Bar = Image.LoadFromResource("CellularSample.Assets.img-cell-2.bmp");
    private readonly Image imgSignal3Bar = Image.LoadFromResource("CellularSample.Assets.img-cell-3.bmp");
    private readonly Image imgSignal4Bar = Image.LoadFromResource("CellularSample.Assets.img-cell-4.bmp");

    private Label status;
    private Label ipAddress;
    private Picture signalBars;

    public DisplayController(IPixelDisplay _display)
    {
        screen = new DisplayScreen(_display, RotationType._270Degrees)
        {
            BackgroundColor = Color.FromHex("14607F")
        };

        screen.Controls.Add(new Box(5, 5, screen.Width - 10, screen.Height - 10)
        {
            IsFilled = false,
            ForeColor = Color.FromHex("F9E000")
        });

        signalBars = new Picture(105, 33, 110, 103, imgSignal0Bar);
        screen.Controls.Add(signalBars);

        status = new Label(60, 149, 200, 24)
        {
            Text = "OFFLINE...",
            TextColor = Color.FromHex("F9E000"),
            Font = new Font16x24(),
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        screen.Controls.Add(status);

        ipAddress = new Label(35, 183, 250, 28)
        {
            Text = "---.---.---.---",
            TextColor = Color.FromHex("14607F"),
            Font = new Font16x24(),
            BackColor = Color.FromHex("F9E000"),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Bottom,
        };
        screen.Controls.Add(ipAddress);
    }

    public void UpdateSignalBar(int strength)
    {
        Resolver.Log.Info("Signal Strength: " + strength);

        switch (strength)
        {
            case int n when (n >= -70 && n <= -50):
                signalBars.Image = imgSignal4Bar;
                break;
            case int n when (n >= -80 && n <= -71):
                signalBars.Image = imgSignal3Bar;
                break;
            case int n when (n >= -90 && n <= -81):
                signalBars.Image = imgSignal2Bar;
                break;
            case int n when (n >= -100 && n <= -91):
                signalBars.Image = imgSignal1Bar;
                break;
            default:
                signalBars.Image = imgSignal0Bar;
                break;
        }
    }

    public void UpdateStatus(string status)
    {
        this.status.Text = status;
    }

    public void UpdateIpAddress(string ipAddress)
    {
        this.ipAddress.Text = string.IsNullOrEmpty(ipAddress)
            ? "---.---.---.---"
            : ipAddress;
    }
}