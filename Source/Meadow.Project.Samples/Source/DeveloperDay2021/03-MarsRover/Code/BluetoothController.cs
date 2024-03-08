using System;
using Meadow;
using Meadow.Devices;
using Meadow.Gateways.Bluetooth;

namespace MarsRover
{
    public class BluetoothController
    {
        /// <summary>
        /// BLE Tree containing the characteristics.
        /// </summary>
        public Definition BleTreeDefinition { get; private set; }

        /// <summary>
        /// Boolean characteristic that is set when the controller wants to indicate a forward movement.
        /// </summary>
        public CharacteristicBool Forward { get; private set; }

        /// <summary>
        /// Boolean characteristic that is set when the controller wants to indicate a reverse movement.
        /// </summary>
        public CharacteristicBool Reverse { get; private set; }

        /// <summary>
        /// Boolean characteristic that is set when the controller wants to indicate a move to the left.
        /// </summary>
        public CharacteristicBool Left { get; private set; }

        /// <summary>
        /// Boolean characteristic that is set when the controller wants to indicate a move to the right.
        /// </summary>
        public CharacteristicBool Right { get; private set; }

        /// <summary>
        /// Create a new Bluetooth controller object with characteristics for directional control.
        /// </summary>
        /// <param name="_device">Hardware device the system is running on.</param>
        public BluetoothController(F7Micro _device)
        {
            Forward = new CharacteristicBool(
                "Forward",
                    uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300aa",
                    permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                    properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                );
            Reverse = new CharacteristicBool(
                "Reverse",
                    uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300bb",
                    permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                    properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                );
            Left = new CharacteristicBool(
                "Left",
                    uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300cc",
                    permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                    properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                );
            Right = new CharacteristicBool(
                "Right",
                    uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300dd",
                    permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                    properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                );

            var service = new Service(name: "DirectionalControl", uuid: 253, Forward, Reverse, Left, Right);

            BleTreeDefinition =  new Definition("Mars Rover", service);
            _device.BluetoothAdapter.StartBluetoothServer(BleTreeDefinition);
        }
    }
}
