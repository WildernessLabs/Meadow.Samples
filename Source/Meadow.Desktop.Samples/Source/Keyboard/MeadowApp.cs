using Meadow;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using System.Diagnostics;
using System.Threading.Tasks;

public class MeadowApp : App<Desktop>
{
    private Keyboard _keyBoard;
    private IDigitalInterruptPort _a;

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }

    public override Task Initialize()
    {
        _keyBoard = new Keyboard();
        _a = _keyBoard.CreateDigitalInterruptPort(_keyBoard.Pins.A, InterruptMode.EdgeBoth);
        _a.Changed += OnKeyChanged;
        return Task.CompletedTask;
    }

    private void OnKeyChanged(object? sender, DigitalPortResult e)
    {
        var pin = (sender as Keyboard.KeyboardKey)?.Pin as KeyboardKeyPin;
        Debug.WriteLine($"Key '{pin?.Name}' is {(e.New.State ? "down" : "up")}");
    }
}