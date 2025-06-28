using Meadow;
using Meadow.Devices;
using Meadow.Gateways.Bluetooth;
using System;
using System.Threading.Tasks;

namespace Bluetooth_Basics;

// public class MeadowApp : App<F7FeatherV1>
//public class MeadowApp : App<F7CoreComputeV2>
public class MeadowApp : App<F7FeatherV2>
{
    private Definition bleTreeDefinition;
    private CharacteristicBool onOffCharacteristic;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize hardware...");

        // initialize the bluetooth definition tree
        Resolver.Log.Info("Starting the BLE server.");
        bleTreeDefinition = GetDefinition();

        Device.BluetoothAdapter.ServerStarting += (s, e) => { Resolver.Log.Info("Server starting..."); };
        Device.BluetoothAdapter.ServerStarted += (s, e) => { Resolver.Log.Info("Server started"); };
        Device.BluetoothAdapter.ServerStopping += (s, e) => { Resolver.Log.Info("Server stopping..."); };
        Device.BluetoothAdapter.ServerStopped += (s, e) => { Resolver.Log.Info("Server stopped"); };
        Device.BluetoothAdapter.ClientConnected += (s, e) => { Resolver.Log.Info("Client connected"); };
        Device.BluetoothAdapter.ClientDisconnected += (s, e) => { Resolver.Log.Info("Client disconnected"); };

        Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

        // wire up some notifications on set
        foreach (var characteristic in bleTreeDefinition.Services[0].Characteristics)
        {
            characteristic.ValueSet += (c, d) =>
            {
                Resolver.Log.Info($"HEY, I JUST GOT THIS BLE DATA for Characteristic '{c.Name}' of type {d.GetType().Name}: {d}");
                c.SetValue(d);
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

        return new Definition(Device.Information.DeviceName, service);
    }
}