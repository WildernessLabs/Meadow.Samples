using Meadow.Gateway.WiFi;
using Meadow.Hardware;

namespace WiFinder.Core;

public interface INetworkController
{
    event EventHandler<WifiNetwork?>? SelectedNetworkChanged;
    event EventHandler<List<WifiNetwork>>? NetworkListChanged;
    event EventHandler<AntennaType>? AntennaChanged;

    List<WifiNetwork> Networks { get; }
    WifiNetwork? SelectedNetwork { get; }

    void SelectPreviousNetwork();
    void SelectNextNetwork();

    void ToggleAntenna();
}
