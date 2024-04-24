namespace GnssTrackerConnectivity.Common.Models;

public class AtmosphericReadingsDTO
{
    public string? Temperature { get; set; }

    public string? Pressure { get; set; }

    public string? Humidity { get; set; }

    public string? GasResistance { get; set; }

    public string? Co2Concentration { get; set; }
}