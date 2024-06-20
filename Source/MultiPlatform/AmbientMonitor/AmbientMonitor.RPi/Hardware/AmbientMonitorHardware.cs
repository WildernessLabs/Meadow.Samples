using AmbientMonitor.Core.Contracts;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;

namespace AmbientMonitor.RPi.Hardware;

public class AmbientMonitorHardware : IAmbientMonitorHardware
{
    private readonly RaspberryPi device;
    private readonly IButton? leftButton;
    private readonly IButton? rightButton;
    private readonly IPixelDisplay? display;
    private readonly INetworkAdapter? networkAdapter;
    private readonly ITemperatureSensor temperatureSimulator;
    private readonly IBarometricPressureSensor barometricPressureSensor;
    private readonly IHumiditySensor humiditySensor;

    public IButton? LeftButton => leftButton;

    public IButton? RightButton => rightButton;

    public IPixelDisplay? Display => display;

    public RotationType DisplayRotation => RotationType._270Degrees;

    public INetworkAdapter? NetworkAdapter => networkAdapter;

    public ITemperatureSensor? TemperatureSensor => temperatureSimulator;

    public IBarometricPressureSensor? BarometricPressureSensor => barometricPressureSensor;

    public IHumiditySensor? HumiditySensor => humiditySensor;

    public AmbientMonitorHardware(RaspberryPi device)
    {
        this.device = device;

        leftButton = new PushButton(device.Pins.GPIO26, ResistorMode.ExternalPullUp);

        rightButton = new PushButton(device.Pins.GPIO19, ResistorMode.ExternalPullUp);

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

        temperatureSimulator = new SimulatedTemperatureSensor(
            new Temperature(20, Temperature.UnitType.Celsius),
            new Temperature(18, Temperature.UnitType.Celsius),
            new Temperature(25, Temperature.UnitType.Celsius));

        barometricPressureSensor = new SimulatedBarometricPressureSensor();

        humiditySensor = new SimulatedHumiditySensor();

        if (MeadowApp.Device.NetworkAdapters.Count > 0)
        {
            networkAdapter = MeadowApp.Device.NetworkAdapters[0];
        }
    }
}