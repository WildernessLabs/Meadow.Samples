using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using Meadow.Logging;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowApp
{
    public class CcmPinTests
    {
        private IMeadowDevice _device;
        private Logger _logger;

        public CcmPinTests(IMeadowDevice device, Logger logger)
        {
            _device = device;
            _logger = logger;
        }

        public static void PulsePwm(IPwmPort pwm, Action? beforeUp = null, Action? beforeDown = null)
        {
            beforeUp?.Invoke();

            for (var i = 0; i < 100; i += 5)
            {
                pwm.DutyCycle = i / 100f;
                Thread.Sleep(100);
            }

            beforeDown?.Invoke();

            for (var i = 100; i > 0; i -= 5)
            {
                pwm.DutyCycle = i / 100f;
                Thread.Sleep(100);
            }
        }

        public async Task TestPulsePWMs(IF7CoreComputeMeadowDevice device, int iterations)
        {
            for (var i = 0; i < iterations; i++)
            {
                foreach (var pin in device.Pins)
                {
                    if (pin.Supports<IPwmChannelInfo>())
                    {
                        using (var pwm = device.CreatePwmPort(pin, new Frequency(500, Frequency.UnitType.Hertz), dutyCycle: 0f))
                        {
                            pwm.Start();

                            // do it twice to give the user the chance to probe
                            for (int c = 0; c < 2; c++)
                            {
                                PulsePwm(
                                    pwm,
                                    () => _logger.Info($"Pin {pin.Name} increasing"),
                                    () => _logger.Info($"Pin {pin.Name} decreasing")
                                    );
                            }
                        }
                    }
                }
            }
        }

        public async Task TestSPI5ControlPins(int iterations)
        {
            // this method is intended to allow testing the display control pins with a scope
            var chipSelect = _device.CreateDigitalOutputPort(_device.GetPin("D17"));
            var dc = _device.CreateDigitalOutputPort(_device.GetPin("D18"));
            var reset = _device.CreateDigitalOutputPort(_device.GetPin("D19"));

            await Task.Run(async () =>
            {
                while (iterations > 0)
                {
                    var state = false;

                    for (int i = 0; i < 20; i++)
                    {
                        _logger.Info($"CS {state}");
                        chipSelect.State = state;
                        await Task.Delay(500);
                    }

                    for (int i = 0; i < 20; i++)
                    {
                        _logger.Info($"DC {state}");
                        chipSelect.State = state;
                        await Task.Delay(500);
                    }

                    for (int i = 0; i < 20; i++)
                    {
                        _logger.Info($"RES {state}");
                        chipSelect.State = state;
                        await Task.Delay(500);
                    }

                    iterations--;
                }
            });
        }
    }

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
}
