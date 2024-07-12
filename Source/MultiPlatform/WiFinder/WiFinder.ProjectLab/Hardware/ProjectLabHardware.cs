using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using WiFinder.Core;
using WiFinder.Core.Contracts;

namespace WiFinder.F7;

internal class ProjectLabHardware : IWiFinderHardware
{
    private IProjectLabHardware projLab;

    public ProjectLabHardware(IProjectLabHardware projLab)
    {
        this.projLab = projLab;

        var wifi = projLab.ComputeModule.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        NetworkController = new ProjLabNetworkController(wifi!);
    }

    public IButton? LeftButton => projLab.LeftButton;
    public IButton? RightButton => projLab.RightButton;
    public IButton? UpButton => projLab.UpButton;
    public IButton? DownButton => projLab.DownButton;
    public IPixelDisplay Display => projLab.Display ?? throw new NotSupportedException("Requires a ProjLab with Display");
    public RotationType DisplayRotation => RotationType._270Degrees;

    public INetworkController NetworkController { get; }
}
