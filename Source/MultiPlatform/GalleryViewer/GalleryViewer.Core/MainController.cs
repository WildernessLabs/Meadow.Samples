using GalleryViewer.Core.Contracts;
using System.Threading.Tasks;

namespace GalleryViewer.Core;

public class MainController
{
    private IGalleryViewerHardware hardware;

    private DisplayController displayController;
    private InputController inputController;

    public MainController() { }

    public Task Initialize(IGalleryViewerHardware hardware)
    {
        this.hardware = hardware;

        //inputController = new InputController(hardware);
        //inputController.UnitDownRequested += OnUnitDownRequested;
        //inputController.UnitUpRequested += OnUnitUpRequested;

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