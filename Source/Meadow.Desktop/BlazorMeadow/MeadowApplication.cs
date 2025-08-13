using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Leds;

internal class MeadowApplication : App<Meadow.Desktop>
{
    public override Task Initialize()
    {
        FtdiExpanderCollection.Devices.Refresh();
        var ftdi = FtdiExpanderCollection.Devices[0];
        var output = ftdi.Pins.D7.CreateDigitalOutputPort(false);
        Resolver.Services.Add(output);

        var bme680 = new Bme680(ftdi.CreateSpiBus(), ftdi.Pins.C7);
        Resolver.Services.Add(bme680);

        var led = new Led(ftdi.Pins.C0);
        Resolver.Services.Add<ILed>(led);

        return base.Initialize();
    }

    public override Task Run()
    {
        return base.Run();
    }
}
