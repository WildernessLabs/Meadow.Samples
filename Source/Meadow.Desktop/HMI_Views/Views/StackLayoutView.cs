using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System.Threading.Tasks;

namespace HMI_Views.Views;

class StackLayoutView
{
    public StackLayoutView(IPixelDisplay _display)
    {
        var screen = new DisplayScreen(_display);
        screen.BackgroundColor = Color.Azure;

        var layout = new StackLayout(0, 0, screen.Width, screen.Height)
        {
            StackOrientation = StackLayout.Orientation.Horizontal
        };
        screen.Controls.Add(layout);

        var font = new Font12x16();


        var label = new Label(0, 0, 100, 50, ScaleFactor.X1)
        {
            Text = "Hello Dock!",
            Font = new Font12x16(),
            TextColor = Color.DarkBlue,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        layout.Controls.Add(label);

        var box = new Box(0, 0, 140, 50)
        {
            ForegroundColor = Color.Orange,
            IsFilled = true
        };
        layout.Controls.Add(box);

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
        layout.Controls.Add(button1);

        var button2 = new Button(0, 0, 100, 50) { Text = "SKIP" };
        button2.ApplyTheme(buttonTheme);
        layout.Controls.Add(button2);

        var button3 = new Button(0, 0, 100, 50) { Text = "NO" };
        button3.ApplyTheme(buttonTheme);
        layout.Controls.Add(button3);
    }

    public Task Run()
    {
        return Task.CompletedTask;
    }
}