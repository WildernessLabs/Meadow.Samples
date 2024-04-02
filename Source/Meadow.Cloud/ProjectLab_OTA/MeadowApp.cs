using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using ProjectLab_OTA.Controllers;
using ProjectLab_OTA.Hardware;
using System.Threading.Tasks;

namespace ProjectLab_OTA;

public class MeadowApp : App<F7CoreComputeV2>
{
    /*
    
    OTA instructions:

    1. Bump VERSION value below

    2. Open a Terminal (VS2022 - View -> Terminal) and create an mpak file: 

        meadow cloud package create --name <filename>.mpak

    3. Upload the mpak file to Meadow.Cloud:

        meadow cloud package upload bin\Release\netstandard2.1\mpak\<filename>.mpak

    4. Publish the mpak uploaded to roll out an OTA Update:

        meadow cloud package publish <package Id> --collectionId <collection Id>, or

        Go to Meadow.Cloud (https://www.meadowcloud.co/) -> Packages, click Publish on the .mpak uploaded

    5. If/When Meadow device resets, you can still check its console output with the command:

        meadow listen

    */

    public static double VERSION { get; set; } = 1.2;

    MainController mainController;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        var hardware = new MeadowCloudOtaHardware();
        var network = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

        mainController = new MainController(hardware, network);
        mainController.Initialize();

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        mainController.Run();

        return Task.CompletedTask;
    }
}