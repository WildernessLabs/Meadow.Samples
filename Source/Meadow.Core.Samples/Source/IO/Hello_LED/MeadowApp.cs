using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace Hello_LED
{
    public class MeadowApp : App<F7FeatherV2>
    {
        IDigitalOutputPort _redLED;
        IDigitalOutputPort _blueLED;
        IDigitalOutputPort _greenLED;

        public override Task Initialize()
        {
            Resolver.Log.Info("Creating Outputs");
            _redLED = Device.Pins.OnboardLedRed.CreateDigitalOutputPort();
            _blueLED = Device.Pins.OnboardLedBlue.CreateDigitalOutputPort();
            _greenLED = Device.Pins.OnboardLedGreen.CreateDigitalOutputPort();

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            var state = false;
            var stateCount = 0;

            while (true)
            {
                state = !state;

                Resolver.Log.Info($" Count: {++stateCount}, State: {state}");

                _redLED.State = state;
                await Task.Delay(200);
                _greenLED.State = state;
                await Task.Delay(200);
                _blueLED.State = state;
                await Task.Delay(200);
            }
        }
    }
}