namespace GnssTrackerConnectivity.Common.Models;

public class MotionReadingsDTO
{
    public string Acceleration3dX { get; set; }
    public string Acceleration3dY { get; set; }
    public string Acceleration3dZ { get; set; }

    public string AngularVelocity3dX { get; set; }
    public string AngularVelocity3dY { get; set; }
    public string AngularVelocity3dZ { get; set; }

    public string Temperature { get; set; }
}