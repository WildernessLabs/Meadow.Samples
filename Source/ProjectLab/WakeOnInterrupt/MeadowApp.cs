using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using System;
using System.Threading.Tasks;

namespace WakeOnInterrupt;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private DisplayScreen _screen;
    private Label _message;
    private Box _box;
    private IDigitalInterruptPort _powerPort;
    private IDigitalOutputPort _backlightPort;
    private int _wakeCount;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        Resolver.Log.Info($"Running on ProjectLab Hardware {Hardware.RevisionString}");

        _powerPort = Hardware.IOTerminal.Pins.A1.CreateDigitalInterruptPort(Meadow.Hardware.InterruptMode.EdgeFalling, Meadow.Hardware.ResistorMode.Disabled);
        _powerPort.Changed += OnPowerPortChanged;

        _backlightPort = Hardware.MikroBus1.Pins.INT.CreateDigitalOutputPort(true);

        Hardware.ComputeModule.PlatformOS.AfterWake += AfterWake;
        CreateLayout();

        PowerOnPeripherals(false);

        return base.Initialize();
    }

    private void AfterWake(object sender, WakeSource e)
    {
        if (e == WakeSource.Interrupt)
        {
            Resolver.Log.Info("Wake on interrupt");
            _wakeCount++;
            PowerOnPeripherals(true);
        }
        else
        {
            PowerOffPeripherals();
        }
    }

    private void PowerOffPeripherals()
    {
        Resolver.Log.Info("Powering off");
        _backlightPort.State = false;
        Hardware.ComputeModule.PlatformOS.Sleep(Hardware.IOTerminal.Pins.A1, InterruptMode.EdgeRising);
    }

    private void PowerOnPeripherals(bool fromWake)
    {
        Resolver.Log.Info("Powering on");


        if (!fromWake)
        {
            DisplayMessage("Hello from boot!");
        }
        else
        {
            DisplayMessage($"Hello from wake! ({_wakeCount})");
        }

        _backlightPort.State = true;
    }

    private void OnPowerPortChanged(object sender, DigitalPortResult e)
    {
        var powerState = _powerPort.State;

        Resolver.Log.Info($"Power port state changed to {powerState}");

        if (!powerState)
        {
            PowerOffPeripherals();
        }
    }

    private void DisplayMessage(string message)
    {
        _message.Text = message;
    }

    private void CreateLayout()
    {
        _screen = new DisplayScreen(Hardware.Display, RotationType._270Degrees);
        _message = new Label(0, 0, _screen.Width, 30)
        {
            Font = new Font12x20()
        };

        _box = new Box(0, 60, 30, 30)
        {
            ForegroundColor = Color.Green
        };

        _screen.Controls.Add(_message, _box);
    }

    public override async Task Run()
    {
        Console.WriteLine("Run...");

        var x = 0;
        var direction = 1;
        var moveAmount = 3;
        var i = 0;

        while (true)
        {
            if (x >= (_screen.Width - _box.Width - moveAmount))
            {
                direction = -1;
            }
            else if (x <= 0)
            {
                direction = 1;
            }

            x += (moveAmount * direction);

            _box.Left = x;

            if (i++ % 10 == 0) Console.WriteLine($"x={x} direction={direction}");

            await Task.Delay(100);
        }
    }

}
