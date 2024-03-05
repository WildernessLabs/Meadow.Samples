namespace Eyeball.Core;

public partial class EyeballController
{
    public enum EyeColors
    {
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Count
    }

    public enum EyeMovement
    {
        LookLeft,
        LookRight,
        LookUp,
        LookDown,
        Blink,
        Dilate,
        RetinaFade,
        Count
    }
}
