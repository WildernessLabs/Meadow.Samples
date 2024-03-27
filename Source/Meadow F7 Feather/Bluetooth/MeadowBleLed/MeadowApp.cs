using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Gateways.Bluetooth;
using MeadowBleLed.Controller;
using System.Threading.Tasks;

namespace MeadowBleLed
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        readonly string ON = "73cfbc6f61fa4d80a92feec2a90f8a3e";
        readonly string OFF = "6315119dd61949bba21def9e99941948";
        readonly string PULSING = "d755180131fc435da9941e7f15e17baf";
        readonly string BLINKING = "3a6cc4f2a6ab4709a9bfc9611c6bf892";
        readonly string RUNNING_COLORS = "30df1258f42b4788af2ea8ed9d0b932f";

        IDefinition bleTreeDefinition;

        ICharacteristic On;
        ICharacteristic Off;
        ICharacteristic StartPulse;
        ICharacteristic StartBlink;
        ICharacteristic StartRunningColors;

        public override Task Initialize()
        {
            LedController.Instance.SetColor(Color.Red);

            bleTreeDefinition = GetDefinition();
            Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

            On.ValueSet += (s,e) => { LedController.Instance.TurnOn(); };
            Off.ValueSet += (s, e) => { LedController.Instance.TurnOff(); };
            StartPulse.ValueSet += (s, e) => { LedController.Instance.StartPulse(); };
            StartBlink.ValueSet += (s, e) => { LedController.Instance.StartBlink(); };
            StartRunningColors.ValueSet += (s, e) => { LedController.Instance.StartRunningColors(); };

            LedController.Instance.SetColor(Color.Green);

            return base.Initialize();
        }

        Definition GetDefinition()
        {
            var service = new Service(
                name: "MeadowRGBService",
                uuid: 253,
                On = new CharacteristicBool(
                    name: nameof(On),
                    uuid: ON,
                    permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                    properties: CharacteristicProperty.Read | CharacteristicProperty.Write), 
                Off = new CharacteristicBool(
                    name: nameof(Off),
                    uuid: OFF,
                    permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                    properties: CharacteristicProperty.Read | CharacteristicProperty.Write),
                StartPulse = new CharacteristicBool(
                    name: nameof(StartPulse),
                    uuid: PULSING,
                    permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                    properties: CharacteristicProperty.Read | CharacteristicProperty.Write),
                StartBlink = new CharacteristicBool(
                    name: nameof(StartBlink),
                    uuid: BLINKING,
                    permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                    properties: CharacteristicProperty.Read | CharacteristicProperty.Write),
                StartRunningColors = new CharacteristicBool(
                    name: nameof(StartRunningColors),
                    uuid: RUNNING_COLORS,
                    permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                    properties: CharacteristicProperty.Read | CharacteristicProperty.Write)
            );

            return new Definition("MeadowRGB", service);
        }
    }
}