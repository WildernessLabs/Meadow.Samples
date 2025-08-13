﻿using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;

namespace ProjectLab_AzureIoTHub.Hardware;

internal class MeadowAzureIoTHubHardware : IMeadowAzureIoTHubHardware
{
    protected IProjectLabHardware ProjLab { get; private set; }

    public IPixelDisplay Display { get; set; }

    public ITemperatureSensor TemperatureSensor { get; set; }

    public IBarometricPressureSensor BarometricPressureSensor { get; set; }

    public IHumiditySensor HumiditySensor { get; set; }

    public IRgbPwmLed RgbPwmLed { get; set; }

    public MeadowAzureIoTHubHardware(IProjectLabHardware projLab)
    {
        ProjLab = projLab;
    }

    public void Initialize()
    {
        Display = ProjLab.Display;

        RgbPwmLed = ProjLab.RgbLed;

        TemperatureSensor = ProjLab.TemperatureSensor;
        BarometricPressureSensor = ProjLab.BarometricPressureSensor;
        HumiditySensor = ProjLab.HumiditySensor;
    }
}