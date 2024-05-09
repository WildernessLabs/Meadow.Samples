using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;

namespace WiFinder.Core;

public class DisplayController
{
    private readonly DisplayScreen screen;
    private readonly INetworkController networkController;
    private NetworkListLayout networkListLayout = default!;
    private SignalHistoryLayout signalHistoryLayout = default!;

    public DisplayController(
        IPixelDisplay display,
        RotationType displayRotation,
        INetworkController networkController)
    {
        this.networkController = networkController;

        var theme = new DisplayTheme
        {
            Font = new Font12x20(),
            BackgroundColor = Color.Black,
            TextColor = Color.White
        };

        screen = new DisplayScreen(
            display,
            rotation: displayRotation,
            theme: theme);

        GenerateLayouts(screen);

        UpdateDisplay();

        networkController.SelectedNetworkChanged += OnSelectedNetworkChanged;
        networkController.NetworkListChanged += OnNetworkListChanged;
        networkController.AntennaChanged += OnAntennaChanged;
    }

    private void OnAntennaChanged(object sender, Meadow.Hardware.AntennaType e)
    {
        networkListLayout.AntennaText = e switch
        {
            AntennaType.External => "EXT",
            _ => "INT"
        };
    }

    private void OnNetworkListChanged(object sender, List<Meadow.Gateway.WiFi.WifiNetwork> e)
    {
        UpdateDisplay();
    }

    private void OnSelectedNetworkChanged(object sender, Meadow.Gateway.WiFi.WifiNetwork? e)
    {
        UpdateDisplay();
    }

    public void ShowAntennaSelection()
    {
        networkListLayout.AntennaSelected = true;
    }

    public void ShowNetworkList()
    {
        networkListLayout.AntennaSelected = false;

        if (!networkListLayout.IsVisible)
        {
            screen.BeginUpdate();
            networkListLayout.IsVisible = true;
            signalHistoryLayout.IsVisible = false;
            screen.EndUpdate();
        }
    }

    public void ShowSelectedNetworkDetails()
    {
        if (!signalHistoryLayout.IsVisible)
        {
            screen.BeginUpdate();
            networkListLayout.IsVisible = false;
            signalHistoryLayout.IsVisible = true;
            screen.EndUpdate();
        }
    }

    private void GenerateLayouts(DisplayScreen screen)
    {
        networkListLayout = new NetworkListLayout(screen);
        signalHistoryLayout = new SignalHistoryLayout(screen)
        {
            IsVisible = false
        };

        screen.Controls.Add(networkListLayout, signalHistoryLayout);
    }

    private void UpdateDisplay()
    {
        screen.BeginUpdate();

        if (networkListLayout.IsVisible)
        {
            networkListLayout.NetworkList.Items.Clear();

            var i = 0;
            var selectedIndex = -1;

            foreach (var network in networkController.Networks)
            {
                networkListLayout.NetworkList.Items.Add(network);
                if (networkController.SelectedNetwork != null && network.Bssid == networkController.SelectedNetwork.Bssid)
                {
                    selectedIndex = i;
                }
                i++;
            }
            if (selectedIndex >= 0)
            {
                networkListLayout.NetworkList.SelectedIndex = selectedIndex;
            }
            networkListLayout.UpdateNetworkCount(networkController.Networks.Count);
        }
        else
        {
            if (networkController.SelectedNetwork != null)
            {
                var current = networkController.Networks.FirstOrDefault(n => n.Bssid.Equals(networkController.SelectedNetwork.Bssid));
                if (current != null)
                {
                    signalHistoryLayout.Update(current);
                }
            }
        }
        screen.EndUpdate();
    }
}
