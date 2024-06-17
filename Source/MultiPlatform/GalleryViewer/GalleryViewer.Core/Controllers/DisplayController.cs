using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace GalleryViewer.Core;

public class DisplayController
{
    private readonly DisplayScreen? screen;

    private Image connectedImage;

    private Picture networkIcon;


    public DisplayController(
        IPixelDisplay? display,
        RotationType displayRotation)
    {
        var theme = new DisplayTheme
        {
            Font = new Font12x20(),
            BackgroundColor = Color.Black,
            TextColor = Color.White
        };

        screen = new DisplayScreen(
            physicalDisplay: display,
            rotation: displayRotation,
            theme: theme);

        screen.Controls.Add(new Label(
            0, 0, 128, 20)
        {
            Text = "Gallery Viewer",
        });

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {

    }
}