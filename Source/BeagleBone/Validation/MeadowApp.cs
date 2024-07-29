using Meadow;
using Meadow.Foundation.ICs.CAN;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Hardware;
using Meadow.Units;
using System.Threading.Tasks;

namespace Validation;

public class MeadowApp : App<BeagleBoneBlack>
{
    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        await ValidateSPI();
    }

    public async Task ValidateSPI()
    {
        Resolver.Log.Info("BeagleBone SPI stuff...");

        var transceiver = new Mcp2515(
            Device.CreateSpiBus(0, 1_000_000.Hertz()),
            Device.Pins.GPIO_49.CreateDigitalOutputPort(),
             Mcp2515.CanOscillator.Osc_8MHz);

        var bus = transceiver.CreateCanBus(CanBitrate.Can_250kbps);

        while (true)
        {
            Resolver.Log.Info("Checking bus...");

            if (bus.IsFrameAvailable())
            {
                var frame = bus.ReadFrame();
                if (frame != null)
                {
                    Resolver.Log.Info($"Received a {frame!.GetType().Name}");
                }
                else
                {
                    Resolver.Log.Info("Frame available but not readable?");
                }
            }
            else
            {
                Resolver.Log.Info("No data");
            }

            await Task.Delay(1000);
        }

    }

    public async Task ValidateI2C()
    {
        Resolver.Log.Info("BeagleBone reading BMP280 over I2C...");

        var sensor = new Bmp280(Device.CreateI2cBus(Device.Pins.I2C2_SCL, Device.Pins.I2C2_SDA, Meadow.Hardware.I2cBusSpeed.Standard));
        //var sensor = new Bmp280(Device.CreateI2cBus());

        while (true)
        {
            var reading = await sensor.Read();
            Resolver.Log.Info($"Temp: {reading.Temperature?.Fahrenheit:N1} F");
            await Task.Delay(1000);
        }

    }

    public async Task ValidatePWMs()
    {
        var ports = new[]
        {
            // sudo config-pin P9.22 pwm
            Device.Pins.ECAPPWM0.CreatePwmPort(50.Hertz(), 0.5f),
//            Device.Pins.ECAPPWM2.CreatePwmPort(50.Hertz(), 0.5f),
//            Device.Pins.EHRPWM1A.CreatePwmPort(50.Hertz(), 0.5f),
//            Device.Pins.EHRPWM1B.CreatePwmPort(50.Hertz(), 0.5f),
//            Device.Pins.EHRPWM2A.CreatePwmPort(50.Hertz(), 0.5f),
//            Device.Pins.EHRPWM2B.CreatePwmPort(50.Hertz(), 0.5f),
        };

        while (true)
        {
            foreach (var port in ports)
            {
                Resolver.Log.Info($"Starting {port.Pin.Name}");
                port.Start();

                Resolver.Log.Info($" {port.Pin.Key} {port.State} {port.Frequency.Hertz}Hz {port.DutyCycle:N1}");
            }

            await Task.Delay(1000);

            foreach (var port in ports)
            {
                Resolver.Log.Info($"Stopping {port.Pin.Name}");
                port.Stop();

                Resolver.Log.Info($" {port.Pin.Key} {port.State} {port.Frequency.Hertz}Hz {port.DutyCycle:N1}");
            }

            await Task.Delay(1000);
        }
    }

    public async Task ValidateAnalogInputs()
    {
        var pins = new[]
        {
            Device.Pins.AIN0.CreateAnalogInputPort(1),
            Device.Pins.AIN1.CreateAnalogInputPort(1),
            Device.Pins.AIN2.CreateAnalogInputPort(1),
            Device.Pins.AIN3.CreateAnalogInputPort(1),
            Device.Pins.AIN4.CreateAnalogInputPort(1),
            Device.Pins.AIN5.CreateAnalogInputPort(1),
            Device.Pins.AIN6.CreateAnalogInputPort(1),
        };

        while (true)
        {
            foreach (var pin in pins)
            {
                var voltage = await pin.Read();

                Resolver.Log.Info($"{pin.Pin.Name} = {voltage.Volts:N2} V");
            }

            await Task.Delay(1000);
        }
    }

    public async Task ValidateDigitalOutputs()
    {
        var state = false;

        var pins = new[]
        {
            Device.Pins.GPIO_48.CreateDigitalOutputPort(state),
            Device.Pins.GPIO_60.CreateDigitalOutputPort(state),
            Device.Pins.GPIO_66.CreateDigitalOutputPort(state),
            Device.Pins.GPIO_67.CreateDigitalOutputPort(state),
        };

        while (true)
        {
            state = !state;

            foreach (var pin in pins)
            {
                Resolver.Log.Info($"{pin.Pin.Name} Set State = {state}");
                pin.State = state;
                Resolver.Log.Info($"{pin.Pin.Name} Read State = {pin.State}");
            }

            await Task.Delay(1000);
        }
    }
}