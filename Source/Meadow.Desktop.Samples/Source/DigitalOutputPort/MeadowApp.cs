using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;
using System.Threading;
using System.Threading.Tasks;

public class MeadowApp : App<Desktop>
{
    private FtdiExpander _expander = FtdiExpanderCollection.Devices[0];
    private IDigitalOutputPort _c0;

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }

    public override Task Initialize()
    {
        Resolver.Log.Info("Creating Outputs");

        _c0 = _expander.CreateDigitalOutputPort(_expander.Pins.C0);

        while (true)
        {
            _c0.State = !_c0.State;
            Thread.Sleep(1);
        }

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
    }
}