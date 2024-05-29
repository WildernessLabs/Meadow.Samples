using WiFinder.Core.Contracts;

namespace WiFinder.Core;

public class MainController
{
    private IWiFinderHardware hardware;

    private DisplayController displayController;
    private InputController inputController;

    private INetworkController NetworkController => hardware.NetworkController;

    private AppActivity CurrentActivity { get; set; }

    public MainController()
    {
    }

    public Task Initialize(IWiFinderHardware hardware)
    {
        this.hardware = hardware;

        // create generic services
        inputController = new InputController(hardware);

        displayController = new DisplayController(
            this.hardware.Display,
            this.hardware.DisplayRotation,
            this.hardware.NetworkController);

        // connect events
        inputController.DownButtonPressed += OnDownButtonPressed;
        inputController.UpButtonPressed += OnUpButtonPressed;
        inputController.SelectButtonPressed += OnSelectButtonPressed;
        inputController.BackButtonPressed += OnBackButtonPressed;

        CurrentActivity = AppActivity.NetworkSelect;

        return Task.CompletedTask;
    }

    private enum AppActivity
    {
        AntennaSelect,
        NetworkSelect,
        NetworkDetail
    }

    private void OnBackButtonPressed(object sender, EventArgs e)
    {
        // if we're on the network list, select antenna swap
        switch (CurrentActivity)
        {
            case AppActivity.NetworkSelect:
                displayController.ShowAntennaSelection();
                CurrentActivity = AppActivity.AntennaSelect;
                break;
            case AppActivity.NetworkDetail:
                displayController.ShowNetworkList();
                CurrentActivity = AppActivity.NetworkSelect;
                break;
        }
    }

    private void OnSelectButtonPressed(object sender, EventArgs e)
    {
        // if we're on antenna select, just de-select back to the network list
        switch (CurrentActivity)
        {
            case AppActivity.AntennaSelect:
                displayController.ShowNetworkList();
                CurrentActivity = AppActivity.NetworkSelect;
                break;
            case AppActivity.NetworkSelect:
                displayController.ShowSelectedNetworkDetails();
                CurrentActivity = AppActivity.NetworkDetail;
                break;
        }
    }

    private void OnUpButtonPressed(object sender, EventArgs e)
    {
        switch (CurrentActivity)
        {
            case AppActivity.AntennaSelect:
                NetworkController.ToggleAntenna();
                break;
            default:
                NetworkController.SelectPreviousNetwork();
                break;
        }
    }

    private void OnDownButtonPressed(object sender, EventArgs e)
    {
        switch (CurrentActivity)
        {
            case AppActivity.AntennaSelect:
                NetworkController.ToggleAntenna();
                break;
            default:
                NetworkController.SelectNextNetwork();
                break;
        }
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