using Meadow;
using Meadow.Gateways.Bluetooth;
using MeadowBleLed.Controllers;

namespace MeadowBleLed.Connectivity;

public class BluetoothServer
{
    readonly string ON = "73cfbc6f61fa4d80a92feec2a90f8a3e";
    readonly string OFF = "6315119dd61949bba21def9e99941948";
    readonly string PULSING = "d755180131fc435da9941e7f15e17baf";
    readonly string BLINKING = "3a6cc4f2a6ab4709a9bfc9611c6bf892";
    readonly string RUNNING_COLORS = "30df1258f42b4788af2ea8ed9d0b932f";

    private CommandController commandController;

    ICharacteristic LedOn;
    ICharacteristic LedOff;
    ICharacteristic LedBlink;
    ICharacteristic LedPulse;
    ICharacteristic LedRunColors;

    public BluetoothServer()
    {
        commandController = Resolver.Services.Get<CommandController>();
    }

    private void LedOnCharacteristicValueSet(ICharacteristic c, object data)
    {
        commandController.FireLedOn();
    }

    private void LedOffCharacteristicValueSet(ICharacteristic c, object data)
    {
        commandController.FireLedOff();
    }

    private void LedBlinkCharacteristicValueSet(ICharacteristic c, object data)
    {
        commandController.FireLedBlink();
    }

    private void LedPulseCharacteristicValueSet(ICharacteristic c, object data)
    {
        commandController.FireLedPulse();
    }

    private void LedRunColorsCharacteristicValueSet(ICharacteristic c, object data)
    {
        commandController.FireLedRunColors();
    }

    public Definition GetDefinition()
    {
        LedOn = new CharacteristicBool(
            name: nameof(LedOn),
            uuid: ON,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        LedOn.ValueSet += LedOnCharacteristicValueSet;

        LedOff = new CharacteristicBool(
            name: nameof(LedOff),
            uuid: OFF,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        LedOff.ValueSet += LedOffCharacteristicValueSet;

        LedBlink = new CharacteristicBool(
            name: nameof(LedBlink),
            uuid: BLINKING,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        LedBlink.ValueSet += LedBlinkCharacteristicValueSet;

        LedPulse = new CharacteristicBool(
            name: nameof(LedPulse),
            uuid: PULSING,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        LedPulse.ValueSet += LedPulseCharacteristicValueSet;

        LedRunColors = new CharacteristicBool(
            name: nameof(LedRunColors),
            uuid: RUNNING_COLORS,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        LedRunColors.ValueSet += LedRunColorsCharacteristicValueSet;

        ICharacteristic[] characteristics =
        {
            LedOn,
            LedOff,
            LedBlink,
            LedPulse,
            LedRunColors
        };

        var service = new Service(
            name: "Service",
            uuid: 253,
            characteristics
        );

        return new Definition("MeadowRGB", service);
    }
}