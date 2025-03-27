using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System.Threading.Tasks;

namespace HMI_Views.Views;

class GridLayoutView
{
    public GridLayoutView(IPixelDisplay _display)
    {
        var screen = new DisplayScreen(_display);
        screen.BackgroundColor = Color.Azure;

        var layout = new GridLayout(0, 0, screen.Width, screen.Height, 3, 5);
        screen.Controls.Add(layout);

        var font = new Font12x16();


        var label = new Label(0, 0, 100, 50, ScaleFactor.X1)
        {
            Text = "Hello Grid!",
            Font = new Font12x16(),
            TextColor = Color.DarkBlue,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        layout.Add(label, 0, 0, 1, 3, GridLayout.Alignment.Center);

        var box = new Box(0, 0, 50, 50)
        {
            ForegroundColor = Color.Orange,
            IsFilled = true
        };
        layout.Add(box, 1, 0);

        box = new Box(0, 0, 1, 1)
        {
            ForegroundColor = Color.Red,
            IsFilled = true
        };
        layout.Add(box, 1, 1, 1, 2, GridLayout.Alignment.Stretch);

        box = new Box(0, 0, 1, 1)
        {
            ForegroundColor = Color.Blue,
            IsFilled = true
        };
        layout.Add(box, 0, 4, 2, 1, GridLayout.Alignment.Stretch);

        var buttonTheme = new DisplayTheme()
        {
            ForegroundColor = Color.Purple,
            HighlightColor = Color.DarkOliveGreen,
            ShadowColor = Color.DarkBlue,
            TextColor = Color.Black,
            Font = font,
        };

        var button1 = new Button(0, 0, 100, 50) { Text = "YES" };
        button1.ApplyTheme(buttonTheme);
        layout.Add(button1, 2, 2, 1, 1, GridLayout.Alignment.Stretch);

        var button2 = new Button(0, 0, 100, 50) { Text = "SKIP" };
        button2.ApplyTheme(buttonTheme);
        layout.Add(button2, 2, 3, 1, 1, GridLayout.Alignment.Stretch);

        var button3 = new Button(0, 0, 100, 50) { Text = "NO" };
        button3.ApplyTheme(buttonTheme);
        layout.Add(button3, 2, 4, 1, 1, GridLayout.Alignment.Stretch);
    }

    public Task Run()
    {
        return Task.CompletedTask;
    }
}