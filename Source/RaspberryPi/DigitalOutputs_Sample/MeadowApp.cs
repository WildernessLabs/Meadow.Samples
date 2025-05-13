using Meadow;
using Meadow.Foundation.ICs.ADC;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using Meadow.Units;

namespace DigitalOutputs_Sample;

public class MeadowApp : App<RaspberryPi>
{
    private List<Led>? leds;

    private Mcp23008 _mcp23008;
    private Mcp3004 _mcp3004;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var names = Device.PlatformOS.GetSerialPortNames();
        Resolver.Log.Info($"names: {names.Length}");

        var pn = Device.PlatformOS.GetSerialPortName("serial0");
        Resolver.Log.Info($"Portname: {pn}");

        var port = pn.CreateSerialPort();
        port.Open();
        var tx = new byte[10];
        var rx = new byte[10];

        while (true)
        {
            Random.Shared.NextBytes(tx);

            port.Write(tx);
            Thread.Sleep(100);
            Resolver.Log.Info($"Available: {port.BytesToRead}");
            var r = port.Read(rx, 0, rx.Length);
            Resolver.Log.Info($"Read: {r}");

            Resolver.Log.Info($"{BitConverter.ToString(tx)} --> {BitConverter.ToString(rx)}");

            Thread.Sleep(2000);
        }






        var _mcpInt = Device.Pins.GPIO18.CreateDigitalInterruptPort(InterruptMode.EdgeRising, ResistorMode.Disabled);

        _mcp23008 = new Mcp23008(
            Device.CreateI2cBus(1),
            address: (byte)Mcp23008.Addresses.Default,
            interruptPort: _mcpInt,
            resetPort: Device.Pins.GPIO17.CreateDigitalOutputPort(false)
            );

        _mcp3004 = new Mcp3004(
            Device.CreateSpiBus(
                Device.Pins.GPIO21,
                Device.Pins.GPIO20,
                Device.Pins.GPIO19,
                new Frequency(2.34, Frequency.UnitType.Megahertz)),
            Device.Pins.Pin24.CreateDigitalOutputPort(true));

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        Resolver.Log.Info("Run...");

        //var gp6 = _mcp23008.Pins.GP6.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
        //var gp5 = _mcp23008.Pins.GP5.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);

        //gp5.Changed += (s, e) => Resolver.Log.Info($"GP5 changed");
        //gp6.Changed += (s, e) => Resolver.Log.Info($"GP6 changed");

        var ch0 = _mcp3004.Pins.CH0.CreateAnalogInputPort();
        var ch1 = _mcp3004.Pins.CH1.CreateAnalogInputPort();
        var ch2 = _mcp3004.Pins.CH2.CreateAnalogInputPort();
        var ch3 = _mcp3004.Pins.CH3.CreateAnalogInputPort();
        ch0.StartUpdating();
        while (true)
        {
            Resolver.Log.Info($"--------");
            Resolver.Log.Info($"Read CH0: {ch0.Voltage:N2} V");
            Resolver.Log.Info($"Read CH1: {ch1.Voltage:N2} V");
            Resolver.Log.Info($"Read CH2: {ch2.Voltage:N2} V");
            Resolver.Log.Info($"Read CH3: {ch3.Voltage:N2} V");

            //Resolver.Log.Info($"GP5: {gp5.State}");
            //Resolver.Log.Info($"GP6: {gp6.State}");

            Thread.Sleep(2000);
        }
    }

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }
}