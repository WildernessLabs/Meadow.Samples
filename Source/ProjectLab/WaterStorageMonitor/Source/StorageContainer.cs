using System;
using Meadow;
using Meadow.Logging;
using Meadow.Units;
using RainWaterMonitor.Hardware;

namespace RainWaterMonitor
{
    public class StorageContainer
    {
        public event EventHandler<IChangeResult<Volume>> VolumeUpdated = delegate { };

        protected Logger? Logger { get; } = Resolver.Log;
        protected IWaterMonitorHardware hardware;

        public StorageContainerConfig ContainerInfo { get; protected set; }
        public Length DistanceToTopOfLiquid { get; protected set; }
        public Volume FillAmount => CalculateFillAmount(DistanceToTopOfLiquid);
        public double FillPercent => (FillAmount.Liters / ContainerInfo.Capacity.Liters) * 100;

        public StorageContainer(
            IWaterMonitorHardware hardware,
            StorageContainerConfig containerInfo)
        {
            Logger?.Info("StorageContainer()");

            this.hardware = hardware;
            this.ContainerInfo = containerInfo;

            hardware.DistanceSensor.DistanceUpdated += DistanceSensor_Updated;

            Logger?.Info("StorageContainer up!");
        }

        private void DistanceSensor_Updated(object sender, IChangeResult<Length> changeResult)
        {
            // save the old conditions
            var oldConditions = this.FillAmount;
            // get the new distance
            this.DistanceToTopOfLiquid = changeResult.New;

            var newConditions = this.FillAmount;

            this.VolumeUpdated(this, new ChangeResult<Volume>(newConditions, oldConditions));
        }

        public void StartUpdating(TimeSpan? updateInterval = null)
        {
            if (updateInterval is null)
            {
                updateInterval = TimeSpan.FromSeconds(5);
            }

            hardware.DistanceSensor.StartUpdating(updateInterval);
        }

        public void StopUpdating()
        {
            hardware.DistanceSensor.StopUpdating();
        }

        protected Volume CalculateFillAmount(Length distanceToTop)
        {
            // this is because the VL430l0x returns negative if it's too far
            if (distanceToTop.Centimeters < 0) { return new Volume(0); }

            // (height - emptyspace) * volumePerCm
            var volumeInLiters = (ContainerInfo.EmptyHeight.Centimeters - distanceToTop.Centimeters) * ContainerInfo.VolumePerCentimeter.Liters;
            if (volumeInLiters < 0) { volumeInLiters = 0; }
            return new Volume(volumeInLiters, Volume.UnitType.Liters);
        }
    }
}