using Meadow;
using Meadow.Devices;
using Meadow.Gateways.Bluetooth;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Bluetooth_Notify;

// public class MeadowApp : App<F7FeatherV1>
// public class MeadowApp : App<F7FeatherV2>
public class MeadowApp : App<F7CoreComputeV2>
{
    /// <summary>
    /// Name of the notify characteristic.
    /// </summary>
    const string NOTIFY_FIELD_NAME = "NotifyField";

    /// <summary>
    /// UUID of the notify characteristic.
    /// </summary>
    const string NOTIFY_FIELD_UUID = "018e99d6-8a61-11eb-8dcd-0242ac1300cc";

    /// <summary>
    /// Definition of the Bluetooth service.
    /// </summary>
    Definition bleTreeDefinition;

    /// <summary>
    /// Notification characteristic.
    /// </summary>
    CharacteristicInt32 _notifyCharacteristic;

    /// <summary>
    /// Set up the service and definition objects for the BLE server and start Bluetooth with the services
    /// defined for service A.
    /// </summary>
    public override Task Initialize()
    {
        Resolver.Log.Info("Starting the BLE server.");

        _notifyCharacteristic = new CharacteristicInt32(NOTIFY_FIELD_NAME,
                 uuid: NOTIFY_FIELD_UUID,
                 permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                 properties: CharacteristicProperty.Write | CharacteristicProperty.Read | CharacteristicProperty.Notify
                 );
        var service = new Service(
             "GattService",
             253,

             _notifyCharacteristic
        );
        bleTreeDefinition = new Definition(Device.Information.DeviceName, service);
        Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

        return base.Initialize();
    }

    /// <summary>
    /// Run the test.
    /// </summary>
    public override Task Run()
    {
        Resolver.Log.Info("Running Bluetooth_Notify test.");

        for (int index = 0; index < 60; index++)
        {
            _notifyCharacteristic.SetValue(index + 1);
            Resolver.Log.Info($"Notification value: {index + 1}");
            Thread.Sleep(1000);
        }

        Resolver.Log.Info("Test complete.");

        return Task.CompletedTask;
    }
}