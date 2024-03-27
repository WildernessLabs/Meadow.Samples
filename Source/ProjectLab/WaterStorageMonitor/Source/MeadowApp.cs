using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Logging;
using Meadow.Peripherals.Leds;
using Meadow.Units;
using RainWaterMonitor.Hardware;

namespace RainWaterMonitor
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        protected Logger? Logger { get; } = Resolver.Log;
        DisplayController displayController;
        RgbPwmLed onboardLed;
        IWaterMonitorHardware hardware;
        StorageContainer storageContainer;

        HardwareConfigTypes currentHardwareConfig = HardwareConfigTypes.LabProto;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            //==== RGB LED
            Resolver.Log.Info("Initializing onboard RGB LED.");
            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);
            Resolver.Log.Info("RGB LED up.");

            //==== instantiate the project lab hardware
            switch (currentHardwareConfig)
            {
                default:
                case HardwareConfigTypes.BenchProto:
                    hardware = new BenchProtoWaterMonitorHardware();
                    break;
                case HardwareConfigTypes.LabProto:
                    hardware = new LabProtoWaterMonitorHardware();
                    break;
            }

            Resolver.Log.Info($"Running on ProjectLab Hardware {hardware.RevisionString}");

            //---- display controller (handles display updates)
            if (hardware.Display is { } display)
            {
                Resolver.Log.Info("Creating DisplayController.");
                displayController = new DisplayController(display);
                Resolver.Log.Info("DisplayController up.");
            }

            //---- BME688 Atmospheric sensor
            if (hardware.EnvironmentalSensor is { } bme688)
            {
                bme688.Updated += Bme688Updated;
            }

            //---- buttons
            if (hardware.RightButton is { } rightButton) { }
            if (hardware.DownButton is { } downButton) { }
            if (hardware.LeftButton is { } leftButton) { }
            if (hardware.UpButton is { } upButton) { }

            //---- storage container
            try {
                Logger?.Info("Setting storage container config.");
                StorageContainerConfig containerConfig;
                switch (currentHardwareConfig)
                {
                    default:
                    case HardwareConfigTypes.BenchProto:
                        Logger?.Info("Setting bench proto");
                        containerConfig = KnownStorageContainerConfigs.BenchContainer;
                        break;
                    case HardwareConfigTypes.LabProto:
                        Logger?.Info("Setting lab proto");
                        containerConfig = KnownStorageContainerConfigs.Standard55GalDrum;
                        break;
                }
                Logger?.Info("About to instantiate storage container");
                storageContainer = new StorageContainer(hardware, containerConfig);
                Logger?.Info("Storage container up.");
                storageContainer.VolumeUpdated += StorageContainer_VolumeUpdated;
            }
            catch (Exception e) {
                Logger?.Info($"Couldn't instantiate storage container: {e.Message}");
            }

            //---- heartbeat
            onboardLed.StartPulse(WildernessLabsColors.PearGreen);

            Console.WriteLine("Initialization complete");

            return base.Initialize();
        }
        public override Task Run()
        {
            Console.WriteLine("Run...");

            //---- BME688 Atmospheric sensor
            if (hardware.EnvironmentalSensor is { } bme688)
            {
                bme688.StartUpdating(TimeSpan.FromSeconds(5));
            }

            //---- Storage Container
            if (hardware.DistanceSensor is { } distance)
            {
                //distance.StartUpdating(TimeSpan.FromMilliseconds(500));
                storageContainer.StartUpdating(TimeSpan.FromMilliseconds(500));
            }

            if (displayController != null)
            {
                displayController.Update();
            }

            Console.WriteLine("starting blink");
            onboardLed.StartBlink(WildernessLabsColors.PearGreen, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(2000), 0.5f);

            return base.Run();
        }

        private void StorageContainer_VolumeUpdated(object sender, IChangeResult<Volume> e)
        {
            Resolver.Log.Info($"Storage container volume updated: {e.New.Liters:n2}liters");
            if (displayController != null)
            {
                Resolver.Log.Info($"storage container fill percent: {storageContainer.FillPercent}%");
                displayController.ContainerFillPercentage = storageContainer.FillPercent;
            }
        }

        //private void DistanceSensor_DistanceUpdated(object sender, IChangeResult<Length> e)
        //{
        //    Resolver.Log.Info($"Distance sensor updated: {e.New.Centimeters:n2}cm");
        //}


        private void Bme688Updated(object sender, IChangeResult<(Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance)> e)
        {
            Resolver.Log.Info($"BME688: {(int)e.New.Temperature?.Celsius}°C - {(int)e.New.Humidity?.Percent}% - {(int)e.New.Pressure?.Millibar}mbar");
            if (displayController != null)
            {
                displayController.AtmosphericConditions = e.New;
            }
        }

    }
}