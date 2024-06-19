using AmbientMonitor.Core.Contracts;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Sensors;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;

namespace AmbientMonitor.RPi.Hardware;

internal class AmbientMonitorHardware : IAmbientMonitorHardware
{
    private readonly RaspberryPi device;
    private readonly IPixelDisplay? display = null;
    private readonly ITemperatureSensor temperatureSimulator;
    private readonly IBarometricPressureSensor barometricPressureSensor;
    private readonly IHumiditySensor humiditySensor;

    public IButton? LeftButton { get; }

    public IButton? RightButton { get; }

    public IPixelDisplay? Display => display;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public INetworkAdapter? NetworkAdapter { get; }

    public ITemperatureSensor? TemperatureSensor { get; }

    public IBarometricPressureSensor? BarometricPressureSensor { get; }

    public IHumiditySensor? HumiditySensor { get; }

    public AmbientMonitorHardware(RaspberryPi device)
    {
        this.device = device;

        //LeftButton = new PushButton(keyboard.Pins.Left.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));

        //RightButton = new PushButton(keyboard.Pins.Right.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));

        var spiBus = device.CreateSpiBus(
           device.Pins.SPI0_SCLK,
           device.Pins.SPI0_MOSI,
           device.Pins.SPI0_MISO,
           new Frequency(48, Frequency.UnitType.Megahertz));

        display = new Ili9341
        (
            spiBus,
            chipSelectPin: device.Pins.GPIO16,
            dcPin: device.Pins.GPIO21,
            resetPin: device.Pins.GPIO20,
            width: 240, height: 320
        );

        TemperatureSensor = new SimulatedTemperatureSensor(
            new Temperature(20, Temperature.UnitType.Celsius),
            new Temperature(18, Temperature.UnitType.Celsius),
            new Temperature(25, Temperature.UnitType.Celsius));

        BarometricPressureSensor = new SimulatedBarometricPressureSensor();

        HumiditySensor = new SimulatedHumiditySensor();

        if (MeadowApp.Device.NetworkAdapters.Count > 0)
        {
            NetworkAdapter = MeadowApp.Device.NetworkAdapters[0];
        }
    }
}