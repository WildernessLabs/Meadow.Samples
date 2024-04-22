using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace HMI_Views.Views;

public class GnssTrackerConnectivityView
{
    private DisplayScreen displayScreen;

    public GnssTrackerConnectivityView(IPixelDisplay display)
    {
        displayScreen = new DisplayScreen(display, RotationType._90Degrees)
        {
            BackgroundColor = Color.White
        };

        displayScreen.Controls.Add(new Box(5, 5, displayScreen.Width - 10, displayScreen.Height - 10)
        {
            IsFilled = false,
            ForeColor = Color.Red
        });

        //var ble = Image.LoadFromResource("GnssTrackerConnectivity.Resources.img-ble.bmp");
        //displayScreen.Controls.Add(new Picture(
        //    15,
        //    22,
        //    60,
        //    78,
        //    ble));

        displayScreen.Controls.Add(new Label(84, 22, 151, 16)
        {
            Text = "Line 1",
            TextColor = Color.Black,
            Font = new Font12x16()
        });

        displayScreen.Controls.Add(new Label(84, 53, 151, 16)
        {
            Text = "Line 2",
            TextColor = Color.Black,
            Font = new Font12x16()
        });

        displayScreen.Controls.Add(new Label(84, 84, 151, 16)
        {
            Text = "Line 3",
            TextColor = Color.Black,
            Font = new Font12x16()
        });
    }

    public void Run()
    {

    }
}
