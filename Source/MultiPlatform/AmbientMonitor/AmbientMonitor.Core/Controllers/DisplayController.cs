using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System.Collections.Generic;

namespace AmbientMonitor.Core.Controllers;

public class DisplayController
{
    private readonly int rowHeight = 60;
    private readonly int graphHeight = 115;
    private readonly int axisLabelsHeight = 15;
    private readonly int margin = 15;

    private Color TextColor = Color.White;
    private Color backgroundColor = Color.FromHex("575E3C");
    private Color foregroundColor = Color.FromHex("323626");
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

    public DisplayController(
        IPixelDisplay? display,
        RotationType displayRotation)
    {
        displayScreen = new DisplayScreen(display, displayRotation)
        {
            BackgroundColor = backgroundColor
        };

        displayScreen.BeginUpdate();

        LoadSplashLayout();
        LoadDataLayout();

        displayScreen.Controls.Add(splashLayout!, dataLayout!);

        displayScreen.EndUpdate();
    }

    private void LoadSplashLayout()
    {
        splashLayout = new AbsoluteLayout(0, 0, displayScreen.Width, displayScreen.Height)
        {
            IsVisible = false
        };

        var image = Image.LoadFromResource("AmbientMonitor.Core.Assets.img_meadow.bmp");
        var displayImage = new Picture(0, 0, displayScreen.Width, displayScreen.Height, image)
        {
            BackgroundColor = Meadow.Color.FromHex("575E3C"),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        splashLayout.Controls.Add(displayImage);
    }

    private void LoadDataLayout()
    {
        dataLayout = new AbsoluteLayout(0, 0, displayScreen.Width, displayScreen.Height)
        {
            BackgroundColor = backgroundColor,
            IsVisible = false
        };

        dataLayout.Controls.Add(new Box(0, 0, displayScreen.Width, rowHeight)
        {
            ForegroundColor = foregroundColor
        });

        status = new Label(margin, 15, displayScreen.Width / 2, 20)
        {
            Text = "--:-- -- --/--/--",
            TextColor = TextColor,
            Font = font12X20,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        dataLayout.Controls.Add(status);

        latestReading = new Label(margin, 37, displayScreen.Width / 2, 8)
        {
            Text = "Latest Reading: --:-- -- --/--/--",
            TextColor = TextColor,
            Font = font6x8,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        dataLayout.Controls.Add(latestReading);

        var wifiImage = Image.LoadFromResource("AmbientMonitor.Core.Assets.img-wifi-fade.bmp");
        wifiStatus = new Picture(displayScreen.Width - wifiImage.Width - margin, 0, wifiImage.Width, rowHeight, wifiImage)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(wifiStatus);

        var syncImage = Image.LoadFromResource("AmbientMonitor.Core.Assets.img-sync-fade.bmp");
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
            ForegroundColor = foregroundColor
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
            Text = "Y: Celcius | X: Every minute",
            TextColor = TextColor,
            BackgroundColor = foregroundColor,
            Font = font6x8,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        dataLayout.Controls.Add(axisLabels);

        temperature = new Label(15, 205, 115, 20)
        {
            Text = "TEMPERATURE",
            TextColor = TextColor,
            BackgroundColor = foregroundColor,
            Font = font8x12,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        dataLayout.Controls.Add(temperature);

        pressure = new Label(130, 205, 89, 20)
        {
            Text = "PRESSURE",
            TextColor = TextColor,
            BackgroundColor = backgroundColor,
            Font = font8x12,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        dataLayout.Controls.Add(pressure);

        humidity = new Label(219, 205, 86, 20)
        {
            Text = "HUMIDITY",
            TextColor = TextColor,
            BackgroundColor = backgroundColor,
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
            BackgroundColor = foregroundColor,
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

        temperature.BackgroundColor = backgroundColor;
        pressure.BackgroundColor = backgroundColor;
        humidity.BackgroundColor = backgroundColor;

        switch (graphType)
        {
            case 0:
                temperature.BackgroundColor = foregroundColor;
                axisLabels.Text = "Y: Celcius | X: Every minute";
                break;
            case 1:
                pressure.BackgroundColor = foregroundColor;
                axisLabels.Text = "Y: Millibar | X: Every minute";
                break;
            case 2:
                humidity.BackgroundColor = foregroundColor;
                axisLabels.Text = "Y: Percent | X: Every minute";
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
            ? Image.LoadFromResource("AmbientMonitor.Core.Assets.img-wifi.bmp")
            : Image.LoadFromResource("AmbientMonitor.Core.Assets.img-wifi-fade.bmp");
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

    public void UpdateSyncStatus(bool isSynced)
    {
        var imageSync = isSynced
            ? Image.LoadFromResource("AmbientMonitor.Core.Assets.img-sync.bmp")
            : Image.LoadFromResource("AmbientMonitor.Core.Assets.img-sync-fade.bmp");
        syncStatus.Image = imageSync;
    }
}