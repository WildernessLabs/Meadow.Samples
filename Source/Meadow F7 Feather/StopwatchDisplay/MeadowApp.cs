using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Led;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace StopwatchDisplay
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        bool isRunning;
        Stopwatch stopwatch;
        PushButton reset;
        PushButton startStop;
        FourDigitSevenSegment display;

        public override Task Initialize() 
        {
            var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            stopwatch = new Stopwatch();

            startStop = new PushButton(
                inputPin: Device.Pins.D12,
                resistorMode: Meadow.Hardware.ResistorMode.InternalPullUp);
            startStop.Clicked += StartStopClicked;

            reset = new PushButton(
                inputPin: Device.Pins.D13,
                resistorMode: Meadow.Hardware.ResistorMode.InternalPullUp);
            reset.Clicked += ResetClicked;

            display = new FourDigitSevenSegment
            (
                portDigit1: Device.CreateDigitalOutputPort(Device.Pins.D00),
                portDigit2: Device.CreateDigitalOutputPort(Device.Pins.D03),
                portDigit3: Device.CreateDigitalOutputPort(Device.Pins.D04),
                portDigit4: Device.CreateDigitalOutputPort(Device.Pins.D06),
                portA: Device.CreateDigitalOutputPort(Device.Pins.D01),
                portB: Device.CreateDigitalOutputPort(Device.Pins.D05),
                portC: Device.CreateDigitalOutputPort(Device.Pins.D08),
                portD: Device.CreateDigitalOutputPort(Device.Pins.D10),
                portE: Device.CreateDigitalOutputPort(Device.Pins.D11),
                portF: Device.CreateDigitalOutputPort(Device.Pins.D02),
                portG: Device.CreateDigitalOutputPort(Device.Pins.D07),
                portDecimal: Device.CreateDigitalOutputPort(Device.Pins.D09),
                isCommonCathode: true
            );
            display.SetDisplay("0000".ToCharArray());

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        void StartStopClicked(object sender, EventArgs e)
        {
            if (isRunning)
            {
                stopwatch.Stop();
            }
            else
            {
                stopwatch.Start();
            }
            isRunning = !isRunning;
        }

        void ResetClicked(object sender, EventArgs e)
        {
            display.SetDisplay("0000".ToCharArray());
            stopwatch.Reset();
        }

        public override Task Run() 
        {
            while (true)
            {
                string time = stopwatch.Elapsed.Minutes.ToString("D2") + stopwatch.Elapsed.Seconds.ToString("D2");
                display.SetDisplay(time.ToCharArray());
                Thread.Sleep(1000);
            }
        }
    }
}