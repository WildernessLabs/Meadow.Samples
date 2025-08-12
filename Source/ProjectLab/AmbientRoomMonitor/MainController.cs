using AmbientRoomMonitor.Hardware;
using AmbientRoomMonitor.Services;
using Meadow;
using System;

namespace AmbientRoomMonitor;

internal class MainController
{
    IAmbientRoomMonitorHardware hardware;

    DisplayController displayService;

    public MainController(IAmbientRoomMonitorHardware hardware)
    {
        this.hardware = hardware;
    }

    public void Initialize()
    {
        hardware.Initialize();

        displayService = new DisplayController(hardware.Display);

        hardware.BarometricPressureSensor.Updated += BarometricPressureSensor_Updated;
    }

    private void BarometricPressureSensor_Updated(object sender, IChangeResult<Meadow.Units.Pressure> e)
    {
        hardware.RgbPwmLed.StartBlink(Color.Orange);

        displayService.UpdateAtmosphericConditions(
            light: $"{hardware.LightSensor.Illuminance.Value.Lux:N0}",
            pressure: $"{e.New.Millibar:N0}",
            humidity: $"{hardware.HumiditySensor.Humidity.Value.Percent:N0}",
            temperature: $"{hardware.TemperatureSensor.Read().Result.Celsius:N0}");

        hardware.RgbPwmLed.StartBlink(Color.Green);
    }

    public void Run()
    {
        hardware.LightSensor.StartUpdating(TimeSpan.FromSeconds(5));
        hardware.HumiditySensor.StartUpdating(TimeSpan.FromSeconds(5));
    }
}