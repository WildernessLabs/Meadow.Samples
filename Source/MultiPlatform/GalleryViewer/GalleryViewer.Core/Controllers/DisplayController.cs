using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace GalleryViewer.Core;

public class DisplayController
{
    private readonly DisplayScreen? screen;

    private Picture networkIcon;
    private Image connectedImage;

    public DisplayController(
        IPixelDisplay? display,
        RotationType displayRotation)
    {
        if (display != null)
        {
            var theme = new DisplayTheme
            {
                Font = new Font12x20(),
                BackgroundColor = Color.Black,
                TextColor = Color.White
            };

            screen = new DisplayScreen(
                display,
                rotation: displayRotation,
                theme: theme);
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {

    }
}