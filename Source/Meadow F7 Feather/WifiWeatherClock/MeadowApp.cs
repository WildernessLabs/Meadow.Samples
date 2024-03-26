using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;
using WifiWeatherClock.Services;
using WifiWeatherClock.ViewModels;
using WifiWeatherClock.Views;

namespace WifiWeatherClock
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;
        DisplayView displayView;
        AnalogTemperature analogTemperature;

        public override async Task Initialize()
        {
            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            displayView = new DisplayView();

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
            wifi.NetworkConnected += NetworkConnected;

            await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));

            analogTemperature = new AnalogTemperature(Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35);
            await analogTemperature.Read();
            analogTemperature.StartUpdating(TimeSpan.FromMinutes(5));

            onboardLed.SetColor(Color.Green);
        }

        private async void NetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
        {
            await GetTemperature();

            while (true)
            {
                int TimeZoneOffSet = -7; // PST
                var datetime = DateTime.Now.AddHours(TimeZoneOffSet);

                if (datetime.Minute == 0 && datetime.Second == 0)
                {
                    await GetTemperature();
                }

                displayView.WriteLine($"{datetime.ToString("ddd, MMM dd, yyyy")}", 0);
                displayView.WriteLine($"{datetime.ToString("hh:mm:ss tt")}", 1);
                await Task.Delay(1000);
            }
        }

        async Task GetTemperature()
        {
            onboardLed.StartPulse(Color.Orange);

            // Get outdoor conditions
            var outdoorConditions = await WeatherService.GetWeatherForecast();

            // Format indoor/outdoor conditions data
            var model = new WeatherViewModel(outdoorConditions, analogTemperature.Temperature);

            // Update Temperature values and weather description
            displayView.WriteLine($"In: {model.IndoorTemperature.ToString("00")}C | Out: {model.OutdoorTemperature.ToString("00")}C", 2);
            displayView.WriteLine($"{model.Weather}", 3);

            onboardLed.StartPulse(Color.Green);
        }
    }
}