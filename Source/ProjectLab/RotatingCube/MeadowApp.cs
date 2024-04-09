using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Leds;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace RotatingCube
{
    // Change F7CoreComputeV2 to F7FeatherV2 for ProjectLab v2
    public class MeadowApp : App<F7CoreComputeV2>
    {
        private IRgbPwmLed onboardLed;
        private IProjectLabHardware projectLab;
        private MicroGraphics graphics;
        private Cube3d cube;
        private Color cubeColor;
        private readonly Angle ButtonStep = new Angle(1);
        private readonly TimeSpan motionUpdateInterval = TimeSpan.FromMilliseconds(250);
        private readonly int cubeSize = 60;
        private readonly Color initialColor = Color.Cyan;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            projectLab = ProjectLab.Create();
            Resolver.Log.Info($"Running on ProjectLab Hardware {projectLab.RevisionString}");

            onboardLed = projectLab.RgbLed;
            onboardLed.SetColor(Color.Red);

            graphics = new MicroGraphics(projectLab.Display)
            {
                Stroke = 3
            };

            projectLab.RightButton.Clicked += RightButton_Clicked;
            projectLab.LeftButton.Clicked += LeftButton_Clicked;
            projectLab.UpButton.Clicked += UpButton_Clicked;
            projectLab.DownButton.Clicked += DownButton_Clicked;

            projectLab.UpButton.LongClickedThreshold = TimeSpan.FromMilliseconds(500);
            projectLab.UpButton.LongClicked += UpButton_LongClicked;

            projectLab.DownButton.LongClickedThreshold = TimeSpan.FromMilliseconds(500);
            projectLab.DownButton.LongClicked += DownButton_LongClicked;

            (projectLab as ProjectLabHardwareBase).MotionSensor.Updated += MotionSensor_Updated;

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        public void Show3dCube()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    graphics.Clear();

                    cube.Update();

                    DrawWireframe(cubeColor);

                    graphics.Show();

                    cubeColor = cubeColor.WithHue((float)(cubeColor.Hue + 0.001));
                }
            });
        }

        private void DrawWireframe(Color color)
        {
            graphics.DrawLine(cube.Wireframe[0, 0], cube.Wireframe[0, 1], cube.Wireframe[1, 0], cube.Wireframe[1, 1], color);
            graphics.DrawLine(cube.Wireframe[1, 0], cube.Wireframe[1, 1], cube.Wireframe[2, 0], cube.Wireframe[2, 1], color);
            graphics.DrawLine(cube.Wireframe[2, 0], cube.Wireframe[2, 1], cube.Wireframe[3, 0], cube.Wireframe[3, 1], color);
            graphics.DrawLine(cube.Wireframe[3, 0], cube.Wireframe[3, 1], cube.Wireframe[0, 0], cube.Wireframe[0, 1], color);

            //cross face above
            graphics.DrawLine(cube.Wireframe[1, 0], cube.Wireframe[1, 1], cube.Wireframe[3, 0], cube.Wireframe[3, 1], color);
            graphics.DrawLine(cube.Wireframe[0, 0], cube.Wireframe[0, 1], cube.Wireframe[2, 0], cube.Wireframe[2, 1], color);

            graphics.DrawLine(cube.Wireframe[4, 0], cube.Wireframe[4, 1], cube.Wireframe[5, 0], cube.Wireframe[5, 1], color);
            graphics.DrawLine(cube.Wireframe[5, 0], cube.Wireframe[5, 1], cube.Wireframe[6, 0], cube.Wireframe[6, 1], color);
            graphics.DrawLine(cube.Wireframe[6, 0], cube.Wireframe[6, 1], cube.Wireframe[7, 0], cube.Wireframe[7, 1], color);
            graphics.DrawLine(cube.Wireframe[7, 0], cube.Wireframe[7, 1], cube.Wireframe[4, 0], cube.Wireframe[4, 1], color);

            graphics.DrawLine(cube.Wireframe[0, 0], cube.Wireframe[0, 1], cube.Wireframe[4, 0], cube.Wireframe[4, 1], color);
            graphics.DrawLine(cube.Wireframe[1, 0], cube.Wireframe[1, 1], cube.Wireframe[5, 0], cube.Wireframe[5, 1], color);
            graphics.DrawLine(cube.Wireframe[2, 0], cube.Wireframe[2, 1], cube.Wireframe[6, 0], cube.Wireframe[6, 1], color);
            graphics.DrawLine(cube.Wireframe[3, 0], cube.Wireframe[3, 1], cube.Wireframe[7, 0], cube.Wireframe[7, 1], color);
        }

        private void MotionSensor_Updated(object sender, IChangeResult<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, Temperature? Temperature)> e)
        {
            cube.XVelocity += new Angle(e.New.Acceleration3D.Value.X.Gravity);
            cube.YVelocity -= new Angle(e.New.Acceleration3D.Value.Y.Gravity);
        }

        private void UpButton_LongClicked(object sender, EventArgs e)
        {
            if (cube != null)
            {
                cube.XVelocity = cube.YVelocity = cube.ZVelocity = new Angle(0);
            }
        }

        private void DownButton_LongClicked(object sender, EventArgs e)
        {
            if (cube != null)
            {
                cube.XRotation = cube.YRotation = cube.ZRotation = new Angle(0);
            }
        }

        private void DownButton_Clicked(object sender, EventArgs e)
        {
            if (cube != null)
            {
                cube.XVelocity -= ButtonStep;
            }
        }

        private void UpButton_Clicked(object sender, EventArgs e)
        {
            if (cube != null)
            {
                cube.XVelocity += ButtonStep;
            }
        }

        private void LeftButton_Clicked(object sender, EventArgs e)
        {
            if (cube != null)
            {
                cube.YVelocity -= ButtonStep;
            }
        }

        private void RightButton_Clicked(object sender, EventArgs e)
        {
            if (cube != null)
            {
                cube.YVelocity += ButtonStep;
            }
        }

        public override Task Run()
        {
            (projectLab as ProjectLabHardwareBase).MotionSensor.StartUpdating(motionUpdateInterval);

            cube = new Cube3d(graphics.Width / 2, graphics.Height / 2, cubeSize);
            cubeColor = initialColor;

            Show3dCube();

            return base.Run();
        }
    }
}