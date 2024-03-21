using Meadow.Devices;
using Meadow.Hardware;

namespace Threading_Basics
{
    public class FeatherV2OutputService : IOutputService
    {
        public IDigitalOutputPort OutputPort { get; private set; }

        public FeatherV2OutputService(F7FeatherV2 device)
        {
            OutputPort = device.CreateDigitalOutputPort(device.Pins.OnboardLedRed);
        }
    }
}