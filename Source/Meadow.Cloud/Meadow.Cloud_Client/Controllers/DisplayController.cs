using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System.Collections.Generic;

namespace Meadow.Cloud_Client.Controllers;

internal class DisplayController
{
    private readonly int rowHeight = 60;
    private readonly int graphHeight = 115;
    private readonly int axisLabelsHeight = 15;
    private readonly int margin = 15;

    private Color backgroundColor = Color.FromHex("14607F");
    private Color foregroundColor = Color.FromHex("10485E");
    private Color chartCurveColor = Color.FromHex("EF7D3B");
    private Color TextColor = Color.White;

    private Font12x20 font12X20 = new Font12x20();
    private Font8x12 font8x12 = new Font8x12();
    private Font6x8 font6x8 = new Font6x8();

    protected DisplayScreen DisplayScreen { get; set; }

    protected AbsoluteLayout SplashLayout { get; set; }

    protected AbsoluteLayout DataLayout { get; set; }

    protected LineChartSeries LineChartSeries { get; set; }

    protected LineChart LineChart { get; set; }

    protected Picture WifiStatus { get; set; }

    protected Picture SyncStatus { get; set; }

    protected Label Status { get; set; }

    protected Label LatestReading { get; set; }

    protected Label AxisLabels { get; set; }

    protected Label Temperature { get; set; }

    protected Label Pressure { get; set; }

    protected Label Humidity { get; set; }

    protected Label ConnectionErrorLabel { get; set; }

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

    private void LoadSplashLayout()
    {
        SplashLayout = new AbsoluteLayout(DisplayScreen, 0, 0, DisplayScreen.Width, DisplayScreen.Height)
        {
            IsVisible = false
        };

        var image = Image.LoadFromResource("Meadow.Cloud_Client.Resources.img_meadow.bmp");
        var displayImage = new Picture(0, 0, DisplayScreen.Width, DisplayScreen.Height, image)
        {
            BackColor = Meadow.Color.FromHex("14607F"),
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

        DataLayout.Controls.Add(new Box(0, 0, DisplayScreen.Width, rowHeight)
        {
            ForeColor = foregroundColor
        });

        Status = new Label(margin, 15, DisplayScreen.Width / 2, 20)
        {
            Text = $"--:-- -- --/--/--",
            TextColor = TextColor,
            Font = font12X20,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DataLayout.Controls.Add(Status);

        LatestReading = new Label(margin, 37, DisplayScreen.Width / 2, 8)
        {
            Text = $"Latest Reading: --:-- -- --/--/--",
            TextColor = TextColor,
            Font = font6x8,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        DataLayout.Controls.Add(LatestReading);

        var wifiImage = Image.LoadFromResource("Meadow.Cloud_Client.Resources.img_wifi_connecting.bmp");
        WifiStatus = new Picture(DisplayScreen.Width - wifiImage.Width - margin, 0, wifiImage.Width, rowHeight, wifiImage)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        DataLayout.Controls.Add(WifiStatus);

        var syncImage = Image.LoadFromResource("Meadow.Cloud_Client.Resources.img_refreshed.bmp");
        SyncStatus = new Picture(DisplayScreen.Width - syncImage.Width - wifiImage.Width - margin * 2, 0, syncImage.Width, rowHeight, syncImage)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        DataLayout.Controls.Add(SyncStatus);

        DataLayout.Controls.Add(new Box
            (margin,
            rowHeight + margin,
            DisplayScreen.Width - margin * 2,
            graphHeight + axisLabelsHeight)
        {
            ForeColor = foregroundColor
        });

        LineChart = new LineChart(
            margin,
            rowHeight + margin,
            DisplayScreen.Width - margin * 2,
            graphHeight)
        {
            BackgroundColor = foregroundColor,
            AxisColor = TextColor,
            ShowYAxisLabels = true,
            IsVisible = false,
            AlwaysShowYOrigin = false,
        };
        LineChartSeries = new LineChartSeries()
        {
            LineColor = chartCurveColor,
            PointColor = chartCurveColor,
            LineStroke = 1,
            PointSize = 2,
            ShowLines = true,
            ShowPoints = true,
        };
        LineChart.Series.Add(LineChartSeries);
        DataLayout.Controls.Add(LineChart);

        AxisLabels = new Label(
            margin,
            margin + rowHeight + graphHeight,
            DisplayScreen.Width - margin * 2,
            axisLabelsHeight)
        {
            Text = $"Y: Celcius | X: Every 30 minutes",
            TextColor = TextColor,
            BackColor = foregroundColor,
            Font = font6x8,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        DataLayout.Controls.Add(AxisLabels);

        Temperature = new Label(15, 205, 115, 20)
        {
            Text = $"TEMPERATURE",
            TextColor = TextColor,
            BackColor = foregroundColor,
            Font = font8x12,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        DataLayout.Controls.Add(Temperature);

        Pressure = new Label(130, 205, 89, 20)
        {
            Text = $"PRESSURE",
            TextColor = TextColor,
            BackColor = backgroundColor,
            Font = font8x12,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        DataLayout.Controls.Add(Pressure);

        Humidity = new Label(219, 205, 86, 20)
        {
            Text = $"HUMIDITY",
            TextColor = TextColor,
            BackColor = backgroundColor,
            Font = font8x12,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        DataLayout.Controls.Add(Humidity);

        ConnectionErrorLabel = new Label(
            (int)(DisplayScreen.Width * 0.25),
            DisplayScreen.Height / 2,
            (int)(DisplayScreen.Width * 0.60),
            20)
        {
            Text = "NO NETWORK CONNECTION",
            TextColor = TextColor,
            BackColor = foregroundColor,
            Font = font8x12,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        DataLayout.Controls.Add(ConnectionErrorLabel);
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

    public void UpdateLatestReading(string latestUpdate)
    {
        LatestReading.Text = $"Latest reading: {latestUpdate}";
    }

    public void UpdateGraph(int graphType, List<double> readings)
    {
        DisplayScreen.BeginUpdate();

        Temperature.BackColor = backgroundColor;
        Pressure.BackColor = backgroundColor;
        Humidity.BackColor = backgroundColor;

        switch (graphType)
        {
            case 0:
                Temperature.BackColor = foregroundColor;
                AxisLabels.Text = $"Y: Celcius | X: Every 30 minutes";
                break;
            case 1:
                Pressure.BackColor = foregroundColor;
                AxisLabels.Text = $"Y: Millibar | X: Every 30 minutes";
                break;
            case 2:
                Humidity.BackColor = foregroundColor;
                AxisLabels.Text = $"Y: Percent | X: Every 30 minutes";
                break;
        }

        LineChartSeries.Points.Clear();

        for (var p = 0; p < readings.Count; p++)
        {
            LineChartSeries.Points.Add(p * 2, readings[p]);
        }

        DisplayScreen.EndUpdate();
    }

    public void UpdateWiFiStatus(bool isConnected)
    {
        var imageWiFi = isConnected
            ? Image.LoadFromResource("Meadow.Cloud_Client.Resources.img_wifi_connected.bmp")
            : Image.LoadFromResource("Meadow.Cloud_Client.Resources.img_wifi_connecting.bmp");
        WifiStatus.Image = imageWiFi;

        if (!isConnected && LineChartSeries.Points.Count == 0)
        {
            ConnectionErrorLabel.IsVisible = true;
        }
        else
        {
            ConnectionErrorLabel.IsVisible = false;
        }
    }

    public void UpdateSyncStatus(bool isSyncing)
    {
        var imageSync = isSyncing
            ? Image.LoadFromResource("Meadow.Cloud_Client.Resources.img_refreshing.bmp")
            : Image.LoadFromResource("Meadow.Cloud_Client.Resources.img_refreshed.bmp");
        SyncStatus.Image = imageSync;
    }
}