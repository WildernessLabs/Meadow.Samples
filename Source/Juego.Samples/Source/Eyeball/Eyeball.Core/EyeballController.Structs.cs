using Meadow;

namespace Eyeball.Core;

public partial class EyeballController
{
    public struct CoronaColor
    {
        public CoronaColor(Color dark, Color light)
        {
            CoronaDark = dark;
            CoronaLight = light;
        }

        public Color CoronaDark { get; set; }
        public Color CoronaLight { get; set; }
    }
}