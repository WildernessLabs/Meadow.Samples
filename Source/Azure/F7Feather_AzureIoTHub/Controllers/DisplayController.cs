using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using Meadow.Units;

namespace F7Feather_AzureIoTHub.Controllers;

public class DisplayController
{
    Color backgroundColor = Color.FromHex("#23ABE3");

    Font8x12 font8x12 = new Font8x12();
    Font12x20 font12X20 = new Font12x20();

    Image imgWifi = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_wifi.bmp");
    Image imgWifiFade = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_wifi_fade.bmp");
    Image imgSync = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_sync.bmp");
    Image imgSyncFade = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_sync_fade.bmp");

    DisplayScreen displayScreen;

    AbsoluteLayout SplashLayout;
    AbsoluteLayout DataLayout;

    Picture Wifi;
    Picture Sync;

    Label Title;
    Label Temperature;
    Label Humidity;

    public DisplayController(IPixelDisplay display)
    {
        displayScreen = new DisplayScreen(display, RotationType._90Degrees)
        {
            BackgroundColor = backgroundColor
        };

        LoadSplashLayout();

        displayScreen.Controls.Add(SplashLayout);

        LoadDataLayout();

        displayScreen.Controls.Add(DataLayout);
    }

    private void LoadSplashLayout()
    {
        SplashLayout = new AbsoluteLayout(displayScreen, 0, 0, displayScreen.Width, displayScreen.Height)
        {
            IsVisible = false
        };

        var logo = Image.LoadFromResource("MeadowAzureIoTHub.Resources.img_meadow.bmp");
        var displayImage = new Picture(
            55,
            60,
            logo.Width,
            logo.Height,
            logo)
        {
            BackColor = backgroundColor,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        SplashLayout.Controls.Add(displayImage);

        SplashLayout.Controls.Add(new Label(
            0,
            160,
            displayScreen.Width,
            font8x12.Height)
        {
            Text = $"Azure IoT Hub",
            TextColor = Color.Black,
            Font = font8x12,
            ScaleFactor = ScaleFactor.X2,
            HorizontalAlignment = HorizontalAlignment.Center,
        });
    }

    public void LoadDataLayout()
    {
        DataLayout = new AbsoluteLayout(displayScreen, 0, 0, displayScreen.Width, displayScreen.Height)
        {
            IsVisible = false
        };

        Sync = new Picture(
            15,
            15,
            imgSyncFade.Width,
            imgSyncFade.Height,
            imgSyncFade);
        DataLayout.Controls.Add(Sync);

        Title = new Label(
            60,
            20,
            120,
            26)
        {
            Text = "AMQP",
            TextColor = Color.Black,
            Font = font8x12,
            ScaleFactor = ScaleFactor.X2,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        DataLayout.Controls.Add(Title);

        Wifi = new Picture(
            195,
            15,
            imgWifiFade.Width,
            imgWifiFade.Height,
            imgWifiFade);
        DataLayout.Controls.Add(Wifi);

        DataLayout.Controls.Add(new Box(
            20,
            57,
            200,
            76)
        {
            ForeColor = Color.Black,
            IsFilled = false
        });

        DataLayout.Controls.Add(new Label(
            24,
            65,
            192,
            font8x12.Height * 2)
        {
            Text = "Temperature",
            TextColor = Color.Black,
            Font = font8x12,
            ScaleFactor = ScaleFactor.X2,
            HorizontalAlignment = HorizontalAlignment.Center
        });

        Temperature = new Label(
            24,
            93,
            192,
            font12X20.Height * 2)
        {
            Text = "--°C",
            TextColor = Color.Black,
            Font = font12X20,
            ScaleFactor = ScaleFactor.X2,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        DataLayout.Controls.Add(Temperature);

        DataLayout.Controls.Add(new Box(
            20,
            144,
            200,
            76)
        {
            ForeColor = Color.Black,
            IsFilled = false
        });

        DataLayout.Controls.Add(new Label(
            24,
            152,
            192,
            font8x12.Height * 2)
        {
            Text = "Humidity",
            TextColor = Color.Black,
            Font = font8x12,
            ScaleFactor = ScaleFactor.X2,
            HorizontalAlignment = HorizontalAlignment.Center
        });

        Humidity = new Label(
            24,
            180,
            192,
            font12X20.Height * 2)
        {
            Text = "--%",
            TextColor = Color.Black,
            Font = font12X20,
            ScaleFactor = ScaleFactor.X2,
            HorizontalAlignment = HorizontalAlignment.Center
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

    public void UpdateTitle(string title)
    {
        Title.Text = title;
    }

    public void UpdateWiFiStatus(bool isConnected)
    {
        Wifi.Image = isConnected
            ? imgWifi
            : imgWifiFade;
    }

    public void UpdateSyncStatus(bool isSyncing)
    {
        Sync.Image = isSyncing
            ? imgSync
            : imgSyncFade;
    }

    public void UpdateReadings((Temperature? Temperature, RelativeHumidity? Humidity) reading)
    {
        Temperature.Text = $"{reading.Temperature.Value.Celsius:N1}°C";

        Humidity.Text = $"{reading.Humidity.Value.Percent:N2}%";
    }
}