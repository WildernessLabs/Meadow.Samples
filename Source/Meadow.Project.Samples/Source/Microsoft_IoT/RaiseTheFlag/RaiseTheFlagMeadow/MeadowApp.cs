using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Servos;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using System;
using System.Threading;
using Meadow.Units;
using System.Threading.Tasks;
using Meadow.Hardware;

namespace RaiseTheFlagMeadow
{
    // Change F7MicroV2 to F7Micro for V1.x boards
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        private PushButton _button;
        private Servo _servo;

        private Angle _downPosition = new Angle(0, Angle.UnitType.Degrees);
        private Angle _upPosition = new Angle(90, Angle.UnitType.Degrees);
        private IPin _buttonPin;
        private IPin _servoPin;

        private bool IsFlagMoving { get; set; } = false;

        public MeadowApp()
        {
            _buttonPin = Device.Pins.D01;
            _servoPin = Device.Pins.D03;

            Initialize();
        }

        private async void Initialize()
        {
            // create the push button
            _button = new PushButton(Device, _buttonPin, Meadow.Hardware.ResistorMode.InternalPullUp);

            _button.Clicked += OnButtonClicked;

            // create the servo object
            _servo = new Servo(Device.CreatePwmPort(_servoPin), NamedServoConfigs.SG90);

            // initialize to a known state
            await _servo.RotateTo(_downPosition, true);
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            RotateFlag();
        }

        private async void RotateFlag()
        {
            if (IsFlagMoving) return;

            IsFlagMoving = true;

            try
            {
                await _servo.RotateTo(_upPosition);
                await Task.Delay(2000);
                await _servo.RotateTo(_downPosition, true);
            }
            finally
            {
                IsFlagMoving = false;
            }
        }
    }
}
