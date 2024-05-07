using Meadow;
using Meadow.Gateways.Bluetooth;
using Meadow.Units;

namespace MeadowBleTemperature.Connectivity;

public class BluetoothServer
{
    readonly string TEMPERATURE = "e78f7b5e-842b-4b99-94e3-7401bf72b870";

    ICharacteristic temperatureCharacteristic;

    public BluetoothServer()
    {
        Resolver.Services.Add(this);
    }

    public void UpdateTemperatureCharacteristic(Temperature e)
    {
        temperatureCharacteristic.SetValue($"{e.Celsius:N2}°C;");
    }

    public Definition GetDefinition()
    {
        temperatureCharacteristic = new CharacteristicString(
            name: "Temperature",
            uuid: TEMPERATURE,
            maxLength: 20,
            permissions: CharacteristicPermission.Read,
            properties: CharacteristicProperty.Read);

        var service = new Service(
            name: "Service",
            uuid: 253,
            temperatureCharacteristic
        );

        return new Definition("MeadowTemperature", service);
    }
}