using Meadow.Units;

namespace GnssTrackerConnectivity.Models;

public class AtmosphericConditions
{
    public Temperature Temperature { get; set; }

    public Pressure Pressure { get; set; }

    public RelativeHumidity Humidity { get; set; }

    public Resistance GasResistance { get; set; }

    public Concentration Co2Concentration { get; set; }
}