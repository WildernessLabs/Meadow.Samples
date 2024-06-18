using AmbientMonitor.Core.Contracts;
using AmbientMonitor.Core.Controllers;
using Meadow;
using Meadow.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmbientMonitor.Core;

public class MainController
{
    int TIMEZONE_OFFSET = -7; // UTC-8

    private IAmbientMonitorHardware? hardware;
    private DisplayController displayController;
    private InputController inputController;

    private int currentGraphType = 0;

    private List<double> temperatureReadings = new List<double>();
    private List<double> pressureReadings = new List<double>();
    private List<double> humidityReadings = new List<double>();

    public MainController() { }

    public Task Initialize(IAmbientMonitorHardware hardware)
    {
        this.hardware = hardware;

        var cloudLogger = new CloudLogger();
        Resolver.Log.AddProvider(cloudLogger);
        Resolver.Services.Add(cloudLogger);

        inputController = new InputController(hardware);
        inputController.leftButtonPressed += LeftButtonPressed;
        inputController.rightButtonPressed += RightButtonPressed;

        displayController = new DisplayController(
            this.hardware.Display,
            this.hardware.DisplayRotation);
        displayController.ShowSplashScreen();
        Thread.Sleep(3000);
        displayController.ShowDataScreen();

        return Task.CompletedTask;
    }

    private void LeftButtonPressed(object sender, EventArgs e)
    {
        currentGraphType = currentGraphType - 1 < 0 ? 2 : currentGraphType - 1;

        UpdateGraph();
    }
    private void RightButtonPressed(object sender, EventArgs e)
    {
        currentGraphType = currentGraphType + 1 > 2 ? 0 : currentGraphType + 1;

        UpdateGraph();
    }

    private void UpdateGraph()
    {
        switch (currentGraphType)
        {
            case 0:
                displayController.UpdateGraph(currentGraphType, temperatureReadings);
                break;
            case 1:
                displayController.UpdateGraph(currentGraphType, pressureReadings);
                break;
            case 2:
                displayController.UpdateGraph(currentGraphType, humidityReadings);
                break;
        }
    }

    private void RecordSensor()
    {
        var temperature = hardware?.TemperatureSensor?.Temperature;
        if (temperature != null)
        {
            temperatureReadings.Add(temperature.Value.Celsius);
            if (temperatureReadings.Count > 10)
            {
                temperatureReadings.RemoveAt(0);
            }
        }

        var pressure = hardware?.BarometricPressureSensor?.Pressure;
        if (pressure != null)
        {
            pressureReadings.Add(pressure.Value.Millibar);
            if (pressureReadings.Count > 10)
            {
                pressureReadings.RemoveAt(0);
            }
        }

        var humidity = hardware?.HumiditySensor?.Humidity;
        if (humidity != null)
        {
            humidityReadings.Add(humidity.Value.Percent);
            if (humidityReadings.Count > 10)
            {
                humidityReadings.RemoveAt(0);
            }
        }

        if (hardware.NetworkAdapter.IsConnected)
        {
            displayController.UpdateSyncStatus(true);
            displayController.UpdateStatus("Sending data...");
            Thread.Sleep(2000);

            var cloudLogger = Resolver.Services.Get<CloudLogger>();
            cloudLogger?.LogEvent(1000, "environment reading", new Dictionary<string, object>()
            {
                { "temperature", $"{(temperature?.Celsius)}" },
                { "pressure", $"{pressure?.Millibar:N2}" },
                { "humidity", $"{humidity?.Percent:N2}" },
            });

            displayController.UpdateStatus("Data sent!");
            Thread.Sleep(2000);
            displayController.UpdateSyncStatus(false);
            displayController.UpdateStatus(DateTime.Now.AddHours(TIMEZONE_OFFSET).ToString("hh:mm tt dd/MM/yy"));

            displayController.UpdateLatestReading(DateTime.Now.AddHours(TIMEZONE_OFFSET).ToString("hh:mm tt dd/MM/yy"));

            UpdateGraph();
        }
        else
        {
            displayController.UpdateStatus("Offline...");
        }
    }

    public async Task Run()
    {
        hardware?.TemperatureSensor.StartUpdating(TimeSpan.FromMinutes(1));
        hardware?.BarometricPressureSensor.StartUpdating(TimeSpan.FromMinutes(1));
        hardware?.HumiditySensor.StartUpdating(TimeSpan.FromMinutes(1));

        while (true)
        {
            displayController.UpdateWiFiStatus(hardware.NetworkAdapter.IsConnected);

            if (hardware.NetworkAdapter.IsConnected)
            {
                displayController.UpdateStatus(DateTime.Now.AddHours(TIMEZONE_OFFSET).ToString("hh:mm tt dd/MM/yy"));

                RecordSensor();

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
            else
            {
                displayController.UpdateStatus("Offline...");

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}