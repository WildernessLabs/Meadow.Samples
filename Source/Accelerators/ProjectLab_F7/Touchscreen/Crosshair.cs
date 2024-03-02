using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;

namespace Touchscreen_Demo;

public class Crosshair : ThemedControl
{
    private Color _foreColor = Color.Black;
    private int _lineWidth;

    public Crosshair(int left, int top, int size = 20, int linewidth = 3)
        : base(left, top, size, size)
    {
        _lineWidth = linewidth;
    }

    public override void ApplyTheme(DisplayTheme theme)
    {
        if (theme != null)
        {
            if (theme.ForegroundColor != null) this.ForeColor = theme.ForegroundColor.Value;
        }
    }

    public Color ForeColor
    {
        get => _foreColor;
        set => SetInvalidatingProperty(ref _foreColor, value);
    }

    protected override void OnDraw(MicroGraphics graphics)
    {
        if (ForeColor != Color.Transparent)
        {
            // position is the center of the crosshair
            graphics.DrawRectangle(Left - Width / 2, Top - _lineWidth / 2, Width, _lineWidth, ForeColor, true);
            graphics.DrawRectangle(Left - _lineWidth / 2, Top - Height / 2, _lineWidth, Height, ForeColor, true);
        }
    }
}
