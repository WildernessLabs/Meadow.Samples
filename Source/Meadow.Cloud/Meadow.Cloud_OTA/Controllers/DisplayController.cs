using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System;
using System.Threading;

namespace Meadow.Cloud_OTA.Controllers
{
    internal class DisplayController
    {
        private Color backgroundColor = Color.FromHex("10485E");

        private Font12x16 font12X16 = new Font12x16();

        private ProgressBar progressBar;
        private Label progressValue;
        private Label status;

        public DisplayController(IPixelDisplay display)
        {
            var displayScreen = new DisplayScreen(display, RotationType._270Degrees)
            {
                BackgroundColor = backgroundColor
            };

            var logo = Image.LoadFromResource("Meadow.Cloud_OTA.Resources.img_meadow.bmp");
            displayScreen.Controls.Add(new Picture(95, 33, logo.Width, logo.Height, logo));

            displayScreen.Controls.Add(new Label(0, 127, displayScreen.Width, font12X16.Height * 2)
            {
                Text = $"App v{MeadowApp.VERSION:N1}",
                TextColor = Color.White,
                Font = font12X16,
                ScaleFactor = ScaleFactor.X2,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            status = new Label(0, 175, displayScreen.Width, font12X16.Height)
            {
                Text = "-",
                TextColor = Color.White,
                Font = font12X16,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            displayScreen.Controls.Add(status);

            progressBar = new ProgressBar(90, 205, 140, 16)
            {
                BackColor = Color.Black,
                ValueColor = Color.FromHex("0B3749"),
                BorderColor = Color.FromHex("0B3749"),
                IsVisible = false
            };
            displayScreen.Controls.Add(progressBar);

            progressValue = new Label(90, 206, 140, 16)
            {
                Text = "0%",
                TextColor = Color.White,
                Font = font12X16,
                HorizontalAlignment = HorizontalAlignment.Center,
                IsVisible = false
            };
            displayScreen.Controls.Add(progressValue);
        }

        public void UpdateStatus(string text)
        {
            status.Text = text;
        }

        public void UpdateDownloadProgress(int progress)
        {
            if (!progressBar.IsVisible)
            {
                progressBar.IsVisible = true;
                progressValue.IsVisible = true;
            }

            progressBar.Value = progress;
            progressValue.Text = $"{progress}%";

            if (progress == 100)
            {
                UpdateStatus("Download Complete");

                Thread.Sleep(TimeSpan.FromSeconds(3));

                UpdateStatus(string.Empty);
                progressBar.IsVisible = false;
                progressValue.IsVisible = false;
            }
        }
    }
}
