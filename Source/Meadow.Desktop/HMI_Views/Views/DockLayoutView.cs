using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System.Threading.Tasks;

namespace HMI_Views.Views;

class DockLayoutView
{
    public DockLayoutView(IPixelDisplay _display)
    {
        var screen = new DisplayScreen(_display);
        screen.BackgroundColor = Color.Azure;

        var layout = new DockLayout(0, 0, screen.Width, screen.Height);
        screen.Controls.Add(layout);

        var font = new Font12x16();


        var label = new Label(0, 0, 100, 50, Meadow.Foundation.Graphics.ScaleFactor.X1)
        {
            Text = "Hello Dock!",
            Font = new Font12x16(),
            TextColor = Color.DarkBlue,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        layout.Add(label, DockLayout.DockPosition.Top);

        var box = new Box(0, 0, 140, 50)
        {
            ForegroundColor = Color.Orange,
            IsFilled = true
        };
        layout.Add(box, DockLayout.DockPosition.Center);

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
        layout.Add(button1, DockLayout.DockPosition.BottomLeft);

        var button2 = new Button(0, 0, 100, 50) { Text = "SKIP" };
        button2.ApplyTheme(buttonTheme);
        layout.Add(button2, DockLayout.DockPosition.Bottom);

        var button3 = new Button(0, 0, 100, 50) { Text = "NO" };
        button3.ApplyTheme(buttonTheme);
        layout.Add(button3, DockLayout.DockPosition.BottomRight);
    }

    public Task Run()
    {
        return Task.CompletedTask;
    }
}
