using Meadow;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System.Threading.Tasks;

namespace HMI_Views.Views;

internal class LayoutChildrenVisibilityView
{
    private readonly ILayout layout;

    public LayoutChildrenVisibilityView(IPixelDisplay _display)
    {
        var screen = new DisplayScreen(_display);
        screen.BackgroundColor = Color.Azure;

        layout = new StackLayout(0, 0, screen.Width, screen.Height);

        var label1 = new Label(screen.Width, 30, "Label 1")
        {
            BackgroundColor = Color.LightGray
        };
        var label2 = new Label(screen.Width, 30, "Label 2")
        {
            TextColor = Color.Red,
            IsVisible = false
        };

        layout.Controls.Add(label1, label2);

        screen.Controls.Add(layout);
    }

    public async Task Run()
    {
        var label1 = (Label)layout.Controls[0];
        var label2 = (Label)layout.Controls[1];

        while (true)
        {
            // Scenario 1: Parent visible, both children visible
            await Task.Delay(2000);
            layout.IsVisible = true;
            label1.IsVisible = true;
            label2.IsVisible = true;
            Resolver.Log.Info("Scenario 1: Parent visible, both children visible");

            // Scenario 2: Parent visible, label1 visible, label2 invisible (independent child visibility)
            await Task.Delay(2000);
            layout.IsVisible = true;
            label1.IsVisible = true;
            label2.IsVisible = false;
            Resolver.Log.Info("Scenario 2: Parent visible, label1 visible, label2 invisible");

            // Scenario 3: Parent invisible - all children should be invisible regardless of their state
            await Task.Delay(2000);
            layout.IsVisible = false;
            label1.IsVisible = true;  // This won't matter - parent overrides
            label2.IsVisible = true;  // This won't matter - parent overrides
            Resolver.Log.Info("Scenario 3: Parent invisible - all children invisible (parent overrides)");

            // Scenario 4: Parent visible, label1 invisible, label2 visible
            await Task.Delay(2000);
            layout.IsVisible = true;
            label1.IsVisible = false;
            label2.IsVisible = true;
            Resolver.Log.Info("Scenario 4: Parent visible, label1 invisible, label2 visible");
        }
    }
}
