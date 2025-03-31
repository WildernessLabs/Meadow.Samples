using Meadow;
using Meadow.Devices;
using Meadow.Gateways.Bluetooth;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Bluetooth_Restart;

// public class MeadowApp : App<F7FeatherV1>
// public class MeadowApp : App<F7FeatherV2>
public class MeadowApp : App<F7CoreComputeV2>
{
    /// <summary>
    /// Name of the number characteristic.
    /// </summary>
    const string SERVICE_A_NUMBER_FIELD_NAME = "NumberAField";

    /// <summary>
    /// UUID of the number characteristic.
    /// </summary>
    const string SERVICE_A_NUMBER_FIELD_UUID = "018e99d6-8a61-11eb-8dcd-0242ac1300bb";

    /// <summary>
    /// ID for central A (Espressif).
    /// </summary>
    const ushort CENTRAL_A_ID = 0x02e5;

    /// <summary>
    /// ID for central B (Test ID).
    /// </summary>
    const ushort CENTRAL_B_ID = 0xffff;

    /// <summary>
    /// Name of the text characteristic.
    /// </summary>
    const string SERVICE_B_NUMBER_FIELD_NAME = "NumberBField";

    /// <summary>
    /// UUID of the text characteristic.
    /// </summary>
    const string SERVICE_B_NUMBER_FIELD_UUID = "019e99d6-8a61-11eb-8dcd-0242ac1300cc";

    /// <summary>
    /// Action to be taken by the BLE server.
    /// </summary>
    enum BleAction : byte
    {
        /// <summary>
        /// End the test.
        /// </summary>
        End = 0,
        /// <summary>
        /// Switch the service to the alternative service.
        /// </summary>
        Switch = 1,
        /// <summary>
        /// Set the values of the characteristics.
        /// </summary> 
        SetValues = 2,
    }

    /// <summary>
    /// The service for central A.
    /// </summary>
    Service _serviceA;

    /// <summary>
    /// The service for central B.
    /// </summary>
    Service _serviceB;

    /// <summary>
    /// The definition of the services and characteristic for central A.
    /// </summary>
    Definition _bleADefinition;

    /// <summary>
    /// The definition of the services and characteristic for central B.
    /// </summary> 
    Definition _bleBDefinition;

    /// <summary>
    /// Semaphore used to indicate if a test is still running.
    /// </summary>
    private static SemaphoreSlim _semaphore = new SemaphoreSlim(0, 1);

    /// <summary>
    /// Set up the service and definition objects for the BLE server and start Bluetooth with the services
    /// defined for service A.
    /// </summary>
    public override Task Initialize()
    {
        Resolver.Log.Info("Starting the BLE server configured for service A.");

        SetupDefinitions();
        Device.BluetoothAdapter.StartBluetoothServer(_bleADefinition);

        return base.Initialize();
    }

    /// <summary>
    /// Run the test.
    /// </summary>
    public override Task Run()
    {
        _semaphore.Wait();
        //
        //  We have received the end signal from the client.
        //
        Device.BluetoothAdapter.StopBluetoothServer();
        Resolver.Log.Info("Test completed.");

        return Task.CompletedTask;
    }

    /// <summary>
    /// Switch the service to the alternative service.
    /// </summary>
    void SwitchService(Definition definition)
    {
        Device.BluetoothAdapter.StopBluetoothServer();
        Device.BluetoothAdapter.StartBluetoothServer(definition);
    }

    /// <summary>
    /// Set up the definitions, services and characteristics for the BLE services.
    /// </summary>
    protected void SetupDefinitions()
    {
        var numberFieldA = new CharacteristicInt32(
            SERVICE_A_NUMBER_FIELD_NAME,
            uuid: SERVICE_A_NUMBER_FIELD_UUID,
            permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
            properties: CharacteristicProperty.Write | CharacteristicProperty.Read
        );
        numberFieldA.ValueSet += (c, d) =>
        {
            Resolver.Log.Info($"Received data for Characteristic '{c.Name}' of type {d.GetType().Name}: {d}");
            if (d is int i)
            {
                switch (i)
                {
                    case 0: 
                        _semaphore.Release();
                        break;
                    case 1:
                        Resolver.Log.Info("Switching to service B.");
                        SwitchService(_bleBDefinition);
                        break;
                    default:
                        c.SetValue(i);
                        break;
                }
            }
            else
            {
                throw new ArgumentException("Data is not an integer.");
            }
        };

        _serviceA = new Service("GattServiceA", 0xfa, numberFieldA);
        _bleADefinition = new Definition("MeadowF7A", _serviceA);

        var numberFieldB = new CharacteristicInt32(
            SERVICE_B_NUMBER_FIELD_NAME,
            uuid: SERVICE_B_NUMBER_FIELD_UUID,
            permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
            properties: CharacteristicProperty.Write | CharacteristicProperty.Read
        );
        numberFieldB.ValueSet += (c, d) =>
        {
            Resolver.Log.Info($"Received data for Characteristic '{c.Name}' of type {d.GetType().Name}: {d}");
            if (d is int i)
            {
                switch (i)
                {
                    case 0: 
                        _semaphore.Release();
                        break;
                    case 1:
                        Resolver.Log.Info("Switching to service A.");
                        SwitchService(_bleADefinition);
                        break;
                    default:
                        c.SetValue(i);
                        break;
                }
            }
            else
            {
                throw new ArgumentException("Data is not an integer.");
            }
        };

        _serviceB = new Service("GattServiceB", 0xfb, numberFieldB);
        _bleBDefinition = new Definition("MeadowF7B", _serviceB);
    }
}