﻿using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace ProjectLab_Logging.Controllers;

internal class DisplayController
{
    private readonly int rowHeight = 60;
    private readonly int rowMargin = 15;

    private Color backgroundColor = Color.FromHex("#F3F7FA");
    private Color foregroundColor = Color.Black;

    private Font6x8 font6x8 = new Font6x8();
    private Font12x20 font12X20 = new Font12x20();

    private DisplayScreen displayScreen;

    private AbsoluteLayout splashLayout;
    private AbsoluteLayout dataLayout;

    private Picture wifiStatus;
    private Picture syncStatus;

    private Label status;
    private Label lastUpdated;
    private Label temperature;
    private Label pressure;
    private Label humidity;

    public DisplayController(IPixelDisplay display)
    {
        displayScreen = new DisplayScreen(display, RotationType._270Degrees)
        {
            BackgroundColor = backgroundColor
        };

        LoadSplashLayout();

        LoadDataLayout();

        displayScreen.Controls.Add(splashLayout, dataLayout);
    }

    private void LoadSplashLayout()
    {
        splashLayout = new AbsoluteLayout(0, 0, displayScreen.Width, displayScreen.Height)
        {
            IsVisible = false
        };

        var image = Image.LoadFromResource("ProjectLab_Logging.Resources.img_meadow.bmp");
        var displayImage = new Picture(0, 0, displayScreen.Width, displayScreen.Height, image)
        {
            BackColor = Color.FromHex("#C9DB31"),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        splashLayout.Controls.Add(displayImage);
    }

    private void LoadDataLayout()
    {
        dataLayout = new AbsoluteLayout(0, 0, displayScreen.Width, displayScreen.Height)
        {
            IsVisible = false
        };

        displayScreen.Controls.Add(new GradientBox(0, 0, displayScreen.Width, displayScreen.Height)
        {
            StartColor = Color.FromHex("#CADC32"),
            EndColor = Color.FromHex("#E5EE9B")
        });

        var wifiImage = Image.LoadFromResource("ProjectLab_Logging.Resources.img_wifi_connecting.bmp");
        wifiStatus = new Picture(displayScreen.Width - wifiImage.Width - rowMargin, 0, wifiImage.Width, rowHeight, wifiImage)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(wifiStatus);

        var syncImage = Image.LoadFromResource("ProjectLab_Logging.Resources.img_refreshed.bmp");
        syncStatus = new Picture(displayScreen.Width - syncImage.Width - wifiImage.Width - 10 - rowMargin, 0, syncImage.Width, rowHeight, syncImage)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(syncStatus);

        status = new Label(rowMargin, 15, displayScreen.Width / 2, 20)
        {
            Text = $"--:-- -- --/--/--",
            TextColor = foregroundColor,
            Font = font12X20,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        dataLayout.Controls.Add(status);

        lastUpdated = new Label(rowMargin, 37, displayScreen.Width / 2, 8)
        {
            Text = $"Last updated: --:-- -- --/--/--",
            TextColor = foregroundColor,
            Font = font6x8,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        dataLayout.Controls.Add(lastUpdated);

        dataLayout.Controls.Add(new Label(rowMargin, rowHeight, displayScreen.Width / 2, rowHeight)
        {
            Text = $"TEMPERATURE",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        dataLayout.Controls.Add(new Label(rowMargin, rowHeight * 2, displayScreen.Width / 2, rowHeight)
        {
            Text = $"PRESSURE",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left
        });
        dataLayout.Controls.Add(new Label(rowMargin, rowHeight * 3, displayScreen.Width / 2, rowHeight)
        {
            Text = $"HUMIDITY",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left
        });

        temperature = new Label(displayScreen.Width / 2 - rowMargin, rowHeight, displayScreen.Width / 2, rowHeight)
        {
            Text = $"- °C",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        dataLayout.Controls.Add(temperature);

        pressure = new Label(displayScreen.Width / 2 - rowMargin, rowHeight * 2, displayScreen.Width / 2, rowHeight)
        {
            Text = $"- mb",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        dataLayout.Controls.Add(pressure);

        humidity = new Label(displayScreen.Width / 2 - rowMargin, rowHeight * 3, displayScreen.Width / 2, rowHeight)
        {
            Text = $"- % ",
            TextColor = foregroundColor,
            Font = font12X20,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        dataLayout.Controls.Add(humidity);
    }

    public void ShowSplashScreen()
    {
        dataLayout.IsVisible = false;
        splashLayout.IsVisible = true;
    }

    public void ShowDataScreen()
    {
        splashLayout.IsVisible = false;
        dataLayout.IsVisible = true;
    }

    public void UpdateStatus(string status)
    {
        this.status.Text = status;
    }

    public void UpdateLastUpdated(string lastUpdated)
    {
        this.lastUpdated.Text = $"Last Updated: {lastUpdated}";
    }

    public void UpdateWiFiStatus(bool isConnected)
    {
        var imageWiFi = isConnected
            ? Image.LoadFromResource("ProjectLab_Logging.Resources.img_wifi_connected.bmp")
            : Image.LoadFromResource("ProjectLab_Logging.Resources.img_wifi_connecting.bmp");
        wifiStatus.Image = imageWiFi;
    }

    public void UpdateSyncStatus(bool isSyncing)
    {
        var imageSync = isSyncing
            ? Image.LoadFromResource("ProjectLab_Logging.Resources.img_refreshing.bmp")
            : Image.LoadFromResource("ProjectLab_Logging.Resources.img_refreshed.bmp");
        syncStatus.Image = imageSync;
    }

    public void UpdateAtmosphericConditions(string temperature, string pressure, string humidity)
    {
        displayScreen.BeginUpdate();

        this.temperature.Text = $"{temperature} °C";
        this.pressure.Text = $"{pressure} mb";
        this.humidity.Text = $"{humidity} % ";

        displayScreen.EndUpdate();
    }
}