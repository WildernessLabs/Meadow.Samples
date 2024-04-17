using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System;
using System.Threading.Tasks;

namespace HMI_Views.Views;

public class WiFiWeather
{
    Image _weatherIcon = Image.LoadFromResource("HMI_Views.Resources.w_clear.bmp");

    int x_padding = 20;

    protected DisplayScreen DisplayScreen { get; set; }

    protected Label DayOfWeek { get; set; }

    protected Label Month { get; set; }

    protected Label Year { get; set; }

    protected Label Time { get; set; }

    protected Picture Weather { get; set; }

    protected Label Temperature { get; set; }

    protected Label Humidity { get; set; }

    protected Label Pressure { get; set; }

    protected Label FeelsLike { get; set; }

    protected Label WindDirection { get; set; }

    protected Label WindSpeed { get; set; }

    Meadow.Color backgroundColor = Meadow.Color.FromHex("#F3F7FA");
    Meadow.Color foregroundColor = Meadow.Color.Black;

    Font12x20 font12X20 = new Font12x20();
    Font12x16 font12X16 = new Font12x16();
    Font8x16 font8X16 = new Font8x16();

    public WiFiWeather(IPixelDisplay display)
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
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(DayOfWeek);

        Month = new Label(DisplayScreen.Width / 2 - x_padding, 50, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"Jan",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(Month);

        Year = new Label(DisplayScreen.Width / 2 - x_padding, 95, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"0000",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X16,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(Year);

        Time = new Label(0, 150, DisplayScreen.Width, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"00:00:00 AM",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        DisplayScreen.Controls.Add(Time);

        DisplayScreen.Controls.Add(new Label(x_padding, 215, DisplayScreen.Width / 2, font12X20.Height * 2)
        {
            Text = $"Temperature",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        DisplayScreen.Controls.Add(new Label(x_padding, 304, DisplayScreen.Width / 2, font12X20.Height * 2)
        {
            Text = $"Humidity",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        DisplayScreen.Controls.Add(new Label(x_padding, 393, DisplayScreen.Width / 2, font12X20.Height * 2)
        {
            Text = $"Pressure",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        DisplayScreen.Controls.Add(new Label(DisplayScreen.Width / 2 + x_padding, 215, DisplayScreen.Width / 2 - x_padding * 2, font12X20.Height * 2)
        {
            Text = $"Feels like",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        });
        DisplayScreen.Controls.Add(new Label(DisplayScreen.Width / 2 - x_padding, 304, DisplayScreen.Width / 2, font12X20.Height * 2)
        {
            Text = $"Wind Dir",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        });
        DisplayScreen.Controls.Add(new Label(DisplayScreen.Width / 2 - x_padding, 393, DisplayScreen.Width / 2, font12X20.Height * 2)
        {
            Text = $"Wind Spd",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        });

        Temperature = new Label(x_padding, 245, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"23°C",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DisplayScreen.Controls.Add(Temperature);
        Humidity = new Label(x_padding, 334, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"93%",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DisplayScreen.Controls.Add(Humidity);
        Pressure = new Label(x_padding, 423, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"1102hPa",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font8X16,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DisplayScreen.Controls.Add(Pressure);

        FeelsLike = new Label(DisplayScreen.Width / 2 - x_padding, 245, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"26°C",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(FeelsLike);
        WindDirection = new Label(DisplayScreen.Width / 2 - x_padding, 334, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"178°",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        DisplayScreen.Controls.Add(WindDirection);
        WindSpeed = new Label(DisplayScreen.Width / 2 - x_padding, 423, DisplayScreen.Width / 2, font12X20.Height * 2, ScaleFactor.X2)
        {
            Text = $"19m/s",
            TextColor = foregroundColor,
            BackColor = backgroundColor,
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

    public async Task Run()
    {
        UpdateDisplay(
            weatherIcon: $"HMI_Views.Resources.w_drizzle.bmp",
            temperature: $"23°C",
            humidity: $"93%",
            pressure: $"1102hPa",
            feelsLike: $"26°C",
            windDirection: $"178°",
            windSpeed: $"19m/s");

        while (true)
        {
            var today = DateTime.Now;

            DayOfWeek.Text = $"{today.DayOfWeek},{today.Day}{GetOrdinalSuffix(today.Day)}";
            Month.Text = $"{today.ToString("MMMM").Substring(0, today.ToString("MMMM").Length > 6 ? 7 : today.ToString("MMMM").Length)}";
            Year.Text = $"{today.ToString("yyyy")}";
            Time.Text = DateTime.Now.ToString("hh:mm:ss tt");
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}