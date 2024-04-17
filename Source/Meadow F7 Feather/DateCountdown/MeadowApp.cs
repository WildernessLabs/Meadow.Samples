using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DateCountdown;

public class MeadowApp : App<F7FeatherV2>
{
    CharacterDisplay display;

    public override async Task Initialize()
    {
        var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
        onboardLed.SetColor(Color.Red);

        display = new CharacterDisplay
        (
            pinRS: Device.Pins.D10,
            pinE: Device.Pins.D09,
            pinD4: Device.Pins.D08,
            pinD5: Device.Pins.D07,
            pinD6: Device.Pins.D06,
            pinD7: Device.Pins.D05,
            rows: 4, columns: 20
        );
        ShowSplashScreen();

        var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));

        onboardLed.SetColor(Color.Green);
    }

    void ShowSplashScreen()
    {
        display.WriteLine("====================", 0);
        display.WriteLine(" Holidays Countdown ", 1);
        display.WriteLine("=   Joining WIFI   =", 2);
        display.WriteLine("====================", 3);
    }

    void UpdateCountdown()
    {
        int TimeZoneOffSet = -8; // PST
        var today = DateTime.Now.AddHours(TimeZoneOffSet);

        display.WriteLine($"{today.ToString("MMMM dd, yyyy")}", 0);
        display.WriteLine($"{today.ToString("hh:mm:ss tt")}", 1);

        var christmasDate = new DateTime(today.Year, 12, 25);
        if (christmasDate < today)
            christmasDate = new DateTime(today.Year + 1, 12, 25);

        var countdown = christmasDate.Subtract(today);
        display.WriteLine($"  {countdown.Days.ToString("D3")}d {countdown.Hours.ToString("D2")}h {countdown.Minutes.ToString("D2")}m {countdown.Seconds.ToString("D2")}s", 3);
    }

    public override Task Run()
    {
        display.WriteLine($"{DateTime.Now.ToString("MMMM dd, yyyy")}", 0);
        display.WriteLine("Christmas Countdown:", 2);

        while (true)
        {
            UpdateCountdown();
            Thread.Sleep(1000);
        }

        return base.Run();
    }
}