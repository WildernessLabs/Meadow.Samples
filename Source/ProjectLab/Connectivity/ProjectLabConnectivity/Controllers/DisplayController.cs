using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectLabConnectivity.Controllers;

public class DisplayController
{
    private CancellationTokenSource token;

    private Image logo = Image.LoadFromResource("ProjectLabConnectivity.Resources.img_meadow.bmp");

    private Color backgroundColor = Color.FromHex("23ABE3");
    private Color foregroundColor = Color.Black;

    private readonly Font12x16 font12X16 = new Font12x16();

    private DisplayScreen displayScreen;

    private AbsoluteLayout splashLayout;
    private AbsoluteLayout dataLayout;

    private Picture connectionImage;
    private Label title;
    private Label subtitle;
    private Label status;

    public DisplayController(IPixelDisplay display)
    {
        displayScreen = new DisplayScreen(display, RotationType._270Degrees)
        {
            BackgroundColor = backgroundColor
        };

        LoadSplashLayout();
        displayScreen.Controls.Add(splashLayout);

        Thread.Sleep(3000);

        LoadDataLayout();
        displayScreen.Controls.Add(dataLayout);
    }

    private void LoadSplashLayout()
    {
        splashLayout = new AbsoluteLayout(displayScreen, 0, 0, displayScreen.Width, displayScreen.Height);

        var image = Image.LoadFromResource("ProjectLabConnectivity.Resources.img_meadow.bmp");
        var displayImage = new Picture(0, 0, splashLayout.Width, splashLayout.Height, image)
        {
            BackColor = backgroundColor,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        splashLayout.Controls.Add(displayImage);
    }

    private void LoadDataLayout()
    {
        dataLayout = new AbsoluteLayout(displayScreen, 0, 0, displayScreen.Width, displayScreen.Height)
        {
            IsVisible = false
        };

        dataLayout.Controls.Add(new Circle(dataLayout.Width / 2, dataLayout.Height / 2, 150)
        {
            IsFilled = false
        });

        var image = Image.LoadFromResource("ProjectLabConnectivity.Resources.img_wifi.bmp");
        connectionImage = new Picture(117, 35, 86, 74, image);
        dataLayout.Controls.Add(connectionImage);

        title = CreateLabel(0, 125, dataLayout.Width, font12X16.Height * 2, "MAPLE", ScaleFactor.X2);
        subtitle = CreateLabel(0, 168, dataLayout.Width, font12X16.Height, "STARTING", ScaleFactor.X1);
        status = CreateLabel(0, 193, dataLayout.Width, font12X16.Height, "-", ScaleFactor.X1);

        dataLayout.Controls.Add(new[] { title, subtitle, status });
    }

    private Label CreateLabel(int x, int y, int width, int height, string text, ScaleFactor scaleFactor)
    {
        return new Label(x, y, width, height)
        {
            Text = text,
            TextColor = foregroundColor,
            Font = font12X16,
            ScaleFactor = scaleFactor,
            HorizontalAlignment = HorizontalAlignment.Center
        };
    }

    public async Task StartConnectingMapleAnimation()
    {
        splashLayout.IsVisible = false;
        dataLayout.IsVisible = true;

        token = new CancellationTokenSource();

        var connecting = Image.LoadFromResource("ProjectLabConnectivity.Resources.img_wifi_fade.bmp");
        var connected = Image.LoadFromResource("ProjectLabConnectivity.Resources.img_wifi.bmp");

        title.Text = "MAPLE";
        subtitle.Text = "STARTING";
        status.Text = "-";

        bool alternateImg = false;

        while (!token.IsCancellationRequested)
        {
            alternateImg = !alternateImg;

            connectionImage.Image = alternateImg
                ? connecting
                : connected;

            await Task.Delay(500);
        }
    }

    public async Task StartConnectingBluetoothAnimation()
    {
        splashLayout.IsVisible = false;
        dataLayout.IsVisible = true;

        token = new CancellationTokenSource();

        var connecting = Image.LoadFromResource("ProjectLabConnectivity.Resources.img_ble_fade.bmp");
        var connected = Image.LoadFromResource("ProjectLabConnectivity.Resources.img_ble.bmp");

        title.Text = "BLE";
        subtitle.Text = "DISCOVERABLE";
        status.Text = "-";

        bool alternateImg = false;

        while (!token.IsCancellationRequested)
        {
            alternateImg = !alternateImg;

            connectionImage.Image = alternateImg
                ? connecting
                : connected;

            await Task.Delay(500);
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

    public void ShowMapleReady(string ipAddress)
    {
        token.Cancel();

        var connected = Image.LoadFromResource("ProjectLabConnectivity.Resources.img_wifi.bmp");

        connectionImage.Image = connected;

        subtitle.Text = ipAddress;
        status.Text = "READY";
    }

    public void ShowBluetoothPaired()
    {
        token.Cancel();

        var connected = Image.LoadFromResource("ProjectLabConnectivity.Resources.img_ble.bmp");

        connectionImage.Image = connected;

        subtitle.Text = "PAIRED";
        status.Text = "READY";
    }
}