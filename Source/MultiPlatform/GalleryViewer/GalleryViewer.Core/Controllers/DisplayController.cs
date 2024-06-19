using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace GalleryViewer.Core.Controllers;

public class DisplayController
{
    readonly string[] images = new string[3]
    {
        "GalleryViewer.Core.Assets.image1.bmp",
        "GalleryViewer.Core.Assets.image2.bmp",
        "GalleryViewer.Core.Assets.image3.bmp"
    };

    private readonly DisplayScreen? screen;

    private Picture picture;

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

        var image = Image.LoadFromResource(images[0]);
        picture = new Picture(
            0, 0, screen.Width, screen.Height, image);
        screen.Controls.Add(picture);
    }

    public void UpdateDisplay(int imageIndex)
    {
        picture.Image = Image.LoadFromResource(images[imageIndex]);
    }
}