using System;

namespace MeadowAzureServo.Models
{
    public class RotateToEventArgs : EventArgs
    {
        public int AngleInDegrees { get; set; }

        public RotateToEventArgs(int angleInDegrees)
        {
            AngleInDegrees = angleInDegrees;
        }
    }
}