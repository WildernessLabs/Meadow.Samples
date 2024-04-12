using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace AmbientRoomMonitor.Services;

internal class DisplayController
{
    private readonly int rowHeight = 60;
    private readonly int rowMargin = 15;

    private Color backgroundColor = Color.FromHex("#F3F7FA");
    private Color foregroundColor = Color.Black;

    private readonly Font12x20 font12X20 = new Font12x20();

    private DisplayScreen displayScreen;

    private Label light;
    private Label pressure;
    private Label humidity;
    private Label temperature;

    public DisplayController(IPixelDisplay display)
    {
        displayScreen = new DisplayScreen(display, RotationType._270Degrees)
        {
            BackgroundColor = backgroundColor
        };

        displayScreen.Controls.Add(new GradientBox(0, 0, displayScreen.Width, displayScreen.Height)
        {
            StartColor = Color.FromHex("#5AC0EA"),
            EndColor = Color.FromHex("#B8E4F6")
        });

        displayScreen.Controls.Add(new Label(rowMargin, 0, displayScreen.Width / 2, rowHeight)
        {
            Text = $"LUMINANCE",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        displayScreen.Controls.Add(new Label(rowMargin, rowHeight, displayScreen.Width / 2, rowHeight)
        {
            Text = $"PRESSURE",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        displayScreen.Controls.Add(new Label(rowMargin, rowHeight * 2, displayScreen.Width / 2, rowHeight)
        {
            Text = $"HUMIDITY",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        displayScreen.Controls.Add(new Label(rowMargin, rowHeight * 3, displayScreen.Width / 2, rowHeight)
        {
            Text = $"TEMPERATURE",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left
        });

        light = new Label(displayScreen.Width / 2 - rowMargin, 0, displayScreen.Width / 2, rowHeight)
        {
            Text = $"- Lx",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        displayScreen.Controls.Add(light);

        pressure = new Label(displayScreen.Width / 2 - rowMargin, rowHeight, displayScreen.Width / 2, rowHeight)
        {
            Text = $"- Mb",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        displayScreen.Controls.Add(pressure);

        humidity = new Label(displayScreen.Width / 2 - rowMargin, rowHeight * 2, displayScreen.Width / 2, rowHeight)
        {
            Text = $"- % ",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        displayScreen.Controls.Add(humidity);

        temperature = new Label(displayScreen.Width / 2 - rowMargin, rowHeight * 3, displayScreen.Width / 2, rowHeight)
        {
            Text = $"- °C",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        displayScreen.Controls.Add(temperature);
    }

    public void UpdateAtmosphericConditions(string light, string pressure, string humidity, string temperature)
    {
        displayScreen.BeginUpdate();

        this.light.Text = $"{light} Lx";
        this.pressure.Text = $"{pressure} mb";
        this.humidity.Text = $"{humidity} % ";
        this.temperature.Text = $"{temperature} °C";

        displayScreen.EndUpdate();
    }
}