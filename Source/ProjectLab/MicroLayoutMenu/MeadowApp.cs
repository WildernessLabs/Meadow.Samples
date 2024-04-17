using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System.Threading.Tasks;

namespace MicroLayoutMenu;

// Change F7FeatherV2 to F7FeatherV1 for V1.x boards
public class MeadowApp : App<F7CoreComputeV2>
{
    private IProjectLabHardware _projectLab;
    private DisplayScreen _screen;
    private Menu _menu;

    public override Task Initialize()
    {
        _projectLab = ProjectLab.Create();

        _screen = new DisplayScreen(_projectLab.Display, RotationType._270Degrees);

        return base.Initialize();
    }

    void ShowDemoScreen()
    {
        var image = Image.LoadFromResource("MicroLayoutMenu.img_meadow.bmp");

        _screen.Controls.Add(
            new Box(0, 0, _screen.Width, _screen.Height)
            {
                ForeColor = Color.White
            },
            new Label(15, 20, 290, 40)
            {
                Text = "Welcome to MicroLayout!",
                TextColor = Color.Black,
                BackColor = Color.FromHex("#C9DB31"),
                Font = new Font12x20(),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            },
            new Picture(90, 74, 140, 90, image)
            {
                BackColor = Color.FromHex("#23ABE3"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            },
            new Button(30, 178, 120, 40)
            {
                Text = "Button 1",
                TextColor = Color.Black,
                Font = new Font12x20(),
            },
            new Button(170, 178, 120, 40)
            {
                Text = "Button 2",
                TextColor = Color.White,
                ForeColor = Color.Red,
                ShadowColor = Color.FromHex("#555555"),
                Font = new Font12x20(),
            });
    }

    void ShowMenuScreen()
    {
        var menuItems = new string[]
            {
                "Item A",
                "Item B",
                "Item C",
                "Item D",
                "Item E",
                "Item F",
            };

        _menu = new Menu(menuItems, _screen);

        _projectLab.UpButton.Clicked += (s, e) => _menu.Up();
        _projectLab.DownButton.Clicked += (s, e) => _menu.Down();
    }

    public override Task Run()
    {
        ShowDemoScreen();

        //ShowMenuScreen();

        return base.Run();
    }
}