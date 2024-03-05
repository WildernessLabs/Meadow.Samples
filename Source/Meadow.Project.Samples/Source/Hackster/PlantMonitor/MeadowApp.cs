using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading.Tasks;
using VU = Meadow.Units.Voltage.UnitType;

namespace PlantMonitor
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        readonly Voltage MINIMUM_VOLTAGE_CALIBRATION = new Voltage(2.81, VU.Volts);
        readonly Voltage MAXIMUM_VOLTAGE_CALIBRATION = new Voltage(1.50, VU.Volts);

        double moisture;
        Temperature temperature;

        RgbPwmLed onboardLed;
        PushButton button;
        Capacitive capacitive;
        AnalogTemperature analogTemperature;
        DisplayController displayController;

        public override Task Initialize()
        {
            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            button = new PushButton(Device.Pins.D04, ResistorMode.InternalPullUp);
            button.Clicked += ButtonClicked;

            var display = new St7789
            (
                spiBus: Device.CreateSpiBus(),
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );
            displayController = new DisplayController(display);

            capacitive = new Capacitive(
                analogInputPin: Device.Pins.A01,
                minimumVoltageCalibration: MINIMUM_VOLTAGE_CALIBRATION,
                maximumVoltageCalibration: MAXIMUM_VOLTAGE_CALIBRATION);

            var capacitiveObserver = Capacitive.CreateObserver(
                handler: result =>
                {
                    onboardLed.SetColor(Color.Purple);

                    displayController.UpdateMoistureImage(result.New);
                    displayController.UpdateMoisturePercentage(result.New, result.Old.Value);

                    onboardLed.SetColor(Color.Green);
                },
                filter: null
            );
            capacitive.Subscribe(capacitiveObserver);

            capacitive.StartUpdating(TimeSpan.FromHours(1));

            analogTemperature = new AnalogTemperature(Device.Pins.A00, AnalogTemperature.KnownSensorType.LM35);
            var analogTemperatureObserver = AnalogTemperature.CreateObserver(
                handler =>
                {
                    onboardLed.SetColor(Color.Purple);

                    displayController.UpdateTemperatureValue(handler.New, handler.Old.Value);

                    onboardLed.SetColor(Color.Green);
                },
                filter: null
            );
            analogTemperature.Subscribe(analogTemperatureObserver);
            analogTemperature.StartUpdating(TimeSpan.FromHours(1));

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        async void ButtonClicked(object sender, EventArgs e)
        {
            onboardLed.SetColor(Color.Orange);

            var newMoisture = await capacitive.Read();
            var newTemperature = await analogTemperature.Read();

            displayController.UpdateMoisturePercentage(newMoisture, moisture);
            moisture = newMoisture;

            displayController.UpdateTemperatureValue(newTemperature, temperature);
            temperature = newTemperature;

            onboardLed.SetColor(Color.Green);
        }
    }
}