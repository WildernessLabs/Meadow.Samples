using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using Meadow.Units;

namespace Meadow.Cloud_Update
{
    public class DisplayController
    {
        readonly MicroGraphics canvas;

        public (Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance)? AtmosphericConditions
        {
            get => atmosphericConditions;
            set
            {
                atmosphericConditions = value;
                Update();
            }
        }
        (Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance)? atmosphericConditions;

        public Illuminance? LightConditions
        {
            get => lightConditions;
            set
            {
                lightConditions = value;
                Update();
            }
        }
        Illuminance? lightConditions;

        public (Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, Temperature? Temperature) AccelerationConditions
        {
            get => accelerationConditions;
            set
            {
                accelerationConditions = value;
                Update();
            }
        }
        (Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, Temperature? Temperature) accelerationConditions;

        public string Notification
        {
            get => notification;
            set
            {
                notification = value;
                Update();
            }
        }
        string notification = string.Empty;

        public bool UpButtonState
        {
            get => upButtonState;
            set
            {
                upButtonState = value;
                Update();
            }
        }
        bool upButtonState = false;

        public bool DownButtonState
        {
            get => downButtonState;
            set
            {
                downButtonState = value;
                Update();
            }
        }
        bool downButtonState = false;

        public bool LeftButtonState
        {
            get => leftButtonState;
            set
            {
                leftButtonState = value;
                Update();
            }
        }
        bool leftButtonState = false;

        public bool RightButtonState
        {
            get => rightButtonState;
            set
            {
                rightButtonState = value;
                Update();
            }
        }
        bool rightButtonState = false;

        bool isUpdating = false;
        bool needsUpdate = false;

        public DisplayController(IPixelDisplay display)
        {
            canvas = new MicroGraphics(display)
            {
                CurrentFont = new Font12x16()
            };

            canvas.Clear(true);
        }

        public void Update()
        {
            if (isUpdating)
            {   //queue up the next update
                needsUpdate = true;
                return;
            }

            isUpdating = true;

            canvas.Clear();
            Draw();
            canvas.Show();

            isUpdating = false;

            if (needsUpdate)
            {
                needsUpdate = false;
                Update();
            }
        }

        void DrawStatus(string label, string value, Color color, int yPosition)
        {
            canvas.DrawText(x: 2, y: yPosition, label, color: color);
            canvas.DrawText(x: canvas.Width - 2, y: yPosition, value, alignmentH: HorizontalAlignment.Right, color: color);
        }

        void Draw()
        {
            canvas.DrawText(x: 2, y: 0, "Hello PROJ LAB!", WildernessLabsColors.AzureBlue);

            //---- notification
            canvas.DrawText(x: 2, y: 17, notification, WildernessLabsColors.PearGreen);

            if (AtmosphericConditions is { } conditions)
            {
                if (conditions.Temperature is { } temp)
                {
                    DrawStatus("Temperature:", $"{temp.Celsius:N1}C", WildernessLabsColors.GalleryWhite, 35);
                }

                if (conditions.Pressure is { } pressure)
                {
                    DrawStatus("Pressure:", $"{pressure.StandardAtmosphere:N1}atm", WildernessLabsColors.GalleryWhite, 55);
                }

                if (conditions.Humidity is { } humidity)
                {
                    DrawStatus("Humidity:", $"{humidity.Percent:N1}%", WildernessLabsColors.GalleryWhite, 75);
                }
            }

            if (LightConditions is { } light)
            {
                DrawStatus("Lux:", $"{light:N0}Lux", WildernessLabsColors.GalleryWhite, 95);
            }

            if (AccelerationConditions is { } acceleration)
            {
                if (acceleration.Acceleration3D is { } accel3D)
                {
                    DrawStatus("Accel:", $"{accel3D.X.Gravity:0.#},{accel3D.Y.Gravity:0.#},{accel3D.Z.Gravity:0.#}g", WildernessLabsColors.AzureBlue, 115);
                }

                if (acceleration.AngularVelocity3D is { } angular3D)
                {
                    DrawStatus("Gyro:", $"{angular3D.X:0},{angular3D.Y:0},{angular3D.Z:0}rpm", WildernessLabsColors.AzureBlue, 135);
                }
            }

            DrawStatus("Left:", $"{(LeftButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 200);
            DrawStatus("Down:", $"{(DownButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 180);
            DrawStatus("Up:", $"{(UpButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 160);
            DrawStatus("Right:", $"{(RightButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 220);
        }
    }
}