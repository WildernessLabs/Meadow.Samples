using System;
using Meadow.Units;

namespace RainWaterMonitor
{
    public class StorageContainerConfig
    {
        /// <summary>
        /// Total storage capacity when distance to liquid is zero.
        /// </summary>
        public Volume Capacity { get; set; }
        /// <summary>
        /// Distance to the bottom of the container when empty.
        /// </summary>
        public Length EmptyHeight { get; set; }

        /// <summary>
        /// Volume of liquid per centimeter of height.
        /// </summary>
        public Volume VolumePerCentimeter { get; set; }
    }

    public static class KnownStorageContainerConfigs
    {
        public static StorageContainerConfig Standard55GalDrum {
            get {
                if (standard55GalDrum == null)
                {
                    standard55GalDrum = new StorageContainerConfig
                    {
                        Capacity = new Volume(55, Volume.UnitType.Gallons),
                        EmptyHeight = new Length(85, Length.UnitType.Centimeters),

                        // 55 gal drum 1.72 gal per inch
                        // 3.78541 liters per gallon
                        // 3.78541 * 1.72 = 6.5109052 liters per inch
                        // 6.5109052 / 2.54 = 2.563348503937008 liters per cm
                        VolumePerCentimeter = new Volume(2.563, Volume.UnitType.Liters)
                    };
                }
                return standard55GalDrum;
            }
        } private static StorageContainerConfig standard55GalDrum;

        public static StorageContainerConfig BenchContainer {
            get
            {
                if (benchContainer == null)
                {
                    benchContainer = new StorageContainerConfig
                    {
                        Capacity = new Volume(55, Volume.UnitType.Gallons),
                        EmptyHeight = new Length(20, Length.UnitType.Centimeters),

                        VolumePerCentimeter = new Volume(10.5, Volume.UnitType.Liters)
                    };
                }
                return benchContainer;
            }
        } private static StorageContainerConfig benchContainer;
    }
}