﻿using GnssTrackerConnectivity.Common.Bluetooth;
using GnssTrackerConnectivity.Controllers;
using GnssTrackerConnectivity.Models;
using Meadow;
using Meadow.Gateways.Bluetooth;

namespace GnssTrackerConnectivity.Connectivity;

public class BluetoothServer
{
    private CommandController commandController;

    private ICharacteristic ledPairingCharacteristic;
    private ICharacteristic ledToggleCharacteristic;
    private ICharacteristic ledBlinkCharacteristic;
    private ICharacteristic ledPulseCharacteristic;
    private ICharacteristic atmosphericDataCharacteristic;
    private ICharacteristic motionDataCharacteristic;
    private ICharacteristic voltageDataCharacteristic;

    public BluetoothServer()
    {
        commandController = Resolver.Services.Get<CommandController>();

        var sensorController = Resolver.Services.Get<SensorController>();
        sensorController.AtmosphericConditionsChanged += UpdateAtmosphericConditions;
        sensorController.MotionConditionsChanged += UpdateMotionConditions;
        sensorController.VoltageReadingsChanged += UpdateVoltageReadings;
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
            $"{atmosphericConditions.Humidity.Percent:N1};" +
            $"{atmosphericConditions.GasResistance.Megaohms:N1};";
        Resolver.Log.Info($"{stringValue}");
        atmosphericDataCharacteristic.SetValue(stringValue);
    }

    public void UpdateMotionConditions(object sender, MotionConditions motionConditions)
    {
        string stringValue = $"" +
            $"{motionConditions.Acceleration3D.X.CentimetersPerSecondSquared:N1};" +
            $"{motionConditions.Acceleration3D.Y.CentimetersPerSecondSquared:N1};" +
            $"{motionConditions.Acceleration3D.Z.CentimetersPerSecondSquared:N1};" +
            $"{motionConditions.AngularVelocity3D.X.DegreesPerSecond:N1};" +
            $"{motionConditions.AngularVelocity3D.Y.DegreesPerSecond:N1};" +
            $"{motionConditions.AngularVelocity3D.Z.DegreesPerSecond:N1};";
        Resolver.Log.Info($"{stringValue}");
        motionDataCharacteristic.SetValue(stringValue);
    }

    private void UpdateVoltageReadings(object sender, VoltageReadings e)
    {
        string stringValue = $"" +
            $"{e.BatteryVoltage:N1};" +
            $"{e.SolarVoltage:N1};";
        Resolver.Log.Info($"{stringValue}");
        voltageDataCharacteristic.SetValue(stringValue);
    }

    public Definition GetDefinition()
    {
        ledPairingCharacteristic = new CharacteristicBool(
            name: nameof(CharacteristicsConstants.LED_PAIRING),
            uuid: CharacteristicsConstants.LED_PAIRING,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        ledPairingCharacteristic.ValueSet += PairingCharacteristicValueSet;

        ledToggleCharacteristic = new CharacteristicBool(
            name: nameof(CharacteristicsConstants.LED_TOGGLE),
            uuid: CharacteristicsConstants.LED_TOGGLE,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        ledToggleCharacteristic.ValueSet += LedToggleCharacteristicValueSet;

        ledBlinkCharacteristic = new CharacteristicBool(
            name: nameof(CharacteristicsConstants.LED_BLINK),
            uuid: CharacteristicsConstants.LED_BLINK,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        ledBlinkCharacteristic.ValueSet += LedBlinkCharacteristicValueSet;

        ledPulseCharacteristic = new CharacteristicBool(
            name: nameof(CharacteristicsConstants.LED_PULSE),
            uuid: CharacteristicsConstants.LED_PULSE,
            permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
            properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
        ledPulseCharacteristic.ValueSet += LedPulseCharacteristicValueSet;

        atmosphericDataCharacteristic = new CharacteristicString(
            name: nameof(CharacteristicsConstants.ATMOSPHERIC_DATA),
            uuid: CharacteristicsConstants.ATMOSPHERIC_DATA,
            maxLength: 32,
            permissions: CharacteristicPermission.Read,
            properties: CharacteristicProperty.Read);

        motionDataCharacteristic = new CharacteristicString(
            name: nameof(CharacteristicsConstants.MOTION_DATA),
            uuid: CharacteristicsConstants.MOTION_DATA,
            maxLength: 32,
            permissions: CharacteristicPermission.Read,
            properties: CharacteristicProperty.Read);

        voltageDataCharacteristic = new CharacteristicString(
            name: nameof(CharacteristicsConstants.VOLTAGE_DATA),
            uuid: CharacteristicsConstants.VOLTAGE_DATA,
            maxLength: 32,
            permissions: CharacteristicPermission.Read,
            properties: CharacteristicProperty.Read);

        ICharacteristic[] characteristics =
        {
            ledPairingCharacteristic,
            ledToggleCharacteristic,
            ledBlinkCharacteristic,
            ledPulseCharacteristic,
            atmosphericDataCharacteristic,
            motionDataCharacteristic,
            voltageDataCharacteristic
        };

        var service = new Service(
            name: "Service",
            uuid: 253,
            characteristics
        );

        return new Definition("GnssTracker", service);
    }
}