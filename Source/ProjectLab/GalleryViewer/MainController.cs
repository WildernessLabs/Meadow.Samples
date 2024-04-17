using GalleryViewer.Controllers;
using GalleryViewer.Hardware;
using Meadow;
using Meadow.Peripherals.Leds;
using System;
using System.Threading.Tasks;

namespace GalleryViewer;

internal class MainController
{
    IGalleryViewerHardware hardware;
    DisplayController displayController;
    IRgbPwmLed onboardLed;
    int selectedIndex = 0;

    public MainController(IGalleryViewerHardware hardware)
    {
        this.hardware = hardware;
    }

    public void Initialize()
    {
        hardware.Initialize();

        displayController = new DisplayController(hardware.Display);

        hardware.RightButton.Clicked += ButtonRightClicked;
        hardware.LeftButton.Clicked += ButtonLeftClicked;

        onboardLed = hardware.RgbPwmLed;
        onboardLed.SetColor(Color.Green);
    }

    void ButtonLeftClicked(object sender, EventArgs e)
    {
        onboardLed.SetColor(Color.Red);

        if (selectedIndex + 1 > 2)
            selectedIndex = 0;
        else
            selectedIndex++;

        displayController.DrawImage(selectedIndex);

        onboardLed.SetColor(Color.Green);
    }

    void ButtonRightClicked(object sender, EventArgs e)
    {
        onboardLed.SetColor(Color.Red);

        if (selectedIndex - 1 < 0)
            selectedIndex = 2;
        else
            selectedIndex--;

        displayController.DrawImage(selectedIndex);

        onboardLed.SetColor(Color.Green);
    }

    public Task Run()
    {
        displayController.DrawImage(0);

        return Task.CompletedTask;
    }
}