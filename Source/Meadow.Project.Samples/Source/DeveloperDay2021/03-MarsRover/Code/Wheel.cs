using Meadow.Hardware;

namespace MarsRover
{
    /// <summary>
    /// Provide the ability to control the direction a motorised wheel spins.
    /// </summary>
    public class Wheel
    {
        /// <summary>
        /// GPIO pin connected to the red wire of a motor.
        /// </summary>
        private readonly IDigitalOutputPort _redWire;

        /// <summary>
        /// GPIO pin connected to the black wire of a motor.
        /// </summary>
        private readonly IDigitalOutputPort _blackWire;

        /// <summary>
        /// Constructor for a wheel object.
        /// </summary>
        /// <param name="redWire">GPIO pin connected to the red wire.</param>
        /// <param name="blackWire">GPIO pin connected to the black wire.</param>
        public Wheel(IDigitalOutputPort redWire, IDigitalOutputPort blackWire)
        {
            _redWire = redWire;
            _blackWire = blackWire;
        }

        /// <summary>
        /// Make the motor rotate in a forward direction.
        /// </summary>
        public void Forward()
        {
            _redWire.State = false;
            _blackWire.State = true;
        }

        /// <summary>
        /// Make the motor spin in the forward direction.
        /// </summary>
        public void Reverse()
        {
            _redWire.State = true;
            _blackWire.State = false;
        }

        /// <summary>
        /// Stop the motor.
        /// </summary>
        public void Stop()
        {
            _redWire.State = false;
            _blackWire.State = false;
        }
    }
}
