using Meadow;
using Meadow.Foundation.Displays;
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
    public class WeatherView
    {
        MicroGraphics graphics;

        public WeatherView()
        {
            Initialize();
        }

        void Initialize()
        {
            var display = new St7789
            (
                spiBus: MeadowApp.Device.CreateSpiBus(),
                chipSelectPin: null,
                dcPin: MeadowApp.Device.Pins.D01,
                resetPin: MeadowApp.Device.Pins.D00,
                width: 240, height: 240,
                colorMode: ColorMode.Format16bppRgb565
            );

            graphics = new MicroGraphics(display)
            {
                Stroke = 1,
                CurrentFont = new Font12x20(),
                Rotation = RotationType._90Degrees
            };

            graphics.Clear();
        }

        public void UpdateDisplay(WeatherViewModel model)
        {
            graphics.Clear();

            graphics.DrawRectangle(0, 0, graphics.Width, graphics.Height, Color.White, true);

            DisplayJPG(model.WeatherCode, 5, 5);

            graphics.DrawText(134, 143, "Outdoor", Color.Black);

            string outdoorTemp = model.OutdoorTemperature.ToString("00°C");
            graphics.DrawText(128, 178, outdoorTemp, Color.Black, ScaleFactor.X2);

            graphics.DrawText(23, 143, "Indoor", Color.Black);

            string indoorTemp = model.IndoorTemperature.ToString("00°C");
            graphics.DrawText(11, 178, indoorTemp, Color.Black, ScaleFactor.X2);

            graphics.Show();
        }

        public void UpdateDateTime()
        {
            int TimeZoneOffSet = -8; // PST
            var today = DateTime.Now.AddHours(TimeZoneOffSet);

            graphics.DrawRectangle(116, 24, 120, 82, Color.White, true);

            graphics.DrawText(128, 24, today.ToString("MM/dd/yy"), color: Color.Black);

            graphics.DrawText(116, 66, today.ToString("hh:mm"), Color.Black, ScaleFactor.X2);

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