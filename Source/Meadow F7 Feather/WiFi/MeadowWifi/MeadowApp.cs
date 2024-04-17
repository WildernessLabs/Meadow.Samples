using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Gateways.Bluetooth;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace MeadowWifi;

public class MeadowApp : App<F7FeatherV2>
{
    readonly string SSID = "8c3bb16c0d954fb8b37999e1f040b279";
    readonly string PASSWORD = "b1cb00bd69424cba937a597fabb93052";
    readonly string TOGGLE_BLE_CONNECTION = "8ee2de4ced7c4f2c92a1f984507d87e3";
    readonly string TOGGLE_WIFI_CONNECTION = "98dea8431f7443a3bf24f78650672361";

    IDefinition bleTreeDefinition;

    ICharacteristic Ssid;
    ICharacteristic Password;
    ICharacteristic ToggleBleConnection;
    ICharacteristic ToggleWifiConnection;

    string ssid;
    string password;

    IWiFiNetworkAdapter wifi;

    RgbPwmLed onboardLed;

    public override Task Initialize()
    {
        onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue);
        onboardLed.StartPulse(Color.Red);

        bleTreeDefinition = GetDefinition();
        Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

        Ssid.ValueSet += (s, e) =>
        {
            ssid = (string)e;
        };
        Password.ValueSet += (s, e) =>
        {
            password = (string)e;
        };
        ToggleBleConnection.ValueSet += (s, e) =>
        {
            DisplayControllers.Instance.UpdateBluetoothStatus((bool)e
                ? "Paired"
                : "Not Paired");
        };
        ToggleWifiConnection.ValueSet += async (s, e) =>
        {
            _ = onboardLed.StartPulse(Color.Yellow);

            if ((bool)e)
            {
                DisplayControllers.Instance.UpdateWifiStatus("Connecting");
                await wifi.Connect(ssid, password, TimeSpan.FromSeconds(45));

                if (wifi.IsConnected)
                {
                    ConfigFileManager.CreateConfigFiles(ssid, password);
                }
            }
            else
            {
                DisplayControllers.Instance.UpdateWifiStatus("Disconnecting");
                await wifi.Disconnect(false);
            }
        };

        wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
        wifi.NetworkConnected += WifiNetworkConnected;
        wifi.NetworkDisconnected += WifiNetworkDisconnected;

        DisplayControllers.Instance.UpdateStatus();

        onboardLed.StartPulse(Color.Green);

        return base.Initialize();
    }

    private void WifiNetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
    {
        ToggleWifiConnection.SetValue(true);

        DisplayControllers.Instance.UpdateWifiStatus("Connected");

        onboardLed.StartPulse(Color.Magenta);
    }

    private void WifiNetworkDisconnected(INetworkAdapter sender, NetworkDisconnectionEventArgs args)
    {
        ToggleWifiConnection.SetValue(false);

        ConfigFileManager.DeleteConfigFiles();

        DisplayControllers.Instance.UpdateBluetoothStatus("Not Paired");
        DisplayControllers.Instance.UpdateWifiStatus("Disconnected");

        Device.PlatformOS.Reset();
    }

    Definition GetDefinition()
    {
        var service = new Service(
            name: "MeadowWifiService",
            uuid: 253,

            Ssid = new CharacteristicString(
                name: nameof(Ssid),
                uuid: SSID,
                permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                properties: CharacteristicProperty.Read | CharacteristicProperty.Write,
                maxLength: 256),
            Password = new CharacteristicString(
                name: nameof(Password),
                uuid: PASSWORD,
                permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                properties: CharacteristicProperty.Read | CharacteristicProperty.Write,
                maxLength: 256),
            ToggleBleConnection = new CharacteristicBool(
                name: nameof(ToggleBleConnection),
                uuid: TOGGLE_BLE_CONNECTION,
                permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                properties: CharacteristicProperty.Read | CharacteristicProperty.Write),
            ToggleWifiConnection = new CharacteristicBool(
                name: nameof(ToggleWifiConnection),
                uuid: TOGGLE_WIFI_CONNECTION,
                permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                properties: CharacteristicProperty.Read | CharacteristicProperty.Write)
        );

        return new Definition("MeadowWifi", service);
    }
}