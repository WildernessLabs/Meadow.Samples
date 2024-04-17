using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System.Collections.Generic;

namespace ProjectLab_ApiClient.Controllers;

internal class DisplayController
{
    private readonly int rowHeight = 60;
    private readonly int graphHeight = 115;
    private readonly int axisLabelsHeight = 15;
    private readonly int margin = 15;

    private Color TextColor = Color.White;
    private Color backgroundColor = Color.FromHex("14607F");
    private Color foregroundColor = Color.FromHex("10485E");
    private Color chartCurveColor = Color.FromHex("EF7D3B");

    private Font6x8 font6x8 = new Font6x8();
    private Font8x12 font8x12 = new Font8x12();
    private Font12x20 font12X20 = new Font12x20();

    private DisplayScreen displayScreen;

    private AbsoluteLayout splashLayout;
    private AbsoluteLayout dataLayout;

    private LineChartSeries lineChartSeries;
    private LineChart lineChart;

    private Picture wifiStatus;
    private Picture syncStatus;

    private Label status;
    private Label latestReading;
    private Label axisLabels;
    private Label temperature;
    private Label pressure;
    private Label humidity;
    private Label connectionErrorLabel;

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
        splashLayout = new AbsoluteLayout(displayScreen, 0, 0, displayScreen.Width, displayScreen.Height)
        {
            IsVisible = false
        };

        var image = Image.LoadFromResource("ProjectLab_ApiClient.Resources.img_meadow.bmp");
        var displayImage = new Picture(0, 0, displayScreen.Width, displayScreen.Height, image)
        {
            BackColor = Meadow.Color.FromHex("14607F"),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        splashLayout.Controls.Add(displayImage);
    }

    private void LoadDataLayout()
    {
        dataLayout = new AbsoluteLayout(displayScreen, 0, 0, displayScreen.Width, displayScreen.Height)
        {
            BackgroundColor = backgroundColor,
            IsVisible = false
        };

        dataLayout.Controls.Add(new Box(0, 0, displayScreen.Width, rowHeight)
        {
            ForeColor = foregroundColor
        });

        status = new Label(margin, 15, displayScreen.Width / 2, 20)
        {
            Text = $"--:-- -- --/--/--",
            TextColor = TextColor,
            Font = font12X20,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        dataLayout.Controls.Add(status);

        latestReading = new Label(margin, 37, displayScreen.Width / 2, 8)
        {
            Text = $"Latest Reading: --:-- -- --/--/--",
            TextColor = TextColor,
            Font = font6x8,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        dataLayout.Controls.Add(latestReading);

        var wifiImage = Image.LoadFromResource("ProjectLab_ApiClient.Resources.img_wifi_connecting.bmp");
        wifiStatus = new Picture(displayScreen.Width - wifiImage.Width - margin, 0, wifiImage.Width, rowHeight, wifiImage)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(wifiStatus);

        var syncImage = Image.LoadFromResource("ProjectLab_ApiClient.Resources.img_refreshed.bmp");
        syncStatus = new Picture(displayScreen.Width - syncImage.Width - wifiImage.Width - margin * 2, 0, syncImage.Width, rowHeight, syncImage)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(syncStatus);

        dataLayout.Controls.Add(new Box
            (margin,
            rowHeight + margin,
            displayScreen.Width - margin * 2,
            graphHeight + axisLabelsHeight)
        {
            ForeColor = foregroundColor
        });

        lineChart = new LineChart(
            margin,
            rowHeight + margin,
            displayScreen.Width - margin * 2,
            graphHeight)
        {
            BackgroundColor = foregroundColor,
            AxisColor = TextColor,
            ShowYAxisLabels = true,
            IsVisible = false,
            AlwaysShowYOrigin = false,
        };
        lineChartSeries = new LineChartSeries()
        {
            LineColor = chartCurveColor,
            PointColor = chartCurveColor,
            LineStroke = 1,
            PointSize = 2,
            ShowLines = true,
            ShowPoints = true,
        };
        lineChart.Series.Add(lineChartSeries);
        dataLayout.Controls.Add(lineChart);

        axisLabels = new Label(
            margin,
            margin + rowHeight + graphHeight,
            displayScreen.Width - margin * 2,
            axisLabelsHeight)
        {
            Text = $"Y: Celcius | X: Every 30 minutes",
            TextColor = TextColor,
            BackColor = foregroundColor,
            Font = font6x8,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        dataLayout.Controls.Add(axisLabels);

        temperature = new Label(15, 205, 115, 20)
        {
            Text = $"TEMPERATURE",
            TextColor = TextColor,
            BackColor = foregroundColor,
            Font = font8x12,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        dataLayout.Controls.Add(temperature);

        pressure = new Label(130, 205, 89, 20)
        {
            Text = $"PRESSURE",
            TextColor = TextColor,
            BackColor = backgroundColor,
            Font = font8x12,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        dataLayout.Controls.Add(pressure);

        humidity = new Label(219, 205, 86, 20)
        {
            Text = $"HUMIDITY",
            TextColor = TextColor,
            BackColor = backgroundColor,
            Font = font8x12,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        dataLayout.Controls.Add(humidity);

        connectionErrorLabel = new Label(
            (int)(displayScreen.Width * 0.25),
            displayScreen.Height / 2,
            (int)(displayScreen.Width * 0.60),
            20)
        {
            Text = "NO NETWORK CONNECTION",
            TextColor = TextColor,
            BackColor = foregroundColor,
            Font = font8x12,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        dataLayout.Controls.Add(connectionErrorLabel);
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

    public void UpdateLatestReading(string latestUpdate)
    {
        latestReading.Text = $"Latest reading: {latestUpdate}";
    }

    public void UpdateGraph(int graphType, List<double> readings)
    {
        displayScreen.BeginUpdate();

        temperature.BackColor = backgroundColor;
        pressure.BackColor = backgroundColor;
        humidity.BackColor = backgroundColor;

        switch (graphType)
        {
            case 0:
                temperature.BackColor = foregroundColor;
                axisLabels.Text = $"Y: Celcius | X: Every 30 minutes";
                break;
            case 1:
                pressure.BackColor = foregroundColor;
                axisLabels.Text = $"Y: Millibar | X: Every 30 minutes";
                break;
            case 2:
                humidity.BackColor = foregroundColor;
                axisLabels.Text = $"Y: Percent | X: Every 30 minutes";
                break;
        }

        lineChartSeries.Points.Clear();

        for (var p = 0; p < readings.Count; p++)
        {
            lineChartSeries.Points.Add(p * 2, readings[p]);
        }

        displayScreen.EndUpdate();
    }

    public void UpdateWiFiStatus(bool isConnected)
    {
        var imageWiFi = isConnected
            ? Image.LoadFromResource("ProjectLab_ApiClient.Resources.img_wifi_connected.bmp")
            : Image.LoadFromResource("ProjectLab_ApiClient.Resources.img_wifi_connecting.bmp");
        wifiStatus.Image = imageWiFi;

        if (!isConnected && lineChartSeries.Points.Count == 0)
        {
            connectionErrorLabel.IsVisible = true;
        }
        else
        {
            connectionErrorLabel.IsVisible = false;
        }
    }

    public void UpdateSyncStatus(bool isSyncing)
    {
        var imageSync = isSyncing
            ? Image.LoadFromResource("ProjectLab_ApiClient.Resources.img_refreshing.bmp")
            : Image.LoadFromResource("ProjectLab_ApiClient.Resources.img_refreshed.bmp");
        syncStatus.Image = imageSync;
    }
}