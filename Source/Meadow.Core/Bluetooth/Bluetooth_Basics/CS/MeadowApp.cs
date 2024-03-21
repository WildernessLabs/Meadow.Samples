using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Gateways.Bluetooth;

namespace Bluetooth_Basics
{
    public class MeadowApp : App<F7FeatherV2>
    {
        Definition bleTreeDefinition;
        CharacteristicBool onOffCharacteristic;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize hardware...");

            // initialize the bluetooth defnition tree
            Resolver.Log.Info("Starting the BLE server.");
            bleTreeDefinition = GetDefinition();
            Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

            // wire up some notifications on set
            foreach (var characteristic in bleTreeDefinition.Services[0].Characteristics)
            {
                characteristic.ValueSet += (c, d) =>
                {
                    Resolver.Log.Info($"HEY, I JUST GOT THIS BLE DATA for Characteristic '{c.Name}' of type {d.GetType().Name}: {d}");
                };
            }

            // addressing individual characteristics:
            onOffCharacteristic.ValueSet += (c, d) =>
            {
                Resolver.Log.Info($"{c.Name}: {d}");
            };

            Resolver.Log.Info("Hardware initialized.");

            return Task.CompletedTask;
        }

        protected Definition GetDefinition()
        {
            onOffCharacteristic = new CharacteristicBool(
                    "On_Off",
                    Guid.NewGuid().ToString(),
                    CharacteristicPermission.Read | CharacteristicPermission.Write,
                    CharacteristicProperty.Read | CharacteristicProperty.Write);

            var service = new Service(
                 "ServiceA",
                 253,
                 onOffCharacteristic,

                 new CharacteristicBool(
                     "My Bool",
                     uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300aa",
                     permissions: CharacteristicPermission.Read,
                     properties: CharacteristicProperty.Read
                     ),

                 new CharacteristicInt32(
                     "My Number",
                     uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300bb",
                     permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                     properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                     ),

                 new CharacteristicString(
                     "My Text",
                     uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300cc",
                     maxLength: 20,
                     permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                     properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                     )
            );
            
            return new Definition("MY MEADOW F7", service);
        }
    }
}