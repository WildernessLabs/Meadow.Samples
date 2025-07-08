using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System;

namespace WifiWeather.Controllers;

public class DisplayController
{
    Image _weatherIcon = Image.LoadFromResource("WifiWeather.Resources.w_clear.bmp");

    int x_padding = 20;

    private DisplayScreen DisplayScreen;
    private Label DayOfWeek;
    private Label Month;
    private Label Year;
    private Label Time;
    private Picture Weather;
    private Label Temperature;
    private Label Humidity;
    private Label Pressure;
    private Label FeelsLike;
    private Label WindDirection;
    private Label WindSpeed;

    Color backgroundColor = Color.FromHex("#F3F7FA");
    Color foregroundColor = Color.Black;

    Font12x20 font12X20 = new Font12x20();
    Font12x16 font12X16 = new Font12x16();
    Font8x16 font8X16 = new Font8x16();

    public DisplayController(IPixelDisplay display)
    {
        DisplayScreen = new DisplayScreen(display)
        {
            BackgroundColor = backgroundColor
        };

        Weather = new Picture(x_padding, 25, 100, 100, _weatherIcon);
        DisplayScreen.Controls.Add(Weather);

        DayOfWeek = new Label(DisplayScreen.Width / 2 - x_padding, 25, DisplayScreen.Width / 2, font12X20.Height)
        {
            Text = $"Monday,1st",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(DayOfWeek);

        Month = new Label(DisplayScreen.Width / 2 - x_padding, 50, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"Jan",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(Month);

        Year = new Label(DisplayScreen.Width / 2 - x_padding, 95, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"0000",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X16,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(Year);

        Time = new Label(0, 150, DisplayScreen.Width, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"00:00:00 AM",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        DisplayScreen.Controls.Add(Time);

        DisplayScreen.Controls.Add(new Label(x_padding, 215, DisplayScreen.Width / 2, font12X20.Height * 2)
        {
            Text = $"Temperature",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        DisplayScreen.Controls.Add(new Label(x_padding, 304, DisplayScreen.Width / 2, font12X20.Height * 2)
        {
            Text = $"Humidity",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        DisplayScreen.Controls.Add(new Label(x_padding, 393, DisplayScreen.Width / 2, font12X20.Height * 2)
        {
            Text = $"Pressure",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        DisplayScreen.Controls.Add(new Label(DisplayScreen.Width / 2 + x_padding, 215, DisplayScreen.Width / 2 - x_padding * 2, font12X20.Height * 2)
        {
            Text = $"Feels like",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        });
        DisplayScreen.Controls.Add(new Label(DisplayScreen.Width / 2 - x_padding, 304, DisplayScreen.Width / 2, font12X20.Height * 2)
        {
            Text = $"Wind Dir",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        });
        DisplayScreen.Controls.Add(new Label(DisplayScreen.Width / 2 - x_padding, 393, DisplayScreen.Width / 2, font12X20.Height * 2)
        {
            Text = $"Wind Spd",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        });

        Temperature = new Label(x_padding, 245, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"0°C",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DisplayScreen.Controls.Add(Temperature);
        Humidity = new Label(x_padding, 334, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"0%",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DisplayScreen.Controls.Add(Humidity);
        Pressure = new Label(x_padding, 423, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"0hPa",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font8X16,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DisplayScreen.Controls.Add(Pressure);

        FeelsLike = new Label(DisplayScreen.Width / 2 - x_padding, 245, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"0°C",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(FeelsLike);
        WindDirection = new Label(DisplayScreen.Width / 2 - x_padding, 334, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"0°",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(WindDirection);
        WindSpeed = new Label(DisplayScreen.Width / 2 - x_padding, 423, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"0m/s",
            TextColor = foregroundColor,
            BackgroundColor = backgroundColor,
            Font = font8X16,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(WindSpeed);
    }

    private static string GetOrdinalSuffix(int num)
    {
        string number = num.ToString();
        if (number.EndsWith("1")) return "st";
        if (number.EndsWith("2")) return "nd";
        if (number.EndsWith("3")) return "rd";
        if (number.EndsWith("11")) return "th";
        if (number.EndsWith("12")) return "th";
        if (number.EndsWith("13")) return "th";
        return "th";
    }

    public void UpdateDateTime()
    {
        var today = DateTime.Now;

        DayOfWeek.Text = $"{today.DayOfWeek},{today.Day}{GetOrdinalSuffix(today.Day)}";
        Month.Text = $"{today.ToString("MMMM").Substring(0, today.ToString("MMMM").Length > 6 ? 7 : today.ToString("MMMM").Length)}";
        Year.Text = $"{today.ToString("yyyy")}";
        Time.Text = DateTime.Now.ToString("hh:mm:ss tt");
    }

    public void UpdateDisplay(string weatherIcon, string temperature, string humidity, string pressure, string feelsLike, string windDirection, string windSpeed)
    {
        _weatherIcon = Image.LoadFromResource(weatherIcon);
        Weather.Image = _weatherIcon;

        Temperature.Text = temperature;
        Humidity.Text = humidity;
        Pressure.Text = pressure;
        FeelsLike.Text = feelsLike;
        WindDirection.Text = windDirection;
        WindSpeed.Text = windSpeed;
    }
}