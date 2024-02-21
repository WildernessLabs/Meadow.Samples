using Meadow;
using Meadow.Hardware;

namespace MultiPlatformApp;

public class OutputController
{
    private IDigitalOutputPort outputPort;

    public OutputController(IPin outputPin)
    {
        outputPort = outputPin.CreateDigitalOutputPort();
    }

    public void SetOutputState(bool state)
    {
        outputPort.State = state;
    }
}
