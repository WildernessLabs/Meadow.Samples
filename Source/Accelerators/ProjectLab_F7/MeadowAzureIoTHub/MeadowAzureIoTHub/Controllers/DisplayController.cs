using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace MeadowAzureIoTHub.Controllers
{
    internal class DisplayController
    {
        private readonly int rowHeight = 60;
        private readonly int rowMargin = 15;

        private Color backgroundColor = Color.FromHex("F3F7FA");
        private Color foregroundColor = Color.Black;

        private readonly Font12x20 font12X20 = new Font12x20();
        private readonly Font6x8 font6x8 = new Font6x8();

        protected DisplayScreen DisplayScreen { get; set; }

        protected AbsoluteLayout SplashLayout { get; set; }

        protected AbsoluteLayout DataLayout { get; set; }

        protected Picture WifiStatus { get; set; }

        protected Picture SyncStatus { get; set; }

        protected Label Type { get; set; }

        protected Label Status { get; set; }

        protected Label LastUpdated { get; set; }

        protected Label Temperature { get; set; }

        protected Label Pressure { get; set; }

        protected Label Humidity { get; set; }

        public DisplayController(IPixelDisplay display)
        {
            DisplayScreen = new DisplayScreen(display, RotationType._270Degrees)
            {
                BackgroundColor = backgroundColor
            };

            LoadSplashLayout();

            LoadDataLayout();

            DisplayScreen.Controls.Add(SplashLayout, DataLayout);
        }

        void LoadSplashLayout()
        {
            SplashLayout = new AbsoluteLayout(DisplayScreen, 0, 0, DisplayScreen.Width, DisplayScreen.Height)
            {
                IsVisible = false
            };

            var image = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_meadow.bmp");
            var displayImage = new Picture(0, 0, DisplayScreen.Width, DisplayScreen.Height, image)
            {
                BackColor = Color.FromHex("F39E6C"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            SplashLayout.Controls.Add(displayImage);
        }

        void LoadDataLayout()
        {
            DataLayout = new AbsoluteLayout(DisplayScreen, 0, 0, DisplayScreen.Width, DisplayScreen.Height)
            {
                IsVisible = false
            };

            DataLayout.Controls.Add(new Box(
                0,
                0,
                DisplayScreen.Width,
                rowHeight)
            {
                ForeColor = Color.FromHex("F39E6C")
            });
            DataLayout.Controls.Add(new Box(
                0,
                rowHeight,
                DisplayScreen.Width,
                rowHeight)
            {
                ForeColor = Color.FromHex("F6B691")
            });
            DataLayout.Controls.Add(new Box(
                0,
                rowHeight * 2,
                DisplayScreen.Width,
                rowHeight)
            {
                ForeColor = Color.FromHex("FCC5A6")
            });
            DataLayout.Controls.Add(new Box(
                0,
                rowHeight * 3,
                DisplayScreen.Width,
                rowHeight)
            {
                ForeColor = Color.FromHex("FFD6BE")
            });

            var wifiImage = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_wifi_connecting.bmp");
            WifiStatus = new Picture(
                DisplayScreen.Width - wifiImage.Width - rowMargin,
                7,
                wifiImage.Width,
                wifiImage.Height,
                wifiImage)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(WifiStatus);

            var syncImage = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_refreshed.bmp");
            SyncStatus = new Picture(
                DisplayScreen.Width - syncImage.Width - wifiImage.Width - 5 - rowMargin,
                7,
                syncImage.Width,
                syncImage.Height,
                syncImage)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(SyncStatus);

            DataLayout.Controls.Add(new Box(
                248,
                33,
                57,
                20)
            {
                ForeColor = Color.Black,
                IsFilled = false
            });

            Type = new Label(
                252,
                34,
                48,
                20)
            {
                Text = $"----",
                TextColor = foregroundColor,
                Font = font12X20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            DataLayout.Controls.Add(Type);

            Status = new Label(
                rowMargin,
                15,
                DisplayScreen.Width / 2,
                20)
            {
                Text = $"--:-- -- --/--/--",
                TextColor = foregroundColor,
                Font = font12X20,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            DataLayout.Controls.Add(Status);

            LastUpdated = new Label(
                rowMargin,
                37,
                DisplayScreen.Width / 2,
                8)
            {
                Text = $"Last updated: --:-- -- --/--/--",
                TextColor = foregroundColor,
                Font = font6x8,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            DataLayout.Controls.Add(LastUpdated);

            DataLayout.Controls.Add(new Label(
                rowMargin,
                rowHeight,
                DisplayScreen.Width / 2,
                rowHeight)
            {
                Text = $"TEMPERATURE",
                TextColor = foregroundColor,
                Font = font12X20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            });
            DataLayout.Controls.Add(new Label(
                rowMargin,
                rowHeight * 2,
                DisplayScreen.Width / 2,
                rowHeight)
            {
                Text = $"PRESSURE",
                TextColor = foregroundColor,
                Font = font12X20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            });
            DataLayout.Controls.Add(new Label(
                rowMargin,
                rowHeight * 3,
                DisplayScreen.Width / 2,
                rowHeight)
            {
                Text = $"HUMIDITY",
                TextColor = foregroundColor,
                Font = font12X20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            });

            Temperature = new Label(
                DisplayScreen.Width / 2 - rowMargin,
                rowHeight,
                DisplayScreen.Width / 2,
                rowHeight)
            {
                Text = $"- °C",
                TextColor = foregroundColor,
                Font = font12X20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            DataLayout.Controls.Add(Temperature);

            Pressure = new Label(
                DisplayScreen.Width / 2 - rowMargin,
                rowHeight * 2,
                DisplayScreen.Width / 2,
                rowHeight)
            {
                Text = $"- mb",
                TextColor = foregroundColor,
                Font = font12X20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            DataLayout.Controls.Add(Pressure);

            Humidity = new Label(
                DisplayScreen.Width / 2 - rowMargin,
                rowHeight * 3,
                DisplayScreen.Width / 2,
                rowHeight)
            {
                Text = $"- % ",
                TextColor = foregroundColor,
                Font = font12X20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            DataLayout.Controls.Add(Humidity);
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

        public void UpdateType(string title)
        {
            Type.Text = title;
        }

        public void UpdateStatus(string status)
        {
            Status.Text = status;
        }

        public void UpdateLastUpdated(string lastUpdated)
        {
            LastUpdated.Text = $"Last Updated: {lastUpdated}";
        }

        public void UpdateWiFiStatus(bool isConnected)
        {
            var imageWiFi = isConnected
                ? Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_wifi_connected.bmp")
                : Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_wifi_connecting.bmp");
            WifiStatus.Image = imageWiFi;
        }

        public void UpdateSyncStatus(bool isSyncing)
        {
            var imageSync = isSyncing
                ? Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_refreshing.bmp")
                : Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_refreshed.bmp");
            SyncStatus.Image = imageSync;
        }

        public void UpdateAtmosphericConditions(double temperature, double pressure, double humidity)
        {
            DisplayScreen.BeginUpdate();

            Temperature.Text = $"{temperature:N1} °C";
            Pressure.Text = $"{pressure:N1} mb";
            Humidity.Text = $"{humidity:N1} % ";

            DisplayScreen.EndUpdate();
        }
    }
}