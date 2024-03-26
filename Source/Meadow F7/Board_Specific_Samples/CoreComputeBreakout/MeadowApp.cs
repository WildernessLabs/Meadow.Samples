using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using Meadow.Logging;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreComputeBreakout;

public class MeadowApp : App<F7CoreComputeV2>
{
    private Logger _logger;
    private SPIDisplay _spiDisplay;
    private I2CDisplay _i2cDisplay;
    private IPwmPort _led;
    private PwmLed _pwm;

    public MeadowApp()
    {
        Initialize();

        var test = new CcmPinTests(Device, _logger);

        _ = Task.Run(() => BlinkyProc());
        _ = Task.Run(() => SpiDisplayProc());
        _ = Task.Run(() => test.TestPulsePWMs(Device, 10));
    }

    private void BlinkyProc()
    {
        _i2cDisplay.ShowText("LED", 0);

        while (true)
        {
            CcmPinTests.PulsePwm(
                _led,
                () => _i2cDisplay.ShowText("Increase", 1),
                () => _i2cDisplay.ShowText("Decrease", 1));
        }
    }

    private void SpiDisplayProc()
    {
        while (true)
        {
            var now = DateTime.Now.ToString("HH:mm:ss"); ;
            _logger.Info($"now: {now}");
            _spiDisplay.ShowText(now, 1);

            Thread.Sleep(1000);
        }
    }


    void Initialize()
    {
        _logger = new Logger(new ConsoleLogProvider());

        _logger.Info("Initialize hardware...");

        var i2cdisplaybus = 3;

        _logger.Info($"Creating I2C display on bus {i2cdisplaybus}...");
        _i2cDisplay = new I2CDisplay(
            Device.CreateI2cBus(i2cdisplaybus),
            _logger);

        var spidisplaybus = 5;

        _logger.Info($"Creating SPI display on bus {spidisplaybus}...");
        var spi = Device.CreateSpiBus();

        _spiDisplay = new SPIDisplay(
            spi,
            Device.Pins.D17, // cs
            Device.Pins.D18, // dc
            Device.Pins.D19, // res
            _logger);

        _led = Device.CreatePwmPort(Device.Pins.D20, new Frequency(500, Frequency.UnitType.Hertz), dutyCycle: 0);
        _led.Start();
    }
}