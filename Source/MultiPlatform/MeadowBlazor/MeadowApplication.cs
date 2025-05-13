using Meadow;
using Meadow.Foundation.ICs.IOExpanders;

internal class MeadowApplication : App<Meadow.Desktop>
{
    public override Task Initialize()
    {
        FtdiExpanderCollection.Devices.Refresh();
        var ftdi = FtdiExpanderCollection.Devices[0];
        var output = ftdi.Pins.D7.CreateDigitalOutputPort(false);
        Resolver.Services.Add(output);
        return base.Initialize();
    }

    public override Task Run()
    {
        return base.Run();
    }
}
