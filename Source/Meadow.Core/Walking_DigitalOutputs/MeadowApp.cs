using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Walking_DigitalOutputs
{
    public class MeadowApp : App<F7FeatherV2>
    {
        IList<IDigitalOutputPort> _outs = new List<IDigitalOutputPort>();
        IList<string> _outChans = new List<string>();

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

        void ConfigureOutputs()
        {
            foreach (var pin in Device.Pins.AllPins)
            {
                Resolver.Log.Info("Found pin: " + pin.Name);
                foreach (var channel in pin.SupportedChannels)
                {
                    Resolver.Log.Info("Contains " + channel.Name + "channel.");

                    // if it's a digital channel, create a port.
                    if (channel is IDigitalChannelInfo
                        && !(channel is ICommunicationChannelInfo)
                        && !(channel is IPwmChannelInfo)
                        )
                    {
                        if (!_outChans.Contains(channel.Name))
                        {
                            _outs.Add(Device.CreateDigitalOutputPort(pin));
                        }
                        else
                        {
                            Resolver.Log.Info("Cannot add pin " + pin.Name + ", as the digital channel, " + channel.Name + " exists on another pin");
                        }
                    }
                }
            }
        }

        async Task WalkOutputs()
        {
            // turn each one on for a bit.
            foreach (var port in _outs)
            {
                port.State = true;
                await Task.Delay(250);
                port.State = false;
            }
        }

        void DisposePorts()
        {
            foreach (var port in _outs)
            {
                port.Dispose();
            }
            _outs.Clear();
        }
    }
}