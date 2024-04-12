using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Motors;
using Meadow.Gateways.Bluetooth;
using Meadow.Peripherals.Leds;
using System.Threading.Tasks;

namespace MeadowBleRover;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    Definition bleTreeDefinition;
    CharacteristicBool up, down, left, right;

    RgbLed led;
    Led ledUp, ledDown, ledLeft, ledRight;

    CarController carController;

    public override Task Initialize()
    {
        led = new RgbLed(
            Device.Pins.OnboardLedRed,
            Device.Pins.OnboardLedGreen,
            Device.Pins.OnboardLedBlue);
        led.SetColor(RgbLedColors.Red);

        ledUp = new Led(Device.Pins.D13);
        ledDown = new Led(Device.Pins.D10);
        ledLeft = new Led(Device.Pins.D11);
        ledRight = new Led(Device.Pins.D12);
        ledUp.IsOn = ledDown.IsOn = ledLeft.IsOn = ledRight.IsOn = true;

        var motorLeft = new HBridgeMotor
        (
            a1Pin: Device.Pins.D07,
            a2Pin: Device.Pins.D08,
            enablePin: Device.Pins.D09
        );
        var motorRight = new HBridgeMotor
        (
            a1Pin: Device.Pins.D02,
            a2Pin: Device.Pins.D03,
            enablePin: Device.Pins.D04
        );

        carController = new CarController(motorLeft, motorRight);

        led.SetColor(RgbLedColors.Blue);

        bleTreeDefinition = GetDefinition();
        Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

        up.ValueSet += UpValueSet;
        down.ValueSet += DownValueSet;
        left.ValueSet += LeftValueSet;
        right.ValueSet += RightValueSet;

        led.SetColor(RgbLedColors.Green);

        return base.Initialize();
    }

    void UpValueSet(ICharacteristic c, object data)
    {
        ledUp.IsOn = (bool)data;
        if ((bool)data)
            carController.MoveForward();
        else
            carController.Stop();
    }
    void DownValueSet(ICharacteristic c, object data)
    {
        ledDown.IsOn = (bool)data;
        if ((bool)data)
            carController.MoveBackward();
        else
            carController.Stop();
    }
    void LeftValueSet(ICharacteristic c, object data)
    {
        ledLeft.IsOn = (bool)data;
        if ((bool)data)
            carController.TurnLeft();
        else
            carController.Stop();
    }
    void RightValueSet(ICharacteristic c, object data)
    {
        ledRight.IsOn = (bool)data;
        if ((bool)data)
            carController.TurnRight();
        else
            carController.Stop();
    }

    Definition GetDefinition()
    {
        up = new CharacteristicBool(
            "Up",
                uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300aa",
                permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                properties: CharacteristicProperty.Write | CharacteristicProperty.Read
            );
        down = new CharacteristicBool(
            "Down",
                uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300bb",
                permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                properties: CharacteristicProperty.Write | CharacteristicProperty.Read
            );
        left = new CharacteristicBool(
            "Left",
                uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300cc",
                permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                properties: CharacteristicProperty.Write | CharacteristicProperty.Read
            );
        right = new CharacteristicBool(
            "Right",
                uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300dd",
                permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                properties: CharacteristicProperty.Write | CharacteristicProperty.Read
            );

        var service = new Service(
            name: "ServiceA",
            uuid: 253,
            up, down, left, right
        );

        return new Definition("MeadowRover", service);
    }
}