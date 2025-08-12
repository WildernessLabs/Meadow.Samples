﻿using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Light;

namespace AmbientRoomMonitor.Hardware;

internal interface IAmbientRoomMonitorHardware
{
    public IPixelDisplay Display { get; }

    public ILightSensor LightSensor { get; }

    public ITemperatureSensor TemperatureSensor { get; set; }

    public IBarometricPressureSensor BarometricPressureSensor { get; set; }

    public IHumiditySensor HumiditySensor { get; set; }

    public IRgbPwmLed RgbPwmLed { get; }

    public void Initialize();
}