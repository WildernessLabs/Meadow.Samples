using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Peripherals.Displays;
using Meadow.Units;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;

namespace PlantMonitor.Controllers;

public class DisplayController
{
    private static readonly Lazy<DisplayController> instance =
        new Lazy<DisplayController>(() => new DisplayController());
    public static DisplayController Instance => instance.Value;

    readonly int padding = 5;

    MicroGraphics graphics;
    IPixelBuffer image;

    private DisplayController() { }

    public void Initialize(IPixelDisplay display)
    {
        graphics = new MicroGraphics(display)
        {
            IgnoreOutOfBoundsPixels = true,
            CurrentFont = new Font12x20(),
            Stroke = 3,
        };

        graphics.Clear();
        graphics.DrawRectangle(0, 0, graphics.Width, graphics.Height, Color.White, true);
        string plant = "Plant";
        string monitor = "Monitor";
        graphics.CurrentFont = new Font12x16();
        graphics.DrawText(graphics.Width / 2, 80, plant, Color.Black, ScaleFactor.X2, HorizontalAlignment.Center);
        graphics.DrawText(graphics.Width / 2, 130, monitor, Color.Black, ScaleFactor.X2, HorizontalAlignment.Center);
        graphics.Show();
    }

    void DrawImage(int index, int xOffSet = 0, int yOffSet = 0)
    {
        image = LoadJpeg(LoadResource($"PlantMonitor.level_{index}.jpg"));
        graphics.DrawBuffer(
            x: (graphics.Width - image.Width) / 2 + xOffSet,
            y: (graphics.Height - image.Height) / 2 + yOffSet,
            buffer: image);
        graphics.Show();
    }

    IPixelBuffer LoadJpeg(byte[] jpgData)
    {
        var decoder = new JpegDecoder();
        var jpg = decoder.DecodeJpeg(jpgData);

        return new BufferRgb888(decoder.Width, decoder.Height, jpg);
    }

    byte[] LoadResource(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using (Stream stream = assembly.GetManifestResourceStream(fileName))
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }

    public void Update(int percentage)
    {
        RefreshMoistureImage(percentage);
        RefreshMoisturePercentage(percentage);

        graphics.Show();
    }

    void RefreshMoistureImage(int percentage)
    {
        switch (percentage)
        {
            case >= 0 and <= 25: DrawImage(0, 5); break;
            case > 25 and <= 50: DrawImage(1, 5); break;
            case > 50 and <= 75: DrawImage(2, 5); break;
            case > 75: DrawImage(3, 5); break;
        }
    }

    int previousPercentage = 0;
    void RefreshMoisturePercentage(int percentage)
    {
        graphics.DrawText(padding, 208, $"{previousPercentage}%", Color.White, ScaleFactor.X2);
        graphics.DrawText(padding, 208, $"{percentage}%", Color.FromHex("#555555"), ScaleFactor.X2);

        previousPercentage = percentage;
    }

    string previousTemperature = string.Empty;
    public void UpdateTemperatureValue(Temperature newValue)
    {
        graphics.DrawText(graphics.Width - padding, 208, previousTemperature, Color.White, ScaleFactor.X2);
        string tempFormatted = $"{(int)newValue.Celsius}C";
        graphics.DrawText(graphics.Width - padding, 208, tempFormatted, Color.FromHex("#555555"), ScaleFactor.X2, HorizontalAlignment.Right);

        previousTemperature = $"{(int)newValue.Celsius}C";
    }
}