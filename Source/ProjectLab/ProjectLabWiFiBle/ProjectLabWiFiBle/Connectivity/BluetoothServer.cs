using CommonContracts.Bluetooth;
using Meadow;
using Meadow.Gateways.Bluetooth;
using Meadow.Units;
using MeadowConnectedSample.Controllers;
using System;

namespace MeadowConnectedSample.Connectivity
{
    public class BluetoothServer
    {
        private static readonly Lazy<BluetoothServer> instance =
            new Lazy<BluetoothServer>(() => new BluetoothServer());
        public static BluetoothServer Instance => instance.Value;

        Definition bleTreeDefinition;
        ICharacteristic pairingCharacteristic;
        ICharacteristic ledToggleCharacteristic;
        ICharacteristic ledBlinkCharacteristic;
        ICharacteristic ledPulseCharacteristic;
        ICharacteristic environmentalDataCharacteristic;
        ICharacteristic lightDataCharacteristic;
        ICharacteristic motionAccelerationDataCharacteristic;
        ICharacteristic motionAngularVelocityDataCharacteristic;
        ICharacteristic motionTemperatureDataCharacteristic;

        public bool IsInitialized { get; private set; }

        private BluetoothServer() { }

        public void Initialize()
        {
            bleTreeDefinition = GetDefinition();
            pairingCharacteristic.ValueSet += PairingCharacteristicValueSet;
            ledToggleCharacteristic.ValueSet += LedToggleCharacteristicValueSet;
            ledBlinkCharacteristic.ValueSet += LedBlinkCharacteristicValueSet;
            ledPulseCharacteristic.ValueSet += LedPulseCharacteristicValueSet;
            MeadowApp.Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

            IsInitialized = true;
        }

        private void PairingCharacteristicValueSet(ICharacteristic c, object data)
        {
            if ((bool)data)
            {
                DisplayController.Instance.ShowBluetoothPaired();
            }
            else
            {
                _ = DisplayController.Instance.StartConnectingAnimation(false);
            }
        }

        private async void LedToggleCharacteristicValueSet(ICharacteristic c, object data)
        {
            await LedController.Instance.Toggle();
        }

        private async void LedBlinkCharacteristicValueSet(ICharacteristic c, object data)
        {
            await LedController.Instance.StartBlink();
        }

        private async void LedPulseCharacteristicValueSet(ICharacteristic c, object data)
        {
            await LedController.Instance.StartPulse();
        }

        public void SetEnvironmentalCharacteristicValue((Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance) value)
        {
            string stringValue = $"" +
                $"{(int)value.Temperature?.Celsius};" +
                $"{(int)value.Humidity?.Percent};" +
                $"{(int)value.Pressure?.Millibar}";
            Resolver.Log.Info(stringValue);
            environmentalDataCharacteristic.SetValue(stringValue);
        }

        public void SetLightCharacteristicValue(Illuminance? value)
        {
            lightDataCharacteristic.SetValue($"{(int)value?.Lux}");
        }

        public void SetMotionCharacteristicValue((Acceleration3D? acceleration3D, AngularVelocity3D? angularVelocity3D, Temperature? temperature) value)
        {
            string accelerationValue = $"" +
                $"{value.acceleration3D?.X.CentimetersPerSecondSquared:N2};" +
                $"{value.acceleration3D?.Y.CentimetersPerSecondSquared:N2};" +
                $"{value.acceleration3D?.Z.CentimetersPerSecondSquared:N2}";
            motionAccelerationDataCharacteristic.SetValue(accelerationValue);

            string angularVelocityValue = $"" +
                $"{value.angularVelocity3D?.X.DegreesPerSecond:N2};" +
                $"{value.angularVelocity3D?.Y.DegreesPerSecond:N2};" +
                $"{value.angularVelocity3D?.Z.DegreesPerSecond:N2}";
            motionAngularVelocityDataCharacteristic.SetValue(angularVelocityValue);

            string temperatureValue = $"{value.temperature?.Celsius:N2}";
            motionTemperatureDataCharacteristic.SetValue(temperatureValue);
        }

        Definition GetDefinition()
        {
            pairingCharacteristic = new CharacteristicBool(
                name: "PAIRING",
                uuid: CharacteristicsConstants.PAIRING,
                permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
            ledToggleCharacteristic = new CharacteristicBool(
                name: "LED_TOGGLE",
                uuid: CharacteristicsConstants.LED_TOGGLE,
                permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
            ledBlinkCharacteristic = new CharacteristicBool(
                name: "LED_BLINK",
                uuid: CharacteristicsConstants.LED_BLINK,
                permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
            ledPulseCharacteristic = new CharacteristicBool(
                name: "LED_PULSE",
                uuid: CharacteristicsConstants.LED_PULSE,
                permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                properties: CharacteristicProperty.Read | CharacteristicProperty.Write);
            environmentalDataCharacteristic = new CharacteristicString(
                name: "ENVIRONMENTAL_DATA",
                uuid: CharacteristicsConstants.ENVIRONMENTAL_DATA,
                maxLength: 20,
                permissions: CharacteristicPermission.Read,
                properties: CharacteristicProperty.Read);
            lightDataCharacteristic = new CharacteristicString(
                name: "LIGHT_DATA",
                uuid: CharacteristicsConstants.LIGHT_DATA,
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
            motionTemperatureDataCharacteristic = new CharacteristicString(
                name: "MOTION_TEMPERATURE",
                uuid: CharacteristicsConstants.MOTION_TEMPERATURE,
                maxLength: 20,
                permissions: CharacteristicPermission.Read,
                properties: CharacteristicProperty.Read);

            var service = new Service(
                name: "Service",
                uuid: 253,
                pairingCharacteristic,
                ledToggleCharacteristic,
                ledBlinkCharacteristic,
                ledPulseCharacteristic,
                environmentalDataCharacteristic,
                lightDataCharacteristic,
                motionAccelerationDataCharacteristic,
                motionAngularVelocityDataCharacteristic,
                motionTemperatureDataCharacteristic
            );

            return new Definition("ProjectLab", service);
        }
    }
}