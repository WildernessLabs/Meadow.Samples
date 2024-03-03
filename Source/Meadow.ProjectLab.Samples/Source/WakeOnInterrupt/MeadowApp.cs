using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using System;
using System.Threading.Tasks;

namespace WakeOnInterrupt;

public class MeadowApp : App<F7CoreComputeV2>
{
    private IProjectLabHardware _projectLab;
    private DisplayScreen _screen;
    private Label _message;
    private Box _box;
    private IDigitalInterruptPort _powerPort;
    private IDigitalOutputPort _backlighPort;
    private int _wakeCount;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        _projectLab = ProjectLab.Create();
        Resolver.Log.Info($"Running on ProjectLab Hardware {_projectLab.RevisionString}");

        _powerPort = _projectLab.IOTerminal.Pins.A1.CreateDigitalInterruptPort(Meadow.Hardware.InterruptMode.EdgeFalling, Meadow.Hardware.ResistorMode.Disabled);
        _powerPort.Changed += OnPowerPortChanged;

        _backlighPort = _projectLab.MikroBus1.Pins.INT.CreateDigitalOutputPort(true);

        Device.PlatformOS.AfterWake += AfterWake;
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
        _backlighPort.State = false;
        Device.PlatformOS.Sleep(_projectLab.IOTerminal.Pins.A1, InterruptMode.EdgeRising);
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

        _backlighPort.State = true;
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
        _screen = new DisplayScreen(_projectLab.Display, RotationType._270Degrees);
        _message = new Label(0, 0, _screen.Width, 30)
        {
            Font = new Font12x20()
        };

        _box = new Box(0, 60, 30, 30)
        {
            ForeColor = Color.Green
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