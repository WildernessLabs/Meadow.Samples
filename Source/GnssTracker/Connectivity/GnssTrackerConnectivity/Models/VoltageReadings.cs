using Meadow.Units;

namespace GnssTrackerConnectivity.Models;

public class VoltageReadings
{
    public Voltage BatteryVoltage { get; set; }

    public Voltage SolarVoltage { get; set; }
}