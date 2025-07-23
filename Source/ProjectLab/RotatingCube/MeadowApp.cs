using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Leds;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace RotatingCube;

// Change ProjectLabCoreComputeApp to ProjectLabFeatherApp for ProjectLab v2
public class MeadowApp : ProjectLabCoreComputeApp
{
    private IRgbPwmLed onboardLed;
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

        Resolver.Log.Info($"Running on ProjectLab Hardware {Hardware.RevisionString}");

        onboardLed = Hardware.RgbLed;
        onboardLed.SetColor(Color.Red);

        graphics = new MicroGraphics(Hardware.Display)
        {
            Stroke = 3
        };

        Hardware.RightButton.Clicked += RightButton_Clicked;
        Hardware.LeftButton.Clicked += LeftButton_Clicked;
        Hardware.UpButton.Clicked += UpButton_Clicked;
        Hardware.DownButton.Clicked += DownButton_Clicked;

        Hardware.UpButton.LongClickedThreshold = TimeSpan.FromMilliseconds(500);
        Hardware.UpButton.LongClicked += UpButton_LongClicked;

        Hardware.DownButton.LongClickedThreshold = TimeSpan.FromMilliseconds(500);
        Hardware.DownButton.LongClicked += DownButton_LongClicked;

        Hardware.Accelerometer.Updated += Accelerometer_Updated;

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }

    private void Accelerometer_Updated(object sender, IChangeResult<Acceleration3D> e)
    {
        cube.XVelocity += new Angle(e.New.X.Gravity);
        cube.YVelocity -= new Angle(e.New.Y.Gravity);
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
        Hardware.Accelerometer.StartUpdating(motionUpdateInterval);

        cube = new Cube3d(graphics.Width / 2, graphics.Height / 2, cubeSize);
        cubeColor = initialColor;

        Show3dCube();

        return base.Run();
    }
}