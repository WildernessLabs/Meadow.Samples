using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Walking_DigitalOutputs
{
    public class MeadowApp : App<F7FeatherV2>
    {
        IList<IDigitalOutputPort> _outs = new List<IDigitalOutputPort>();

        public override async Task Run()
        {
            while (true)
            {
                // create all our digital output ports
                ConfigureOutputs();

                // turn them on/off
                await WalkOutputs();

                // tear down
                DisposePorts();
            }
        }

        protected void ConfigureOutputs()
        {
            Console.Write("Creating ports...");

            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.OnboardLedRed));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.OnboardLedGreen));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.OnboardLedBlue));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.A00));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.A01));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.A02));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.A03));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.A04));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.A05));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.SCK));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.MOSI));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.MISO));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D00));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D01));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D02));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D03));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D04));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D15));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D14));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D13));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D12));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D11));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D10));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D09));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D08));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D07));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D06));
            _outs.Add(Device.CreateDigitalOutputPort(Device.Pins.D05));

            Resolver.Log.Info("ok.");
        }

        async Task WalkOutputs()
        {
            // turn each one on for a bit.
            foreach (var port in _outs)
            {
                Console.Write($"{port.Pin.Name} ");
                port.State = true;
                await Task.Delay(250);
                port.State = false;
            }
        }

        void DisposePorts()
        {
            Console.Write("Disposing ports...");

            foreach (var port in _outs)
            {
                port.Dispose();
            }
            _outs.Clear();

            Resolver.Log.Info("ok.");
        }
    }
}