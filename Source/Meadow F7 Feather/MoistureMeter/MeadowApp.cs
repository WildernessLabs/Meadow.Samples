using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;
using VU = Meadow.Units.Voltage.UnitType;

namespace MoistureMeter
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        readonly Voltage MINIMUM_VOLTAGE_CALIBRATION = new Voltage(2.84, VU.Volts);
        readonly Voltage MAXIMUM_VOLTAGE_CALIBRATION = new Voltage(1.37, VU.Volts);

        Capacitive capacitive;
        LedBarGraph ledBarGraph;

        public override Task Initialize()
        {
            var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            IDigitalOutputPort[] ports =
            {
                Device.CreateDigitalOutputPort(Device.Pins.D05),
                Device.CreateDigitalOutputPort(Device.Pins.D06),
                Device.CreateDigitalOutputPort(Device.Pins.D07),
                Device.CreateDigitalOutputPort(Device.Pins.D08),
                Device.CreateDigitalOutputPort(Device.Pins.D09),
                Device.CreateDigitalOutputPort(Device.Pins.D10),
                Device.CreateDigitalOutputPort(Device.Pins.D11),
                Device.CreateDigitalOutputPort(Device.Pins.D12),
                Device.CreateDigitalOutputPort(Device.Pins.D13),
                Device.CreateDigitalOutputPort(Device.Pins.D14)
            };

            ledBarGraph = new LedBarGraph(ports);
            capacitive = new Capacitive
            (
                Device.CreateAnalogInputPort(Device.Pins.A00),
                MINIMUM_VOLTAGE_CALIBRATION,
                MAXIMUM_VOLTAGE_CALIBRATION
            );

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        public override async Task Run()
        {
            while (true)
            {
                var reading = await capacitive.Read();
                double moisture = reading;

                if (moisture > 1)
                    moisture = 1f;
                else
                if (moisture < 0)
                    moisture = 0f;

                await ledBarGraph.SetPercentage((float)moisture);
                Console.WriteLine($"Moisture {moisture * 100}%");
                Thread.Sleep(1000);
            }
        }
    }
}