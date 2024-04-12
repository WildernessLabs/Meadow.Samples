using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Gateways.Bluetooth;
using Meadow.Units;
using MeadowBleServo.Controllers;
using System.Threading.Tasks;

namespace MeadowBleServo;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    readonly string IS_SWEEPING = "24517ccc888e4ffc9da521884353b08d";
    readonly string ANGLE = "5a0bb01669ab4a49a2f2de5b292458f3";

    IDefinition bleTreeDefinition;

    ICharacteristic isSweepingCharacteristic;
    ICharacteristic angleCharacteristic;

    public override Task Initialize()
    {
        var onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue);
        onboardLed.SetColor(Color.Red);

        ServoController.Instance.Initialize();

        bleTreeDefinition = GetDefinition();
        Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

        isSweepingCharacteristic.ValueSet += IsSweepingCharacteristicValueSet;
        angleCharacteristic.ValueSet += AngleCharacteristicValueSet;

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }

    void IsSweepingCharacteristicValueSet(ICharacteristic c, object data)
    {
        if ((bool)data)
        {
            ServoController.Instance.StopSweep();
            isSweepingCharacteristic.SetValue(false);
        }
        else
        {
            ServoController.Instance.StartSweep();
            isSweepingCharacteristic.SetValue(true);
        }
    }

    void AngleCharacteristicValueSet(ICharacteristic c, object data)
    {
        int angle = (int)data;

        ServoController.Instance.RotateTo(new Angle(angle));
    }

    Definition GetDefinition()
    {
        isSweepingCharacteristic = new CharacteristicBool(
            name: "IsSweeping",
            uuid: IS_SWEEPING,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);

        angleCharacteristic = new CharacteristicInt32(
            name: "Angle",
            uuid: ANGLE,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);

        var service = new Service(
            name: "ServiceA",
            uuid: 253,
            isSweepingCharacteristic,
            angleCharacteristic
        );

        return new Definition("MeadowServo", service);
    }
}