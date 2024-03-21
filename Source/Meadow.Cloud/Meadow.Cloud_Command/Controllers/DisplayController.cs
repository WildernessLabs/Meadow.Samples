using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace Meadow.Cloud_Command.Controllers
{
    internal class DisplayController
    {
        private readonly int rowHeight = 60;
        private readonly int rowMargin = 15;

        private Meadow.Color backgroundColor = Meadow.Color.FromHex("#F3F7FA");
        private Meadow.Color foregroundColor = Meadow.Color.White;

        private Font12x20 font12X20 = new Font12x20();
        private Font6x8 font6x8 = new Font6x8();

        Image relayOn = Image.LoadFromResource("Meadow.Cloud_Command.Resources.img_relay_on.bmp");
        Image relayOff = Image.LoadFromResource("Meadow.Cloud_Command.Resources.img_relay_off.bmp");

        protected DisplayScreen DisplayScreen { get; set; }

        protected AbsoluteLayout SplashLayout { get; set; }

        protected AbsoluteLayout DataLayout { get; set; }

        protected Picture WifiStatus { get; set; }

        protected Picture SyncStatus { get; set; }

        protected Picture RelayStatus0 { get; set; }

        protected Picture RelayStatus1 { get; set; }

        protected Picture RelayStatus2 { get; set; }

        protected Picture RelayStatus3 { get; set; }

        protected Label Status { get; set; }

        protected Label LastUpdated { get; set; }

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

            var image = Image.LoadFromResource("Meadow.Cloud_Command.Resources.img_meadow.bmp");
            var displayImage = new Picture(0, 0, DisplayScreen.Width, DisplayScreen.Height, image)
            {
                BackColor = Meadow.Color.FromHex("#B35E2C"),
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

            DataLayout.Controls.Add(new Box(0, 0, DisplayScreen.Width, rowHeight)
            {
                ForeColor = Meadow.Color.FromHex("844936")
            });

            var wifiImage = Image.LoadFromResource("Meadow.Cloud_Command.Resources.img_wifi_connecting.bmp");
            WifiStatus = new Picture(DisplayScreen.Width - wifiImage.Width - rowMargin, 0, wifiImage.Width, rowHeight, wifiImage)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(WifiStatus);

            var syncImage = Image.LoadFromResource("Meadow.Cloud_Command.Resources.img_refreshed.bmp");
            SyncStatus = new Picture(DisplayScreen.Width - syncImage.Width - wifiImage.Width - 10 - rowMargin, 0, syncImage.Width, rowHeight, syncImage)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(SyncStatus);

            Status = new Label(rowMargin, 15, DisplayScreen.Width / 2, 20)
            {
                Text = $"--:-- -- --/--/--",
                TextColor = foregroundColor,
                Font = font12X20,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            DataLayout.Controls.Add(Status);

            LastUpdated = new Label(rowMargin, 37, DisplayScreen.Width / 2, 8)
            {
                Text = $"Last updated: --:-- -- --/--/--",
                TextColor = foregroundColor,
                Font = font6x8,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            DataLayout.Controls.Add(LastUpdated);

            DataLayout.Controls.Add(new Box(0, rowHeight, DisplayScreen.Width, DisplayScreen.Height - rowHeight)
            {
                ForeColor = Meadow.Color.FromHex("B35E2C")
            });

            int relayWidth = 71;
            int relayHeight = 156;
            int margin = 12;
            int relaySpacing = 4;
            int smallMargin = 2;

            DataLayout.Controls.Add(new Box(
                margin,
                rowHeight + margin,
                relayWidth,
                relayHeight)
            {
                ForeColor = Meadow.Color.White,
                IsFilled = false
            });
            DataLayout.Controls.Add(new Label(
                margin,
                rowHeight + margin + smallMargin * 3,
                relayWidth,
                font6x8.Height + smallMargin * 2)
            {
                Text = $"RELAY 0",
                TextColor = foregroundColor,
                Font = font6x8,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            RelayStatus0 = new Picture(
                margin,
                rowHeight + margin + smallMargin * 2,
                relayWidth,
                relayHeight,
                relayOff)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(RelayStatus0);

            DataLayout.Controls.Add(new Box(
                margin + relayWidth + relaySpacing,
                rowHeight + margin,
                relayWidth,
                relayHeight)
            {
                ForeColor = Meadow.Color.White,
                IsFilled = false
            });
            DataLayout.Controls.Add(new Label(
                margin + relayWidth + relaySpacing,
                rowHeight + margin + smallMargin * 3,
                relayWidth,
                font6x8.Height + smallMargin * 2)
            {
                Text = $"RELAY 1",
                TextColor = foregroundColor,
                Font = font6x8,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            RelayStatus1 = new Picture(
                margin + relayWidth + relaySpacing,
                rowHeight + margin + smallMargin * 2,
                relayWidth,
                relayHeight,
                relayOff)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(RelayStatus1);

            DataLayout.Controls.Add(new Box(
                margin + relayWidth * 2 + relaySpacing * 2,
                rowHeight + margin,
                relayWidth,
                relayHeight)
            {
                ForeColor = Meadow.Color.White,
                IsFilled = false
            });
            DataLayout.Controls.Add(new Label(
                margin + relayWidth * 2 + relaySpacing * 2,
                rowHeight + margin + smallMargin * 3,
                relayWidth,
                font6x8.Height + smallMargin * 2)
            {
                Text = $"RELAY 2",
                TextColor = foregroundColor,
                Font = font6x8,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            RelayStatus2 = new Picture(
                margin + relayWidth * 2 + relaySpacing * 2,
                rowHeight + margin + smallMargin * 2,
                relayWidth,
                relayHeight,
                relayOff)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(RelayStatus2);

            DataLayout.Controls.Add(new Box(
                margin + relayWidth * 3 + relaySpacing * 3,
                rowHeight + margin,
                relayWidth,
                relayHeight)
            {
                ForeColor = Meadow.Color.White,
                IsFilled = false
            });
            DataLayout.Controls.Add(new Label(
                margin + relayWidth * 3 + relaySpacing * 3,
                rowHeight + margin + smallMargin * 3,
                relayWidth,
                font6x8.Height + smallMargin * 2)
            {
                Text = $"RELAY 3",
                TextColor = foregroundColor,
                Font = font6x8,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            RelayStatus3 = new Picture(
                margin + relayWidth * 3 + relaySpacing * 3,
                rowHeight + margin + smallMargin * 2,
                relayWidth,
                relayHeight,
                relayOff)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            DataLayout.Controls.Add(RelayStatus3);
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

        public void UpdateLastUpdated(string lastUpdated)
        {
            LastUpdated.Text = $"Last command: {lastUpdated}";
        }

        public void UpdateWiFiStatus(bool isConnected)
        {
            var imageWiFi = isConnected
                ? Image.LoadFromResource("Meadow.Cloud_Command.Resources.img_wifi_connected.bmp")
                : Image.LoadFromResource("Meadow.Cloud_Command.Resources.img_wifi_connecting.bmp");
            WifiStatus.Image = imageWiFi;
        }

        public void UpdateSyncStatus(bool isSyncing)
        {
            var imageSync = isSyncing
                ? Image.LoadFromResource("Meadow.Cloud_Command.Resources.img_refreshing.bmp")
                : Image.LoadFromResource("Meadow.Cloud_Command.Resources.img_refreshed.bmp");
            SyncStatus.Image = imageSync;
        }

        public void UpdateRelayStatus(int relay, bool isOn)
        {
            var relayStatus = isOn ? relayOn : relayOff;

            switch (relay)
            {
                case 0:
                    RelayStatus0.Image = relayStatus;
                    break;
                case 1:
                    RelayStatus1.Image = relayStatus;
                    break;
                case 2:
                    RelayStatus2.Image = relayStatus;
                    break;
                case 3:
                    RelayStatus3.Image = relayStatus;
                    break;
                case 4:
                    RelayStatus0.Image = relayStatus;
                    RelayStatus1.Image = relayStatus;
                    RelayStatus2.Image = relayStatus;
                    RelayStatus3.Image = relayStatus;
                    break;
            }
        }
    }
}