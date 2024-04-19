using Meadow;
using Meadow.Gateways.Bluetooth;
using Meadow.Units;
using MeadowBleServo.Controllers;

namespace MeadowBleServo.Connectivity;

public class BluetoothServer
{
    readonly string IS_SWEEPING = "24517ccc888e4ffc9da521884353b08d";
    readonly string ANGLE = "5a0bb01669ab4a49a2f2de5b292458f3";

    private ServoController servoController;

    ICharacteristic isSweepingCharacteristic;
    ICharacteristic angleCharacteristic;

    public BluetoothServer()
    {
        servoController = Resolver.Services.Get<ServoController>();
    }

    void IsSweepingCharacteristicValueSet(ICharacteristic c, object data)
    {
        if ((bool)data)
        {
            servoController.StopSweep();
            isSweepingCharacteristic.SetValue(false);
        }
        else
        {
            servoController.StartSweep();
            isSweepingCharacteristic.SetValue(true);
        }
    }

    void AngleCharacteristicValueSet(ICharacteristic c, object data)
    {
        int angle = (int)data;

        servoController.RotateTo(new Angle(angle));
    }

    public Definition GetDefinition()
    {
        isSweepingCharacteristic = new CharacteristicBool(
            name: "IsSweeping",
            uuid: IS_SWEEPING,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        isSweepingCharacteristic.ValueSet += IsSweepingCharacteristicValueSet;

        angleCharacteristic = new CharacteristicInt32(
            name: "Angle",
            uuid: ANGLE,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        angleCharacteristic.ValueSet += AngleCharacteristicValueSet;

        ICharacteristic[] characteristics =
        {
            isSweepingCharacteristic,
            angleCharacteristic
        };

        var service = new Service(
            name: "Service",
            uuid: 253,
            characteristics
        );

        return new Definition("MeadowServo", service);
    }
}