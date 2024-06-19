using GalleryViewer.Core.Contracts;
using GalleryViewer.Core.Controllers;
using System.Threading.Tasks;

namespace GalleryViewer.Core;

public class MainController
{
    private IGalleryViewerHardware hardware;

    private DisplayController displayController;
    private InputController inputController;

    private int selectedIndex = 0;

    public MainController() { }

    public Task Initialize(IGalleryViewerHardware hardware)
    {
        this.hardware = hardware;

        inputController = new InputController(hardware);
        inputController.leftButtonPressed += LeftButtonPressed;
        inputController.rightButtonPressed += RightButtonPressed;

        displayController = new DisplayController(
            this.hardware.Display,
            this.hardware.DisplayRotation);

        return Task.CompletedTask;
    }

    private void LeftButtonPressed(object sender, System.EventArgs e)
    {
        if (selectedIndex + 1 > 2)
            selectedIndex = 0;
        else
            selectedIndex++;

        displayController.UpdateDisplay(selectedIndex);
    }

    private void RightButtonPressed(object sender, System.EventArgs e)
    {
        if (selectedIndex - 1 < 0)
            selectedIndex = 2;
        else
            selectedIndex--;

        displayController.UpdateDisplay(selectedIndex);
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