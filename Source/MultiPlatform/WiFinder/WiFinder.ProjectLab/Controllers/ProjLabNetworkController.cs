using Meadow;
using Meadow.Hardware;
using WiFinder.Core;

namespace WiFinder.F7;

public class ProjLabNetworkController : NetworkController
{
    private IWiFiNetworkAdapter wifi;

    public ProjLabNetworkController(IWiFiNetworkAdapter wifi)
        : base(wifi)
    {
        this.wifi = wifi;
    }

    public override void ToggleAntenna()
    {
        Resolver.Log.Info("Toggling antenna...");
        wifi.SetAntenna(wifi.CurrentAntenna switch
        {
            AntennaType.OnBoard => AntennaType.External,
            _ => AntennaType.OnBoard
        });

        base.ToggleAntenna();
    }
}
