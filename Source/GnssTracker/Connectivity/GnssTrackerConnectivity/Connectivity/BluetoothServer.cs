using GnssTrackerConnectivity.Common.Bluetooth;
using GnssTrackerConnectivity.Controllers;
using GnssTrackerConnectivity.Models;
using Meadow;
using Meadow.Gateways.Bluetooth;

namespace GnssTrackerConnectivity.Connectivity;

public class BluetoothServer
{
    private CommandController commandController;

    private ICharacteristic pairingCharacteristic;
    private ICharacteristic ledToggleCharacteristic;
    private ICharacteristic ledBlinkCharacteristic;
    private ICharacteristic ledPulseCharacteristic;
    private ICharacteristic environmentalDataCharacteristic;
    private ICharacteristic motionAccelerationDataCharacteristic;
    private ICharacteristic motionAngularVelocityDataCharacteristic;

    public BluetoothServer()
    {
        commandController = Resolver.Services.Get<CommandController>();

        var sensorController = Resolver.Services.Get<SensorController>();
        sensorController.AtmosphericConditionsChanged += UpdateAtmosphericConditions;
        sensorController.MotionConditionsChanged += UpdateMotionConditions;
    }

    private void PairingCharacteristicValueSet(ICharacteristic c, object data)
    {
        commandController.FirePairing((bool)data);
    }

    private void LedToggleCharacteristicValueSet(ICharacteristic c, object data)
    {
        commandController.FireLedToggle();
    }

    private void LedBlinkCharacteristicValueSet(ICharacteristic c, object data)
    {
        commandController.FireLedBlink();
    }

    private void LedPulseCharacteristicValueSet(ICharacteristic c, object data)
    {
        commandController.FireLedPulse();
    }

    public void UpdateAtmosphericConditions(object sender, AtmosphericConditions atmosphericConditions)
    {
        string stringValue = $"" +
            $"{atmosphericConditions.Temperature.Celsius:N1};" +
            $"{atmosphericConditions.Pressure.StandardAtmosphere:N1};" +
            $"{atmosphericConditions.Humidity.Percent:N1}";
        environmentalDataCharacteristic.SetValue(stringValue);
    }

    public void UpdateMotionConditions(object sender, MotionConditions motionConditions)
    {
        string accelerationValue = $"" +
            $"{motionConditions.Acceleration3D.X.CentimetersPerSecondSquared:N2};" +
            $"{motionConditions.Acceleration3D.Y.CentimetersPerSecondSquared:N2};" +
            $"{motionConditions.Acceleration3D.Z.CentimetersPerSecondSquared:N2}";
        motionAccelerationDataCharacteristic.SetValue(accelerationValue);

        string angularVelocityValue = $"" +
            $"{motionConditions.AngularVelocity3D.X.DegreesPerSecond:N2};" +
            $"{motionConditions.AngularVelocity3D.Y.DegreesPerSecond:N2};" +
            $"{motionConditions.AngularVelocity3D.Z.DegreesPerSecond:N2}";
        motionAngularVelocityDataCharacteristic.SetValue(angularVelocityValue);
    }

    public Definition GetDefinition()
    {
        pairingCharacteristic = new CharacteristicBool(
            name: "PAIRING",
            uuid: CharacteristicsConstants.PAIRING,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        pairingCharacteristic.ValueSet += PairingCharacteristicValueSet;

        ledToggleCharacteristic = new CharacteristicBool(
            name: "LED_TOGGLE",
            uuid: CharacteristicsConstants.LED_TOGGLE,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        ledToggleCharacteristic.ValueSet += LedToggleCharacteristicValueSet;

        ledBlinkCharacteristic = new CharacteristicBool(
            name: "LED_BLINK",
            uuid: CharacteristicsConstants.LED_BLINK,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        ledBlinkCharacteristic.ValueSet += LedBlinkCharacteristicValueSet;

        ledPulseCharacteristic = new CharacteristicBool(
            name: "LED_PULSE",
            uuid: CharacteristicsConstants.LED_PULSE,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        ledPulseCharacteristic.ValueSet += LedPulseCharacteristicValueSet;

        environmentalDataCharacteristic = new CharacteristicString(
            name: "ENVIRONMENTAL_DATA",
            uuid: CharacteristicsConstants.ENVIRONMENTAL_DATA,
            maxLength: 20,
            permissions: CharacteristicPermission.Read,
            properties: CharacteristicProperty.Read);

        motionAccelerationDataCharacteristic = new CharacteristicString(
            name: "MOTION_ACCELERATION",
            uuid: CharacteristicsConstants.MOTION_ACCELERATION,
            maxLength: 20,
            permissions: CharacteristicPermission.Read,
            properties: CharacteristicProperty.Read);

        motionAngularVelocityDataCharacteristic = new CharacteristicString(
            name: "MOTION_ANGULAR_VELOCITY",
            uuid: CharacteristicsConstants.MOTION_ANGULAR_VELOCITY,
            maxLength: 20,
            permissions: CharacteristicPermission.Read,
            properties: CharacteristicProperty.Read);

        ICharacteristic[] characteristics =
        {
            pairingCharacteristic,
            ledToggleCharacteristic,
            ledBlinkCharacteristic,
            ledPulseCharacteristic,
            environmentalDataCharacteristic,
            motionAccelerationDataCharacteristic,
            motionAngularVelocityDataCharacteristic
        };

        var service = new Service(
            name: "Service",
            uuid: 253,
            characteristics
        );

        return new Definition("ProjectLab", service);
    }
}