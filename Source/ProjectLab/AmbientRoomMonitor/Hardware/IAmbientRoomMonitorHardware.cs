using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Light;

namespace AmbientRoomMonitor.Hardware;

internal interface IAmbientRoomMonitorHardware
{
    IPixelDisplay Display { get; }

    ILightSensor LightSensor { get; }

    ISamplingTemperatureSensor TemperatureSensor { get; }

    IHumiditySensor HumiditySensor { get; }

    IBarometricPressureSensor BarometricPressureSensor { get; }

    IRgbPwmLed RgbPwmLed { get; }

    void Initialize();
}