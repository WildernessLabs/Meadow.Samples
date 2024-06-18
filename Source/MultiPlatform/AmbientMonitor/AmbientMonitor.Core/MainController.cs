using AmbientMonitor.Core.Contracts;
using Meadow.Units;
using System.Threading.Tasks;

namespace AmbientMonitor.Core;

public class MainController
{
    private IAmbientMonitorHardware hardware;

    private DisplayController displayController;

    private Temperature currentTemperature;

    public MainController()
    {
    }

    public Task Initialize(IAmbientMonitorHardware hardware)
    {
        this.hardware = hardware;

        displayController = new DisplayController(
            this.hardware.Display,
            this.hardware.DisplayRotation);

        return Task.CompletedTask;
    }

    public async Task Run()
    {
        while (true)
        {
            // add any app logic here

            await Task.Delay(5000);
        }
    }
}