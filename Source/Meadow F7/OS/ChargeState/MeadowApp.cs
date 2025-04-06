using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Units;
using System.Threading.Tasks;

namespace ChargeState;

public class MeadowApp : App<F7FeatherV1>
{
    private IObservableAnalogInputPort monitorPort;
    private IDigitalOutputPort greenLed;
    private IDigitalOutputPort redLed;

    public static Voltage PowerThreshold = new Voltage(2.75, Voltage.UnitType.Volts);

    public override Task Initialize()
    {
        Resolver.Log.Info($"Initialize");

        monitorPort = Device.Pins.A00.CreateAnalogInputPort(1);
        monitorPort.StartUpdating();

        greenLed = Device.Pins.OnboardLedGreen.CreateDigitalOutputPort();
        redLed = Device.Pins.OnboardLedRed.CreateDigitalOutputPort();

        return base.Initialize();
    }

    public override async Task Run()
    {
        Resolver.Log.Info($"Run");

        greenLed.State = true;

        while (true)
        {
            // DEV NOTE: This circuit assumes you have a voltage divider from the 5V pin to A0
            //           5V >---[5.6k]----+----[10k]---< GND
            //                            |
            //                            A0

            var currentVoltage = monitorPort.Voltage;

            if (currentVoltage < PowerThreshold)
            {
                redLed.State = true;
            }
            else
            {
                redLed.State = false;
            }

            await Task.Delay(500);
        }
    }
}