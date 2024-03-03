using MagicEightMeadow.Controllers;
using MagicEightMeadow.Hardware;
using Meadow;
using Meadow.Foundation.Sensors.Accelerometers;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace MagicEightMeadow
{
    internal class MainController
    {
        private bool isAnswering;
        private IMagicEightMeadowHardware hardware;
        private DisplayController displayController;

        public MainController(IMagicEightMeadowHardware hardware)
        {
            this.hardware = hardware;
        }

        public void Initialize()
        {
            hardware.Initialize();

            displayController = new DisplayController(hardware.Display);

            var consumer = Bmi270.CreateObserver(
                handler: result => MotionSensorHandler(result),
                filter: result => MotionSensorFilter(result));
            hardware.MotionSensor.Subscribe(consumer);
        }

        private async void MotionSensorHandler(IChangeResult<(Acceleration3D? a3D, AngularVelocity3D? v3D, Temperature? t)> e)
        {
            if (isAnswering)
                return;
            isAnswering = true;

            hardware.RgbPwmLed.SetColor(Color.Orange);

            displayController.ShowAnswer();

            await Task.Delay(TimeSpan.FromSeconds(5));

            displayController.ShowQuestion();

            hardware.RgbPwmLed.SetColor(Color.Green);

            isAnswering = false;
        }

        private bool MotionSensorFilter(IChangeResult<(Acceleration3D? a3D, AngularVelocity3D? v3D, Temperature? t)> e)
        {
            return e.New.v3D.Value.X.DegreesPerSecond > 0.75 || e.New.v3D.Value.Y.DegreesPerSecond > 0.75 || e.New.v3D.Value.Z.DegreesPerSecond > 0.75;
        }

        public void Run()
        {
            displayController.ShowQuestion();

            hardware.MotionSensor.StartUpdating(TimeSpan.FromSeconds(1));
        }
    }
}