using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace BiDirectonalPort
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private IBiDirectionalInterruptPort _d04 = null;
        private IBiDirectionalInterruptPort _d05 = null;
        private IBiDirectionalInterruptPort _d06 = null;

        public override Task Initialize()
        {
            Console.Write("Creating ports...");
            // _d04 = Device.CreateBiDirectionalPort(Device.Pins.D04);
            // _d05 = Device.CreateBiDirectionalPort(Device.Pins.D05);
            // _d06 = Device.CreateBiDirectionalPort(
            //     Device.Pins.D06,

            _d04 = Device.CreateBiDirectionalInterruptPort(Device.Pins.D08);
            _d05 = Device.CreateBiDirectionalInterruptPort(Device.Pins.D09);

            _d06 = Device.CreateBiDirectionalInterruptPort(
                Device.Pins.D10,
                resistorMode: ResistorMode.Disabled,
                initialDirection: PortDirectionType.Input,
                interruptMode: InterruptMode.EdgeFalling,
                glitchDuration: TimeSpan.FromMilliseconds(20),
                debounceDuration: TimeSpan.FromMilliseconds(20),
                initialState: false,
                outputType: OutputType.OpenDrain
                );

            _d06.Changed += OnD06Changed;

            Resolver.Log.Info("ok");

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            var state = true;
            var count = 0;

            while (true)
            {
                // _d04 starts as input
                Resolver.Log.Info($"---- Start ----");
                Resolver.Log.Info($"D04 --> D05 reads {(state ? "high" : "low")}");
                // set output
                _d04.State = state;     // D04 to output and set true
                // read input
                var check = _d05.State; // Read D05 remains input
                Resolver.Log.Info($"  D05 is {(check ? "high" : "low")} should match previous");

                state = !state;

                Resolver.Log.Info($"---- Reverse ----");
                // now reverse
                Resolver.Log.Info($"D04 <-- D05 writes {(state ? "high" : "low")}");
                // set output
                _d05.State = state;   // D05 to output set false
                // read input
                check = _d04.State;   // Read D04 changes to input
                Resolver.Log.Info($"  D04 is {(check ? "high" : "low")} should match previous");

                state = !state;

                if (++count % 10 == 0)
                {
                    // verifies Dispose is working
                    TeardownIO();
                }

                await Task.Delay(2000);
            }
        }

        private async void OnD06Changed(object sender, DigitalPortResult args)
        {
            // The circuit had an led tied to Vcc an resister from the led to D06
            // and a push button from ground to D06. If the led has a low forward
            // drop pressing the button will cause the LED to blink.
            Resolver.Log.Info("D06 Interrupt");
            Resolver.Log.Info("D06 -> false");
            _d06.State = false;      // Becomes output & sets high
            await Task.Delay(2000);

            Resolver.Log.Info("D06 -> true");
            _d06.State = true;     // Still output & sets low
            await Task.Delay(2000);

            Resolver.Log.Info("D06 -> false");
            _d06.State = false;      // Still output & sets high
            await Task.Delay(2000);

            _d06.State = true;     // Still output & sets low
            Resolver.Log.Info("D06 -> input");
            _d06.Direction = PortDirectionType.Input;   // Return to input
        }

        private void TeardownIO()
        {
            Console.Write("Disposing ports...");
            _d04.Dispose();
            _d04 = null;
            _d05.Dispose();
            _d05 = null;
            _d06.Dispose();
            _d06 = null;
            Resolver.Log.Info("ok");
        }
    }
}