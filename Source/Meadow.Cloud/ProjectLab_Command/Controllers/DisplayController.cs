using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;

namespace ProjectLab_Command.Controllers;

internal class DisplayController
{
    private readonly int rowHeight = 60;
    private readonly int rowMargin = 15;

    private Color backgroundColor = Color.FromHex("#F3F7FA");
    private Color foregroundColor = Color.White;

    private Font12x20 font12X20 = new Font12x20();
    private Font6x8 font6x8 = new Font6x8();

    private Image relayOn = Image.LoadFromResource("ProjectLab_Command.Resources.img_relay_on.bmp");
    private Image relayOff = Image.LoadFromResource("ProjectLab_Command.Resources.img_relay_off.bmp");

    private DisplayScreen displayScreen;

    private AbsoluteLayout splashLayout;
    private AbsoluteLayout dataLayout;

    private Picture wifiStatus;
    private Picture syncStatus;
    private Picture relayStatus0;
    private Picture relayStatus1;
    private Picture relayStatus2;
    private Picture relayStatus3;

    private Label status;
    private Label lastUpdated;

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

    void LoadSplashLayout()
    {
        splashLayout = new AbsoluteLayout(displayScreen, 0, 0, displayScreen.Width, displayScreen.Height)
        {
            IsVisible = false
        };

        var image = Image.LoadFromResource("ProjectLab_Command.Resources.img_meadow.bmp");
        var displayImage = new Picture(0, 0, displayScreen.Width, displayScreen.Height, image)
        {
            BackColor = Meadow.Color.FromHex("#B35E2C"),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        splashLayout.Controls.Add(displayImage);
    }

    void LoadDataLayout()
    {
        dataLayout = new AbsoluteLayout(displayScreen, 0, 0, displayScreen.Width, displayScreen.Height)
        {
            IsVisible = false
        };

        dataLayout.Controls.Add(new Box(0, 0, displayScreen.Width, rowHeight)
        {
            ForeColor = Meadow.Color.FromHex("844936")
        });

        var wifiImage = Image.LoadFromResource("ProjectLab_Command.Resources.img_wifi_connecting.bmp");
        wifiStatus = new Picture(displayScreen.Width - wifiImage.Width - rowMargin, 0, wifiImage.Width, rowHeight, wifiImage)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(wifiStatus);

        var syncImage = Image.LoadFromResource("ProjectLab_Command.Resources.img_refreshed.bmp");
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

        dataLayout.Controls.Add(new Box(0, rowHeight, displayScreen.Width, displayScreen.Height - rowHeight)
        {
            ForeColor = Meadow.Color.FromHex("B35E2C")
        });

        int relayWidth = 71;
        int relayHeight = 156;
        int margin = 12;
        int relaySpacing = 4;
        int smallMargin = 2;

        dataLayout.Controls.Add(new Box(
            margin,
            rowHeight + margin,
            relayWidth,
            relayHeight)
        {
            ForeColor = Meadow.Color.White,
            IsFilled = false
        });
        dataLayout.Controls.Add(new Label(
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
        relayStatus0 = new Picture(
            margin,
            rowHeight + margin + smallMargin * 2,
            relayWidth,
            relayHeight,
            relayOff)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(relayStatus0);

        dataLayout.Controls.Add(new Box(
            margin + relayWidth + relaySpacing,
            rowHeight + margin,
            relayWidth,
            relayHeight)
        {
            ForeColor = Meadow.Color.White,
            IsFilled = false
        });
        dataLayout.Controls.Add(new Label(
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
        relayStatus1 = new Picture(
            margin + relayWidth + relaySpacing,
            rowHeight + margin + smallMargin * 2,
            relayWidth,
            relayHeight,
            relayOff)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(relayStatus1);

        dataLayout.Controls.Add(new Box(
            margin + relayWidth * 2 + relaySpacing * 2,
            rowHeight + margin,
            relayWidth,
            relayHeight)
        {
            ForeColor = Meadow.Color.White,
            IsFilled = false
        });
        dataLayout.Controls.Add(new Label(
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
        relayStatus2 = new Picture(
            margin + relayWidth * 2 + relaySpacing * 2,
            rowHeight + margin + smallMargin * 2,
            relayWidth,
            relayHeight,
            relayOff)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(relayStatus2);

        dataLayout.Controls.Add(new Box(
            margin + relayWidth * 3 + relaySpacing * 3,
            rowHeight + margin,
            relayWidth,
            relayHeight)
        {
            ForeColor = Meadow.Color.White,
            IsFilled = false
        });
        dataLayout.Controls.Add(new Label(
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
        relayStatus3 = new Picture(
            margin + relayWidth * 3 + relaySpacing * 3,
            rowHeight + margin + smallMargin * 2,
            relayWidth,
            relayHeight,
            relayOff)
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        dataLayout.Controls.Add(relayStatus3);
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
        this.lastUpdated.Text = $"Last command: {lastUpdated}";
    }

    public void UpdateWiFiStatus(bool isConnected)
    {
        var imageWiFi = isConnected
            ? Image.LoadFromResource("ProjectLab_Command.Resources.img_wifi_connected.bmp")
            : Image.LoadFromResource("ProjectLab_Command.Resources.img_wifi_connecting.bmp");
        wifiStatus.Image = imageWiFi;
    }

    public void UpdateSyncStatus(bool isSyncing)
    {
        var imageSync = isSyncing
            ? Image.LoadFromResource("ProjectLab_Command.Resources.img_refreshing.bmp")
            : Image.LoadFromResource("ProjectLab_Command.Resources.img_refreshed.bmp");
        syncStatus.Image = imageSync;
    }

    public void UpdateRelayStatus(int relay, bool isOn)
    {
        var relayStatus = isOn ? relayOn : relayOff;

        switch (relay)
        {
            case 0:
                relayStatus0.Image = relayStatus;
                break;
            case 1:
                relayStatus1.Image = relayStatus;
                break;
            case 2:
                relayStatus2.Image = relayStatus;
                break;
            case 3:
                relayStatus3.Image = relayStatus;
                break;
            case 4:
                relayStatus0.Image = relayStatus;
                relayStatus1.Image = relayStatus;
                relayStatus2.Image = relayStatus;
                relayStatus3.Image = relayStatus;
                break;
        }
    }
}