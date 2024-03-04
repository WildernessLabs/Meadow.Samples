using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinFormsMeadow.Views
{
    internal class WifiWeatherV2
    {
        private Color backgroundColor = Color.FromHex("10485E");
        private Color outdoorColor = Color.FromHex("C9DB31");
        private Color ForegroundColor = Color.FromHex("EEEEEE");
        private Font8x16 font8x16 = new Font8x16();
        private Font6x8 font6x8 = new Font6x8();

        private int margin = 5;
        readonly int smallMargin = 3;
        readonly int graphHeight = 105;

        readonly int measureBoxWidth = 82;

        readonly int columnWidth = 100;

        readonly int rowHeight = 30;
        readonly int row1 = 135;
        readonly int row2 = 170;
        readonly int row3 = 205;

        Image weatherIcon = Image.LoadFromResource($"WinFormsMeadow.Resources.w_misc.bmp");

        public LineChartSeries OutdoorSeries { get; set; }
        protected DisplayScreen DisplayScreen { get; set; }
        protected AbsoluteLayout SplashLayout { get; set; }
        protected AbsoluteLayout DataLayout { get; set; }
        protected LineChart LineChart { get; set; }
        protected Picture WifiStatus { get; set; }
        protected Picture SyncStatus { get; set; }
        protected Picture Weather { get; set; }
        protected Label Status { get; set; }

        protected Box TemperatureBox { get; set; }
        protected Label TemperatureLabel { get; set; }
        protected Label TemperatureValue { get; set; }

        protected Box PressureBox { get; set; }
        protected Label PressureLabel { get; set; }
        protected Label PressureValue { get; set; }

        protected Box HumidityBox { get; set; }
        protected Label HumidityLabel { get; set; }
        protected Label HumidityValue { get; set; }

        protected Label FeelsLike { get; set; }
        protected Label Sunrise { get; set; }
        protected Label Sunset { get; set; }

        public WifiWeatherV2(IPixelDisplay display)
        {
            DisplayScreen = new DisplayScreen(display)
            {
                BackgroundColor = backgroundColor
            };

            LoadSplashLayout();

            DisplayScreen.Controls.Add(SplashLayout);

            LoadDataLayout();

            DisplayScreen.Controls.Add(DataLayout);
        }

        private void LoadSplashLayout()
        {
            SplashLayout = new AbsoluteLayout(DisplayScreen, 0, 0, DisplayScreen.Width, DisplayScreen.Height)
            {
                IsVisible = false
            };

            var image = Image.LoadFromResource("WinFormsMeadow.Resources.img_meadow.bmp");
            var displayImage = new Picture(0, 0, DisplayScreen.Width, DisplayScreen.Height, image)
            {
                BackColor = Color.FromHex("#14607F"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            SplashLayout.Controls.Add(displayImage);
        }

        private void LoadDataLayout()
        {
            DataLayout = new AbsoluteLayout(DisplayScreen, 0, 0, DisplayScreen.Width, DisplayScreen.Height)
            {
                BackgroundColor = backgroundColor,
                IsVisible = false
            };

            Status = new Label(
                margin,
                margin + 2,
                152,
                font8x16.Height)
            {
                Text = $"Project Lab v3",
                TextColor = Color.White,
                Font = font8x16,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(Status);

            var wifiImage = Image.LoadFromResource("WinFormsMeadow.Resources.img_wifi_connecting.bmp");
            WifiStatus = new Picture(
                DisplayScreen.Width - wifiImage.Width - margin,
                margin,
                wifiImage.Width,
                font8x16.Height,
                wifiImage)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(WifiStatus);

            var syncImage = Image.LoadFromResource("WinFormsMeadow.Resources.img_refreshed.bmp");
            SyncStatus = new Picture(
                DisplayScreen.Width - syncImage.Width - wifiImage.Width - margin * 2,
                margin,
                syncImage.Width,
                font8x16.Height,
                syncImage)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(SyncStatus);

            LineChart = new LineChart(
            margin,
            25,
            DisplayScreen.Width - margin * 2,
            graphHeight)
            {
                BackgroundColor = Color.FromHex("082936"),
                AxisColor = ForegroundColor,
                ShowYAxisLabels = true,
                IsVisible = false,
                AlwaysShowYOrigin = false,
            };
            OutdoorSeries = new LineChartSeries()
            {
                LineColor = outdoorColor,
                PointColor = outdoorColor,
                LineStroke = 1,
                PointSize = 2,
                ShowLines = true,
                ShowPoints = true,
            };
            LineChart.Series.Add(OutdoorSeries);
            DataLayout.Controls.Add(LineChart);

            var weatherImage = Image.LoadFromResource("WinFormsMeadow.Resources.w_misc.bmp");
            Weather = new Picture(
                margin,
                row1,
                100,
                100,
                weatherImage)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(Weather);

            #region TEMPERATURE
            TemperatureBox = new Box(
                columnWidth + margin * 2,
                row1,
                columnWidth,
                rowHeight)
            {
                ForeColor = outdoorColor
            };
            DataLayout.Controls.Add(TemperatureBox);
            TemperatureLabel = new Label(
                columnWidth + margin * 2 + smallMargin,
                row1 + smallMargin,
                measureBoxWidth - smallMargin * 2,
                font6x8.Height)
            {
                Text = $"TEMPERATURE",
                TextColor = backgroundColor,
                Font = font6x8
            };
            DataLayout.Controls.Add(TemperatureLabel);
            TemperatureValue = new Label(
                columnWidth + margin * 2 + smallMargin,
                row1 + font6x8.Height + smallMargin * 2,
                measureBoxWidth - smallMargin * 2,
                font6x8.Height * 2)
            {
                Text = $"-.-C",
                TextColor = backgroundColor,
                Font = font6x8,
                ScaleFactor = ScaleFactor.X2
            };
            DataLayout.Controls.Add(TemperatureValue);
            #endregion

            #region PRESSURE
            PressureBox = new Box(
                columnWidth + margin * 2,
                row2,
                columnWidth,
                rowHeight)
            {
                ForeColor = backgroundColor
            };
            DataLayout.Controls.Add(PressureBox);
            PressureLabel = new Label(
                columnWidth + margin * 2 + smallMargin,
                row2 + smallMargin,
                measureBoxWidth - smallMargin * 2,
                font6x8.Height)
            {
                Text = $"PRESSURE",
                TextColor = ForegroundColor,
                Font = font6x8
            };
            DataLayout.Controls.Add(PressureLabel);
            PressureValue = new Label(
                columnWidth + margin * 2 + smallMargin,
                row2 + font6x8.Height + smallMargin * 2,
                measureBoxWidth - smallMargin * 2,
                font6x8.Height * 2)
            {
                Text = $"-.-hPa",
                TextColor = ForegroundColor,
                Font = font6x8,
                ScaleFactor = ScaleFactor.X2
            };
            DataLayout.Controls.Add(PressureValue);
            #endregion

            #region HUMIDITY
            HumidityBox = new Box(
                columnWidth + margin * 2,
                row3,
                columnWidth,
                rowHeight)
            {
                ForeColor = backgroundColor
            };
            DataLayout.Controls.Add(HumidityBox);
            HumidityLabel = new Label(
                columnWidth + margin * 2 + smallMargin,
                row3 + smallMargin,
                measureBoxWidth - smallMargin * 2,
                font6x8.Height)
            {
                Text = $"HUMIDITY",
                TextColor = ForegroundColor,
                Font = font6x8
            };
            DataLayout.Controls.Add(HumidityLabel);
            HumidityValue = new Label(
                columnWidth + margin * 2 + smallMargin,
                row3 + font6x8.Height + smallMargin * 2,
                columnWidth - smallMargin * 2,
                font6x8.Height * 2)
            {
                Text = $"-.-%",
                TextColor = ForegroundColor,
                Font = font6x8,
                ScaleFactor = ScaleFactor.X2
            };
            DataLayout.Controls.Add(HumidityValue);
            #endregion

            DataLayout.Controls.Add(new Label(
                columnWidth * 2 + margin * 3 + smallMargin,
                row1 + smallMargin,
                measureBoxWidth - smallMargin * 2,
                font6x8.Height)
            {
                Text = $"FEELS LIKE",
                TextColor = ForegroundColor,
                Font = font6x8
            });
            FeelsLike = new Label(
                columnWidth * 2 + margin * 3 + smallMargin,
                row1 + font6x8.Height + smallMargin * 2,
                columnWidth - smallMargin * 2,
                font6x8.Height * 2)
            {
                Text = $"-.-C",
                TextColor = ForegroundColor,
                Font = font6x8,
                ScaleFactor = ScaleFactor.X2
            };
            DataLayout.Controls.Add(FeelsLike);

            DataLayout.Controls.Add(new Label(
                columnWidth * 2 + margin * 3 + smallMargin,
                row2 + smallMargin,
                measureBoxWidth - smallMargin * 2,
                font6x8.Height)
            {
                Text = $"SUNRISE",
                TextColor = ForegroundColor,
                Font = font6x8
            });
            Sunrise = new Label(
                columnWidth * 2 + margin * 3 + smallMargin,
                row2 + font6x8.Height + smallMargin * 2,
                columnWidth - smallMargin * 2,
                font6x8.Height * 2)
            {
                Text = $"--:-- --",
                TextColor = ForegroundColor,
                Font = font6x8,
                ScaleFactor = ScaleFactor.X2
            };
            DataLayout.Controls.Add(Sunrise);

            DataLayout.Controls.Add(new Label(
                columnWidth * 2 + margin * 3 + smallMargin,
                row3 + smallMargin,
                measureBoxWidth - smallMargin * 2,
                font6x8.Height)
            {
                Text = $"SUNSET",
                TextColor = ForegroundColor,
                Font = font6x8
            });
            Sunset = new Label(
                columnWidth * 2 + margin * 3 + smallMargin,
                row3 + font6x8.Height + smallMargin * 2,
                columnWidth - smallMargin * 2,
                font6x8.Height * 2)
            {
                Text = $"--:-- --",
                TextColor = ForegroundColor,
                Font = font6x8,
                ScaleFactor = ScaleFactor.X2
            };
            DataLayout.Controls.Add(Sunset);
        }

        public void ShowSplashScreen()
        {
            DataLayout.IsVisible = false;
            SplashLayout.IsVisible = true;
        }

        public void ShowDataScreen()
        {
            SplashLayout.IsVisible = false;
            DataLayout.IsVisible = true;
        }

        public void UpdateStatus(string status)
        {
            Status.Text = status;
        }

        public void UpdateWiFiStatus(bool isConnected)
        {
            var imageWiFi = isConnected
                ? Image.LoadFromResource("WinFormsMeadow.Resources.img_wifi_connected.bmp")
                : Image.LoadFromResource("WinFormsMeadow.Resources.img_wifi_connecting.bmp");
            WifiStatus.Image = imageWiFi;
        }

        public void UpdateSyncStatus(bool isSyncing)
        {
            var imageSync = isSyncing
                ? Image.LoadFromResource("WinFormsMeadow.Resources.img_refreshing.bmp")
                : Image.LoadFromResource("WinFormsMeadow.Resources.img_refreshed.bmp");
            SyncStatus.Image = imageSync;
        }

        private void UpdateReadingType(int type)
        {
            TemperatureBox.ForeColor = PressureBox.ForeColor = HumidityBox.ForeColor = backgroundColor;
            TemperatureLabel.TextColor = PressureLabel.TextColor = HumidityLabel.TextColor = ForegroundColor;
            TemperatureValue.TextColor = PressureValue.TextColor = HumidityValue.TextColor = ForegroundColor;

            switch (type)
            {
                case 0:
                    TemperatureBox.ForeColor = outdoorColor;
                    TemperatureLabel.TextColor = backgroundColor;
                    TemperatureValue.TextColor = backgroundColor;
                    break;
                case 1:
                    PressureBox.ForeColor = outdoorColor;
                    PressureLabel.TextColor = backgroundColor;
                    PressureValue.TextColor = backgroundColor;
                    break;
                case 2:
                    HumidityBox.ForeColor = outdoorColor;
                    HumidityLabel.TextColor = backgroundColor;
                    HumidityLabel.TextColor = backgroundColor;
                    break;
            }
        }

        public void UpdateReadings(
            int readingType,
            string icon,
            double temperature,
            double humidity,
            double pressure,
            double feelsLike,
            DateTime sunrise,
            DateTime sunset,
            List<double> outdoorReadings)
        {
            DisplayScreen.BeginUpdate();

            UpdateReadingType(readingType);

            weatherIcon = Image.LoadFromResource(icon);
            Weather.Image = weatherIcon;

            TemperatureValue.Text = $"{temperature:N1}C";
            HumidityValue.Text = $"{humidity:N1}%";
            PressureValue.Text = $"{pressure:N2}atm";
            FeelsLike.Text = $"{feelsLike:N1}C";
            Sunrise.Text = $"{sunrise:hh:mm tt}";
            Sunset.Text = $"{sunset:hh:mm tt}";

            OutdoorSeries.Points.Clear();

            for (var p = 0; p < outdoorReadings.Count; p++)
            {
                OutdoorSeries.Points.Add(p * 2, outdoorReadings[p]);
            }

            DisplayScreen.EndUpdate();
        }

        public async Task Run()
        {
            //ShowSplashScreen();
            //Thread.Sleep(3000);
            ShowDataScreen();

            var random = new Random();

            int x = 0;

            var outdoorList = new List<double>
            {
                25.3,
                26.1,
                25.7,
                27.9,
                26.3,
                25.6,
                26.3,
                27.2,
                27.6
            };

            while (true)
            {
                UpdateStatus(DateTime.Now.ToString("hh:mm tt | dd/MM/yy"));

                UpdateReadings(
                    readingType: 0,
                    icon: "WinFormsMeadow.Resources.w_clear.bmp",
                    temperature: random.Next(10, 13),
                    humidity: random.Next(65, 75),
                    pressure: 1 + random.NextDouble(),
                    feelsLike: random.Next(22, 25),
                    sunrise: DateTime.Now,
                    sunset: DateTime.Now.AddHours(8),
                    outdoorReadings: outdoorList);

                await Task.Delay(1000);
            }
        }
    }
}