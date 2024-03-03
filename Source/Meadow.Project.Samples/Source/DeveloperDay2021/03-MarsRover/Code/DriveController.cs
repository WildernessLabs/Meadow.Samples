using Meadow.Devices;
using Meadow.Hardware;

namespace MarsRover
{
    /// <summary>
    /// Provide a mechanism for controlling the wheels on an EMONZY four wheel buggy.
    /// </summary>
    public class DriveController
    {
        /// <summary>
        /// Provide a way of recording the current direction of travel.
        /// </summary>
        public enum Directions  { Stopped, Forward, Reverse, TurningLeft, TurningRight, ShufflingLeft, ShufflingRight, RotatingRight, RotatingLeft, Unknown };

        /// <summary>
        /// Record the current direction of travel.
        /// </summary>
        public Directions Direction { get; private set;  }

        /// <summary>
        /// Pin connected to the enable pin of the H-Bridge chips connected to the motors.
        /// </summary>
        private IDigitalOutputPort _enableMotors;

        /// <summary>
        /// Front left wheel controller.
        /// </summary>
        private Wheel FrontLeftWheel { get; set; }

        /// <summary>
        /// Front right wheel contrroller.
        /// </summary>
        private Wheel FrontRightWheel { get; set; }

        /// <summary>
        /// Rear left wheel controller.
        /// </summary>
        private Wheel RearLeftWheel { get; set; }

        /// <summary>
        /// Rear right wheel controller.
        /// </summary>
        private Wheel RearRightWheel { get; set; }

        /// <summary>
        /// Constructor for the DriverController object.
        /// </summary>
        /// <param name="device">Meadow device object (required to access the GPIO pins).</param>
        public DriveController(F7Micro device)
        {
            _enableMotors = device.CreateDigitalOutputPort(device.Pins.D04);
            Stop();

            FrontLeftWheel = new Wheel(device.CreateDigitalOutputPort(device.Pins.D02), device.CreateDigitalOutputPort(device.Pins.D03));
            FrontRightWheel = new Wheel(device.CreateDigitalOutputPort(device.Pins.D11), device.CreateDigitalOutputPort(device.Pins.D10));
            RearLeftWheel = new Wheel(device.CreateDigitalOutputPort(device.Pins.A00), device.CreateDigitalOutputPort(device.Pins.A01));
            RearRightWheel = new Wheel(device.CreateDigitalOutputPort(device.Pins.D13), device.CreateDigitalOutputPort(device.Pins.D14));
        }

        /// <summary>
        /// Stop all of the motors by dropping the enable pin on the H-Bridge chips.
        /// </summary>
        public void Stop()
        {
            _enableMotors.State = false;
            Direction = Directions.Stopped;
        }

        /// <summary>
        /// Start the motors by enabling the H-Bridge chips.
        /// </summary>
        private void Start()
        {
            _enableMotors.State = true;
        }

        /// <summary>
        /// Move the buggy forward.
        /// </summary>
        public void Forward()
        {
            Stop();

            FrontLeftWheel.Forward();
            FrontRightWheel.Forward();
            RearLeftWheel.Forward();
            RearRightWheel.Forward();

            Direction = Directions.Forward;
            Start();
        }

        /// <summary>
        /// Put the buggy in reverse.
        /// </summary>
        public void Reverse()
        {
            Stop();

            FrontLeftWheel.Reverse();
            FrontRightWheel.Reverse();
            RearLeftWheel.Reverse();
            RearRightWheel.Reverse();
            Direction = Directions.Reverse;

            Start();
        }

        /// <summary>
        /// Move the buggy forward and turn to the left.
        /// </summary>
        public void TurnLeft()
        {
            Stop();

            FrontLeftWheel.Stop();
            FrontRightWheel.Forward();
            RearLeftWheel.Stop();
            RearRightWheel.Forward();
            Direction = Directions.TurningLeft;

            Start();
        }

        /// <summary>
        /// Move the buggy forward and turn to the right.
        /// </summary>
        public void TurnRight()
        {
            Stop();

            FrontLeftWheel.Forward();
            FrontRightWheel.Stop();
            RearLeftWheel.Forward();
            RearRightWheel.Stop();
            Direction = Directions.TurningRight;

            Start();
        }

        /// <summary>
        /// Move the buggy left.
        /// </summary>
        public void ShuffleLeft()
        {
            Stop();

            FrontLeftWheel.Reverse();
            RearLeftWheel.Forward();

            FrontRightWheel.Forward();
            RearRightWheel.Reverse();
            Direction = Directions.ShufflingLeft;

            Start();
        }

        /// <summary>
        /// Move the buggy right.
        /// </summary>
        public void ShuffleRight()
        {
            Stop();

            FrontLeftWheel.Forward();
            RearLeftWheel.Reverse();

            FrontRightWheel.Reverse();
            RearRightWheel.Forward();
            Direction = Directions.ShufflingRight;

            Start();
        }

        /// <summary>
        /// Rotate the buggy right (on the spot).
        /// </summary>
        public void RotateRight()
        {
            Stop();

            FrontLeftWheel.Forward();
            RearLeftWheel.Forward();

            FrontRightWheel.Reverse();
            RearRightWheel.Reverse();
            Direction = Directions.RotatingRight;

            Start();
        }

        /// <summary>
        /// Rotate the buggy left (on the spot).
        /// </summary>
        public void RotateLeft()
        {
            Stop();

            FrontLeftWheel.Reverse();
            RearLeftWheel.Reverse();

            FrontRightWheel.Forward();
            RearRightWheel.Forward();
            Direction = Directions.RotatingLeft;

            Start();
        }
    }
}
