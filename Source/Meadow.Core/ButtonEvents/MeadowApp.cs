using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;

namespace ButtonEvents
{
    public class MeadowApp : App<F7FeatherV2>
    {
        IDigitalInterruptPort _input;

        public MeadowApp()
        {
            _input = Device.CreateDigitalInterruptPort(
                Device.Pins.D00,
                InterruptMode.EdgeBoth,
                ResistorMode.InternalPullUp);
            _input.Changed += _input_Changed;

            Console.WriteLine("App initialized.");
        }

        private void _input_Changed(object sender, DigitalPortResult e)
        {
            Console.WriteLine("Changed: " + e.New.State + ", Time: " + e.New.Time.TimeOfDay.ToString());
        }
    }
}