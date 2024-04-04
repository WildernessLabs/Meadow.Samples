using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Web.Maple;
using Meadow.Gateways;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Light;
using Meadow.Peripherals.Sensors.Motion;
using Meadow.Units;
using MeadowConnectedSample.Connectivity;
using System;
using System.Threading.Tasks;

namespace MeadowConnectedSample.Controllers;

public class MainController
{
    // Connect via Maple (WiFi) or Bluetooth? 
    private bool useWifi = false;

    private IProjectLabHardware hardware;
    private IWiFiNetworkAdapter wifi;
    private IBluetoothAdapter bluetooth;

    private ITemperatureSensor temperatureSensor;
    private IBarometricPressureSensor pressureSensor;
    private IHumiditySensor humiditySensor;
    private ILightSensor lightSensor;
    private IGyroscope gyroscope;
    private IAccelerometer accelerometer;

    private BluetoothServer bluetoothServer;

    private DisplayController displayController;
    private LedController ledController;

    public MainController(IProjectLabHardware hardware, IWiFiNetworkAdapter wifi, IBluetoothAdapter bluetooth)
    {
        this.hardware = hardware;
        this.wifi = wifi;
        this.bluetooth = bluetooth;
    }

    public async Task Initialize()
    {
        temperatureSensor = hardware.TemperatureSensor;
        pressureSensor = hardware.BarometricPressureSensor;
        humiditySensor = hardware.HumiditySensor;
        lightSensor = hardware.LightSensor;
        gyroscope = hardware.Gyroscope;
        accelerometer = hardware.Accelerometer;

        displayController = new DisplayController(hardware.Display);
        displayController.ShowSplashScreen();

        ledController = new LedController(hardware.RgbLed);

        _ = displayController.StartConnectingAnimation(useWifi);

        if (useWifi)
        {
            wifi.NetworkConnected += WifiNetworkConnected;
            await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
        }
        else
        {
            bluetoothServer = new BluetoothServer();

            bluetoothServer.PairingValueSet += (s, e) =>
            {
                Resolver.Log.Info("PairingValueSet");
                displayController.ShowBluetoothPaired();
            };
            bluetoothServer.LedToggleValueSet += (s, e) =>
            {
                Resolver.Log.Info("LedToggleValueSet");
                _ = ledController.Toggle();
            };
            bluetoothServer.LedBlinkValueSet += (s, e) =>
            {
                Resolver.Log.Info("LedBlinkValueSet");
                _ = ledController.StartBlink();
            };
            bluetoothServer.LedPulseValueSet += (s, e) =>
            {
                Resolver.Log.Info("LedPulseValueSet");
                _ = ledController.StartPulse();
            };

            var definition = bluetoothServer.GetDefinition();
            bluetooth.StartBluetoothServer(definition);

            _ = StartUpdating(TimeSpan.FromSeconds(15));

            ledController.SetColor(Color.Green);
        }
    }

    private void WifiNetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
    {
        displayController.StopConnectingAnimation();

        _ = StartUpdating(TimeSpan.FromSeconds(15));

        var mapleServer = new MapleServer(sender.IpAddress, 5417, advertise: true, logger: Resolver.Log);
        mapleServer.Start();

        displayController.ShowMapleReady(sender.IpAddress.ToString());

        ledController.SetColor(Color.Green);
    }

    public async Task StartUpdating(TimeSpan updateInterval)
    {
        while (true)
        {
            var temperature = await temperatureSensor.Read();
            var pressure = await pressureSensor.Read();
            var humidity = await humiditySensor.Read();
            var illuminance = await lightSensor.Read();
            var angularVelocityReading = await gyroscope.Read();
            var acceleration3DReading = await accelerometer.Read();

            Resolver.Log.Info($"" +
                $"Temperature: {temperature.Celsius:N1} | " +
                $"Pressure: {pressure.StandardAtmosphere:N1} | " +
                $"Humidity: {humidity.Percent:N1} | " +
                $"Illuminance: {illuminance.Lux:N1} | " +
                $"Acceleration3D: ({acceleration3DReading.X.CentimetersPerSecondSquared:N1}, {acceleration3DReading.Y.CentimetersPerSecondSquared:N1}, {acceleration3DReading.Z.CentimetersPerSecondSquared:N1}) | " +
                $"AngularVelocity3D: ({angularVelocityReading.X.RevolutionsPerMinute:N1},{angularVelocityReading.Y.RevolutionsPerMinute:N1},{angularVelocityReading.Z.RevolutionsPerMinute:N1}) ");

            AtmosphericConditions.Temperature = temperature;
            AtmosphericConditions.Pressure = pressure;
            AtmosphericConditions.Humidity = humidity;

            LightConditions.Illuminance = illuminance;

            MotionConditions.Acceleration3D = acceleration3DReading;
            MotionConditions.AngularVelocity3D = angularVelocityReading;

            if (!useWifi)
            {
                bluetoothServer.UpdateAtmosphericConditions();
                bluetoothServer.UpdateLightConditions();
                bluetoothServer.UpdateMotionConditions();
            }

            await Task.Delay(updateInterval);
        }
    }
}

public static class AtmosphericConditions
{
    public static Temperature Temperature { get; set; }
    public static Pressure Pressure { get; set; }
    public static RelativeHumidity Humidity { get; set; }
}

public static class LightConditions
{
    public static Illuminance Illuminance { get; set; }
}

public static class MotionConditions
{
    public static Acceleration3D Acceleration3D { get; set; }
    public static AngularVelocity3D AngularVelocity3D { get; set; }
}