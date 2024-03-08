using AmbientRoomMonitor.Hardware;
using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace AmbientRoomMonitor
{
    // Change F7CoreComputeV2 to F7FeatherV2 for ProjectLab v2
    public class MeadowApp : App<F7CoreComputeV2>
    {
        MainController mainController;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            var hardware = new AmbientRoomMonitorHardware();

            mainController = new MainController(hardware);
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
}