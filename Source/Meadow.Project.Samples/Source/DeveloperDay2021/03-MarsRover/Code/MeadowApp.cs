using System;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Distance;
using Meadow.Hardware;
using Meadow.Units;
using LU = Meadow.Units.Length.UnitType;

namespace MarsRover
{
    public class Rover : App<F7Micro, Rover>
    {
        /// <summary>
        /// LED used to indicate that there is a problem.
        /// </summary>
        private IDigitalOutputPort _redLed;

        /// <summary>
        /// LED used to indictae all is well with the world.
        /// </summary>
        private IDigitalOutputPort _greenLed;

        /// <summary>
        /// Private member variable for the drive controller.
        /// </summary>
        private DriveController _driveController;

        /// <summary>
        /// Private member variable for the bluetooth controller (i.e. the remote control).
        /// </summary>
        BluetoothController _bluetoothController;

        /// <summary>
        /// Sensor used to measure the proximity of objects to the front of the rover.
        /// </summary>
        Vl53l0x _distanceSensor;

        /// <summary>
        /// Create a new MarsRover object.
        /// </summary>
        public Rover()
        {
            _redLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedRed);
            _greenLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLedGreen);

            _driveController = new DriveController(Device);
            _bluetoothController = new BluetoothController(Device);

            _bluetoothController.Forward.ValueSet += Forward_ValueSet;
            _bluetoothController.Reverse.ValueSet += Reverse_ValueSet;
            _bluetoothController.Left.ValueSet += Left_ValueSet;
            _bluetoothController.Right.ValueSet += Right_ValueSet;

            var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);
            _distanceSensor = new Vl53l0x(Device, i2cBus);
            _distanceSensor.DistanceUpdated += _distanceSensor_Updated;
            _distanceSensor.StartUpdating(TimeSpan.FromMilliseconds(250));

            EnableDriveSystem();
        }

        /// <summary>
        /// Stop the rover and change the indicators accordingly.
        /// </summary>
        private void EmergencyStop()
        {
            _driveController.Stop();
            _greenLed.State = false;
            _redLed.State = true;
        }

        /// <summary>
        /// Indicate that the drive system can be used.
        /// </summary>
        private void EnableDriveSystem()
        {
            if (_driveController.Direction == DriveController.Directions.Stopped)
            {
                _redLed.State = false;
                _greenLed.State = true;
                //
                //  In the real world we would also give some feedback to the remote user / controller.
                //
            }
        }

        /// <summary>
        /// Check how close we are to objects and stop if necessary.
        /// </summary>
        private void _distanceSensor_Updated(object sender, IChangeResult<Meadow.Units.Length> result)
        {
            if (result.New == null)
            {
                return;
            }

            if (result.New < new Length(0, LU.Millimeters))
            {
                Console.WriteLine("Out of range.");
                EnableDriveSystem();
            }
            else
            {
                Console.WriteLine($"Range to object: {result.New.Millimeters} mm");
                if (result.New < new Length(40, LU.Millimeters))
                {
                    EmergencyStop();
                }
                else
                {
                    EnableDriveSystem();
                }
            }
        }

        #region Bluetooth event handlers

        /// <summary>
        /// Forward characteristic has changed, work out if we move forward or stop.
        /// </summary>
        private void Forward_ValueSet(Meadow.Gateways.Bluetooth.ICharacteristic c, object data)
        {
            if ((bool) data)
            {
                _driveController.Forward();
            }
            else
            {
                _driveController.Stop();
            }
        }

        /// <summary>
        /// Reverse characteristic has changed, work out if we move reverse or stop.
        /// </summary>
        private void Reverse_ValueSet(Meadow.Gateways.Bluetooth.ICharacteristic c, object data)
        {
            if ((bool) data)
            {
                _driveController.Reverse();
            }
            else
            {
                _driveController.Stop();
            }
        }

        /// <summary>
        /// Left characteristic has changed, work out if we turn left or stop.
        /// </summary>
        private void Left_ValueSet(Meadow.Gateways.Bluetooth.ICharacteristic c, object data)
        {
            if ((bool) data)
            {
                _driveController.TurnLeft();
            }
            else
            {
                _driveController.Stop();
            }
        }

        /// <summary>
        /// Right characteristic has changed, work out if we turn right or stop.
        /// </summary>
        private void Right_ValueSet(Meadow.Gateways.Bluetooth.ICharacteristic c, object data)
        {
            if ((bool) data)
            {
                _driveController.TurnRight();
            }
            else
            {
                _driveController.Stop();
            }


        }

        #endregion Bluetooth event handlers
    }
}
