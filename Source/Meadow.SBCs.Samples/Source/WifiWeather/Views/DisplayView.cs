using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;
using WifiWeather.Models;
using WifiWeather.ViewModels;

namespace WifiWeather.Views
{
    public class DisplayView
    {
        MicroGraphics graphics;
        int x_padding = 5;

        Color backgroundColor = Color.FromHex("#F3F7FA");
        Color foregroundColor = Color.Black;

        Font12x20 font12X20 = new Font12x20();
        Font8x16 font8X16 = new Font8x16();

        public DisplayView() { }

        public void Initialize(IPixelDisplay display)
        {
            graphics = new MicroGraphics(display)
            {
                Stroke = 1,
                CurrentFont = font12X20
            };

            x_padding = 20;

            graphics.Clear(backgroundColor);
        }

        public void UpdateDateTime()
        {
            var today = DateTime.Now;

            graphics.DrawRectangle(graphics.Width / 2, 24, graphics.Width, 82, backgroundColor, true);

            graphics.CurrentFont = font12X20;
            graphics.DrawText(graphics.Width - x_padding, 25, $"{today.DayOfWeek},{today.Day}{GetOrdinalSuffix(today.Day)}", foregroundColor, alignmentH: HorizontalAlignment.Right);
            graphics.DrawText(graphics.Width - x_padding, 50, today.ToString("MMM"), foregroundColor, scaleFactor: ScaleFactor.X2, alignmentH: HorizontalAlignment.Right);
            graphics.DrawText(graphics.Width - x_padding, 95, today.ToString("yyyy"), foregroundColor, alignmentH: HorizontalAlignment.Right);

            graphics.DrawRectangle(0, 135, graphics.Width, 35, backgroundColor, true);
            graphics.DrawText(graphics.Width / 2, 135, today.ToString("hh:mm:ss tt"), foregroundColor, ScaleFactor.X2, alignmentH: HorizontalAlignment.Center);

            graphics.Show();
        }

        private static string GetOrdinalSuffix(int num)
        {
            string number = num.ToString();
            if (number.EndsWith("11")) return "th";
            if (number.EndsWith("12")) return "th";
            if (number.EndsWith("13")) return "th";
            if (number.EndsWith("1")) return "st";
            if (number.EndsWith("2")) return "nd";
            if (number.EndsWith("3")) return "rd";
            return "th";
        }

        public void UpdateDisplay(WeatherViewModel model)
        {
            int spacing = 95;
            int valueSpacing = 30;
            int y = 200;

            graphics.Clear(backgroundColor);

            DisplayJPG(model.WeatherCode, x_padding, 15);

            graphics.CurrentFont = font12X20;
            graphics.DrawText(x_padding, y, "Temperature", foregroundColor);
            graphics.DrawText(x_padding, y + spacing, "Humidity", foregroundColor);
            graphics.DrawText(x_padding, y + spacing * 2, "Pressure", foregroundColor);
            graphics.DrawText(graphics.Width - x_padding, y, "Feels like", foregroundColor, alignmentH: HorizontalAlignment.Right);
            graphics.DrawText(graphics.Width - x_padding, y + spacing, "Wind Dir", foregroundColor, alignmentH: HorizontalAlignment.Right);
            graphics.DrawText(graphics.Width - x_padding, y + spacing * 2, "Wind Spd", foregroundColor, alignmentH: HorizontalAlignment.Right);

            graphics.DrawText(x_padding, y + valueSpacing, $"{model.OutdoorTemperature}°C", foregroundColor, ScaleFactor.X2);
            graphics.DrawText(graphics.Width - x_padding, y + valueSpacing, $"{model.FeelsLikeTemperature + 2}°C", foregroundColor, ScaleFactor.X2, alignmentH: HorizontalAlignment.Right);
            graphics.DrawText(x_padding, y + valueSpacing + spacing, $"{model.Humidity}%", foregroundColor, ScaleFactor.X2);
            graphics.DrawText(graphics.Width - x_padding, y + valueSpacing + spacing, $"{model.WindDirection}°", foregroundColor, ScaleFactor.X2, alignmentH: HorizontalAlignment.Right);

            graphics.CurrentFont = font8X16;
            graphics.DrawText(graphics.Width - x_padding, y + valueSpacing + spacing * 2, $"{model.WindSpeed}m/s", foregroundColor, ScaleFactor.X2, alignmentH: HorizontalAlignment.Right);
            graphics.DrawText(x_padding, y + valueSpacing + spacing * 2, $"{model.Pressure}hPa", foregroundColor, ScaleFactor.X2);


            graphics.Show();
        }

        void DisplayJPG(int weatherCode, int xOffset, int yOffset)
        {
            var jpgData = LoadResource(weatherCode);
            var decoder = new JpegDecoder();
            var jpg = decoder.DecodeJpeg(jpgData);

            int x = 0;
            int y = 0;
            byte r, g, b;

            for (int i = 0; i < jpg.Length; i += 3)
            {
                r = jpg[i];
                g = jpg[i + 1];
                b = jpg[i + 2];

                graphics.DrawPixel(x + xOffset, y + yOffset, Color.FromRgb(r, g, b));

                x++;
                if (x % decoder.Width == 0)
                {
                    y++;
                    x = 0;
                }
            }
        }

        byte[] LoadResource(int weatherCode)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName;

            switch (weatherCode)
            {
                case int n when (n >= WeatherConstants.THUNDERSTORM_LIGHT_RAIN && n <= WeatherConstants.THUNDERSTORM_HEAVY_DRIZZLE):
                    resourceName = $"WifiWeather.w_storm.jpg";
                    break;
                case int n when (n >= WeatherConstants.DRIZZLE_LIGHT && n <= WeatherConstants.DRIZZLE_SHOWER):
                    resourceName = $"WifiWeather.w_drizzle.jpg";
                    break;
                case int n when (n >= WeatherConstants.RAIN_LIGHT && n <= WeatherConstants.RAIN_SHOWER_RAGGED):
                    resourceName = $"WifiWeather.w_rain.jpg";
                    break;
                case int n when (n >= WeatherConstants.SNOW_LIGHT && n <= WeatherConstants.SNOW_SHOWER_HEAVY):
                    resourceName = $"WifiWeather.w_snow.jpg";
                    break;
                case WeatherConstants.CLOUDS_CLEAR:
                    resourceName = $"WifiWeather.w_clear.jpg";
                    break;
                case int n when (n >= WeatherConstants.CLOUDS_FEW && n <= WeatherConstants.CLOUDS_OVERCAST):
                    resourceName = $"WifiWeather.w_cloudy.jpg";
                    break;
                default:
                    resourceName = $"WifiWeather.w_misc.jpg";
                    break;
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}
