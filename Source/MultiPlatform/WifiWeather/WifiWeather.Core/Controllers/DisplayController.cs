using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System;
using System.Collections.Generic;

namespace WifiWeather.Core.Controllers;

public class DisplayController
{
    private readonly Color backgroundColor = Color.FromHex("10485E");
    private readonly Color outdoorColor = Color.FromHex("C9DB31");
    private readonly Color foregroundColor = Color.FromHex("EEEEEE");

    private readonly Font8x16 font8x16 = new Font8x16();
    private readonly Font6x8 font6x8 = new Font6x8();

    private readonly int margin = 5;
    private readonly int smallMargin = 3;
    private readonly int graphHeight = 105;
    private readonly int measureBoxWidth = 82;
    private readonly int columnWidth = 100;
    private readonly int rowHeight = 30;
    private readonly int row1 = 135;
    private readonly int row2 = 170;
    private readonly int row3 = 205;

    private int numberOfRequests = 0;

    private Image weatherIcon = Image.LoadFromResource($"WifiWeather.Core.Assets.w_misc.bmp");

    private DisplayScreen displayScreen;
    private AbsoluteLayout splashLayout;
    private AbsoluteLayout dataLayout;

    private LineChartSeries outdoorSeries;
    private LineChart lineChart;

    private Picture wifiStatus;
    private Picture syncStatus;
    private Picture weather;
    private Label status;
    private Label counter;

    private Box temperatureBox;
    private Label temperatureLabel;
    private Label temperatureValue;

    private Box pressureBox;
    private Label pressureLabel;
    private Label pressureValue;

    private Box humidityBox;
    private Label humidityLabel;
    private Label humidityValue;

    private Label feelsLike;
    private Label sunrise;
    private Label sunset;

    public DisplayController(IPixelDisplay display, RotationType rotation)
    {
        displayScreen = new DisplayScreen(display, rotation)
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

        var image = Image.LoadFromResource("WifiWeather.Core.Assets.img_meadow.bmp");
        var displayImage = new Picture(0, 0, displayScreen.Width, displayScreen.Height, image)
        {
            BackColor = Color.FromHex("#14607F"),
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

        status = new Label(
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
        dataLayout.Controls.Add(status);

        dataLayout.Controls.Add(new Box(
            226,
            margin + 2,
            44,
            14)
        {
            ForeColor = foregroundColor,
            IsFilled = false
        });

        counter = new Label(
            228,
            margin + 2,
            44,
            14)
        {
            Text = $"00000",
            TextColor = foregroundColor,
            Font = font6x8,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(counter);

        var wifiImage = Image.LoadFromResource("WifiWeather.Core.Assets.img_wifi_connecting.bmp");
        wifiStatus = new Picture(
            displayScreen.Width - wifiImage.Width - margin,
            margin,
            wifiImage.Width,
            font8x16.Height,
            wifiImage)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(wifiStatus);

        var syncImage = Image.LoadFromResource("WifiWeather.Core.Assets.img_refreshed.bmp");
        syncStatus = new Picture(
            displayScreen.Width - syncImage.Width - wifiImage.Width - margin * 2,
            margin,
            syncImage.Width,
            font8x16.Height,
            syncImage)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(syncStatus);

        lineChart = new LineChart(
            margin,
            25,
            displayScreen.Width - margin * 2,
            graphHeight)
        {
            BackgroundColor = Color.FromHex("082936"),
            AxisColor = foregroundColor,
            ShowYAxisLabels = true,
            IsVisible = false,
            AlwaysShowYOrigin = false,
        };
        outdoorSeries = new LineChartSeries()
        {
            LineColor = outdoorColor,
            PointColor = outdoorColor,
            LineStroke = 1,
            PointSize = 2,
            ShowLines = true,
            ShowPoints = true,
        };
        lineChart.Series.Add(outdoorSeries);
        dataLayout.Controls.Add(lineChart);

        var weatherImage = Image.LoadFromResource("WifiWeather.Core.Assets.w_misc.bmp");
        weather = new Picture(
            margin,
            row1,
            100,
            100,
            weatherImage)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(weather);

        #region TEMPERATURE
        temperatureBox = new Box(
            columnWidth + margin * 2,
            row1,
            columnWidth,
            rowHeight)
        {
            ForeColor = outdoorColor
        };
        dataLayout.Controls.Add(temperatureBox);
        temperatureLabel = new Label(
            columnWidth + margin * 2 + smallMargin,
            row1 + smallMargin,
            measureBoxWidth - smallMargin * 2,
            font6x8.Height)
        {
            Text = $"TEMPERATURE",
            TextColor = backgroundColor,
            Font = font6x8
        };
        dataLayout.Controls.Add(temperatureLabel);
        temperatureValue = new Label(
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
        dataLayout.Controls.Add(temperatureValue);
        #endregion

        #region PRESSURE
        pressureBox = new Box(
            columnWidth + margin * 2,
            row2,
            columnWidth,
            rowHeight)
        {
            ForeColor = backgroundColor
        };
        dataLayout.Controls.Add(pressureBox);
        pressureLabel = new Label(
            columnWidth + margin * 2 + smallMargin,
            row2 + smallMargin,
            measureBoxWidth - smallMargin * 2,
            font6x8.Height)
        {
            Text = $"PRESSURE",
            TextColor = foregroundColor,
            Font = font6x8
        };
        dataLayout.Controls.Add(pressureLabel);
        pressureValue = new Label(
            columnWidth + margin * 2 + smallMargin,
            row2 + font6x8.Height + smallMargin * 2,
            measureBoxWidth - smallMargin * 2,
            font6x8.Height * 2)
        {
            Text = $"-.-hPa",
            TextColor = foregroundColor,
            Font = font6x8,
            ScaleFactor = ScaleFactor.X2
        };
        dataLayout.Controls.Add(pressureValue);
        #endregion

        #region HUMIDITY
        humidityBox = new Box(
            columnWidth + margin * 2,
            row3,
            columnWidth,
            rowHeight)
        {
            ForeColor = backgroundColor
        };
        dataLayout.Controls.Add(humidityBox);
        humidityLabel = new Label(
            columnWidth + margin * 2 + smallMargin,
            row3 + smallMargin,
            measureBoxWidth - smallMargin * 2,
            font6x8.Height)
        {
            Text = $"HUMIDITY",
            TextColor = foregroundColor,
            Font = font6x8
        };
        dataLayout.Controls.Add(humidityLabel);
        humidityValue = new Label(
            columnWidth + margin * 2 + smallMargin,
            row3 + font6x8.Height + smallMargin * 2,
            columnWidth - smallMargin * 2,
            font6x8.Height * 2)
        {
            Text = $"-.-%",
            TextColor = foregroundColor,
            Font = font6x8,
            ScaleFactor = ScaleFactor.X2
        };
        dataLayout.Controls.Add(humidityValue);
        #endregion

        dataLayout.Controls.Add(new Label(
            columnWidth * 2 + margin * 3 + smallMargin,
            row1 + smallMargin,
            measureBoxWidth - smallMargin * 2,
            font6x8.Height)
        {
            Text = $"FEELS LIKE",
            TextColor = foregroundColor,
            Font = font6x8
        });
        feelsLike = new Label(
            columnWidth * 2 + margin * 3 + smallMargin,
            row1 + font6x8.Height + smallMargin * 2,
            columnWidth - smallMargin * 2,
            font6x8.Height * 2)
        {
            Text = $"-.-C",
            TextColor = foregroundColor,
            Font = font6x8,
            ScaleFactor = ScaleFactor.X2
        };
        dataLayout.Controls.Add(feelsLike);

        dataLayout.Controls.Add(new Label(
            columnWidth * 2 + margin * 3 + smallMargin,
            row2 + smallMargin,
            measureBoxWidth - smallMargin * 2,
            font6x8.Height)
        {
            Text = $"SUNRISE",
            TextColor = foregroundColor,
            Font = font6x8
        });
        sunrise = new Label(
            columnWidth * 2 + margin * 3 + smallMargin,
            row2 + font6x8.Height + smallMargin * 2,
            columnWidth - smallMargin * 2,
            font6x8.Height * 2)
        {
            Text = $"--:-- --",
            TextColor = foregroundColor,
            Font = font6x8,
            ScaleFactor = ScaleFactor.X2
        };
        dataLayout.Controls.Add(sunrise);

        dataLayout.Controls.Add(new Label(
            columnWidth * 2 + margin * 3 + smallMargin,
            row3 + smallMargin,
            measureBoxWidth - smallMargin * 2,
            font6x8.Height)
        {
            Text = $"SUNSET",
            TextColor = foregroundColor,
            Font = font6x8
        });
        sunset = new Label(
            columnWidth * 2 + margin * 3 + smallMargin,
            row3 + font6x8.Height + smallMargin * 2,
            columnWidth - smallMargin * 2,
            font6x8.Height * 2)
        {
            Text = $"--:-- --",
            TextColor = foregroundColor,
            Font = font6x8,
            ScaleFactor = ScaleFactor.X2
        };
        dataLayout.Controls.Add(sunset);
    }

    private void UpdateReadingType(int type)
    {
        temperatureBox.ForeColor = pressureBox.ForeColor = humidityBox.ForeColor = backgroundColor;
        temperatureLabel.TextColor = pressureLabel.TextColor = humidityLabel.TextColor = foregroundColor;
        temperatureValue.TextColor = pressureValue.TextColor = humidityValue.TextColor = foregroundColor;

        switch (type)
        {
            case 0:
                temperatureBox.ForeColor = outdoorColor;
                temperatureLabel.TextColor = backgroundColor;
                temperatureValue.TextColor = backgroundColor;
                break;
            case 1:
                pressureBox.ForeColor = outdoorColor;
                pressureLabel.TextColor = backgroundColor;
                pressureValue.TextColor = backgroundColor;
                break;
            case 2:
                humidityBox.ForeColor = outdoorColor;
                humidityLabel.TextColor = backgroundColor;
                humidityValue.TextColor = backgroundColor;
                break;
        }
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

    public void UpdateWiFiStatus(bool isConnected)
    {
        var imageWiFi = isConnected
            ? Image.LoadFromResource("WifiWeather.Core.Assets.img_wifi_connected.bmp")
            : Image.LoadFromResource("WifiWeather.Core.Assets.img_wifi_connecting.bmp");
        wifiStatus.Image = imageWiFi;
    }

    public void UpdateSyncStatus(bool isSyncing)
    {
        var imageSync = isSyncing
            ? Image.LoadFromResource("WifiWeather.Core.Assets.img_refreshing.bmp")
            : Image.LoadFromResource("WifiWeather.Core.Assets.img_refreshed.bmp");
        syncStatus.Image = imageSync;
    }

    public void UpdateGraph(int graphType, List<double> readings)
    {
        displayScreen.BeginUpdate();

        UpdateReadingType(graphType);

        outdoorSeries.Points.Clear();

        for (var p = 0; p < readings.Count; p++)
        {
            outdoorSeries.Points.Add(p * 2, readings[p]);
        }

        displayScreen.EndUpdate();
    }

    public void UpdateReadings(
        int readingType,
        string icon,
        double temperature,
        double pressure,
        double humidity,
        double feelsLike,
        DateTime sunrise,
        DateTime sunset)
    {
        displayScreen.BeginUpdate();

        numberOfRequests++;
        counter.Text = $"{numberOfRequests:D5}";

        UpdateReadingType(readingType);

        weatherIcon = Image.LoadFromResource(icon);
        weather.Image = weatherIcon;

        temperatureValue.Text = $"{temperature:N1}C";
        humidityValue.Text = $"{humidity:N1}%";
        pressureValue.Text = $"{pressure:N2}atm";
        this.feelsLike.Text = $"{feelsLike:N1}C";
        this.sunrise.Text = $"{sunrise:hh:mm tt}";
        this.sunset.Text = $"{sunset:hh:mm tt}";

        displayScreen.EndUpdate();
    }
}