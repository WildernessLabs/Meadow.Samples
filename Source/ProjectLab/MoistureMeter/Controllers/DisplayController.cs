using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using System;

namespace MoistureMeter.Controllers;

public class DisplayController
{
    private static readonly Lazy<DisplayController> instance =
        new Lazy<DisplayController>(() => new DisplayController());
    public static DisplayController Instance => instance.Value;

    MicroGraphics graphics;
    readonly int padding = 12;

    private DisplayController() { }

    public void Initialize(IPixelDisplay display)
    {
        graphics = new MicroGraphics(display)
        {
            Stroke = 1,
            CurrentFont = new Font12x20()
        };
        graphics.Clear();
    }

    public void UpdatePercentage(int percentage)
    {
        int barWidth = 100;
        int barCompleteHeight = 218;
        int barSingleHeight = 20;
        int barSpacing = 2;
        int x = (graphics.Width * 2) / 3 - barWidth / 2 + padding;
        int y = 0;

        graphics.Clear();

        var barColor = Color.FromHex("004B6B");

        graphics.DrawRectangle(x, y + padding, barWidth, barCompleteHeight, barColor, true);

        int percentageGraph = (percentage == 100 ? 9 : percentage / 10);
        for (int barIndex = percentageGraph; barIndex >= 0; barIndex--)
        {
            graphics.DrawRectangle(
                x: x,
                y: graphics.Height - padding - ((barSingleHeight + barSpacing) * barIndex) - 18,
                width: barWidth,
                height: barSingleHeight,
                color: barIndex switch
                {
                    >= 0 and <= 1 => Color.FromHex("FF3535"),
                    >= 2 and <= 4 => Color.FromHex("FF8251"),
                    >= 5 and <= 8 => Color.FromHex("35FF3D"),
                    >= 9 => Color.FromHex("475AFF"),
                    _ => throw new NotImplementedException(),
                },
                filled: true);
        }

        graphics.DrawText(graphics.Width / 3 - padding, (graphics.Height - graphics.CurrentFont.Height) / 2, $"{percentage}%", ScaleFactor.X2, alignmentH: HorizontalAlignment.Center);

        graphics.Show();
    }
}