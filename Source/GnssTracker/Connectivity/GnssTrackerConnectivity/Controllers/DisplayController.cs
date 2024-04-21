using Meadow;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace GnssTrackerConnectivity.Controllers;

public class DisplayController
{
    private DisplayScreen displayScreen;

    public DisplayController(IPixelDisplay display)
    {
        displayScreen = new DisplayScreen(display, RotationType._270Degrees)
        {
            BackgroundColor = Color.White
        };


    }


}
