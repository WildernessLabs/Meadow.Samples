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

        hardware.EnvironmentalSensor.Updated += EnvironmentalSensorUpdated;
    }

    private void EnvironmentalSensorUpdated(object sender, Meadow.IChangeResult<(Meadow.Units.Temperature? Temperature, Meadow.Units.RelativeHumidity? Humidity, Meadow.Units.Pressure? Pressure, Meadow.Units.Resistance? GasResistance)> e)
    {
        hardware.RgbPwmLed.StartBlink(Color.Orange);

        displayService.UpdateAtmosphericConditions(
            light: $"{hardware.LightSensor.Illuminance.Value.Lux:N0}",
            pressure: $"{e.New.Pressure.Value.Millibar:N0}",
            humidity: $"{e.New.Humidity.Value.Percent:N0}",
            temperature: $"{e.New.Temperature.Value.Celsius:N0}");

        hardware.RgbPwmLed.StartBlink(Color.Green);
    }

    public void Run()
    {
        hardware.LightSensor.StartUpdating(TimeSpan.FromSeconds(5));
        hardware.EnvironmentalSensor.StartUpdating(TimeSpan.FromSeconds(5));
    }
}