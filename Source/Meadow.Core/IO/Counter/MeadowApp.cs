using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace DigitalInputPort
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private ICounter count10;
        private IDigitalOutputPort source;

        public override Task Initialize()
        {
            Resolver.Log.Info($"NOTE: D10 and D11 must be tied together for this sample.");
            Resolver.Log.Info($"Initializing...");

            count10 = Device.CreateCounter(Device.Pins.D10, InterruptMode.EdgeRising);
            source = Device.CreateDigitalOutputPort(Device.Pins.D11);

            return base.Initialize();
        }

        public override async Task Run()
        {
            var countTo = 100;
            Resolver.Log.Info($"Counting to: {countTo}");

            count10.Enabled = true;

            while (true)
            {
                count10.Reset();

                for (int i = 0; i < countTo; i++)
                {
                    source.State = true;
                    source.State = false;

                    // DEV NOTE: as of b0.6.6.4 this delay is required to allow the processor to process the interrupt
                    // We're working to improve this.
                    await Task.Delay(1);
                }

                Resolver.Log.Info($"Expected: {countTo} Actual: {count10.Count}");

                await Task.Delay(1000);
            }
        }
    }
}