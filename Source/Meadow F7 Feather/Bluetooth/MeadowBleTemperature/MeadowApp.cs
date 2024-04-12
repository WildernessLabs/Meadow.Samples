using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Gateways.Bluetooth;
using MeadowBleTemperature.Controllers;
using System;
using System.Threading.Tasks;

namespace MeadowBleTemperature;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    readonly string TEMPERATURE = "e78f7b5e-842b-4b99-94e3-7401bf72b870";

    IDefinition bleTreeDefinition;

    ICharacteristic temperatureCharacteristic;

    public override Task Initialize()
    {
        var onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue);
        onboardLed.SetColor(Color.Red);

        TemperatureController.Instance.StartUpdating(TimeSpan.FromSeconds(5));

        bleTreeDefinition = GetDefinition();
        TemperatureController.Instance.TemperatureUpdated += TemperatureUpdated;
        Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }

    private void TemperatureUpdated(object sender, Meadow.Units.Temperature e)
    {
        temperatureCharacteristic.SetValue($"{e.Celsius:N2}°C;");
    }

    Definition GetDefinition()
    {
        temperatureCharacteristic = new CharacteristicString(
            name: "Temperature",
            uuid: TEMPERATURE,
            maxLength: 20,
            permissions: CharacteristicPermission.Read,
            properties: CharacteristicProperty.Read);

        var service = new Service(
            name: "ServiceA",
            uuid: 253,
            temperatureCharacteristic
        );

        return new Definition("MeadowTemperature", service);
    }
}