using Meadow.Units;

namespace GnssTrackerConnectivity.Models;

public class MotionConditions
{
    public Acceleration3D Acceleration3D { get; set; }

    public AngularVelocity3D AngularVelocity3D { get; set; }
}