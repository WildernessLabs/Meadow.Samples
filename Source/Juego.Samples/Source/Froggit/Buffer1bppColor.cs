using Meadow;
using Meadow.Foundation.Graphics.Buffers;

namespace Froggit;

internal class Buffer1bppColor : Buffer1bpp
{
    public Color ColorOn { get; set; } = Color.White;
    public Color ColorOff { get; set; } = Color.Black;

    public Buffer1bppColor()
    {
    }

    public Buffer1bppColor(int width, int height) : base(width, height)
    {
    }

    public Buffer1bppColor(int width, int height, byte[] buffer) : base(width, height, buffer)
    {
    }

    /// <summary>
    /// Get the pixel color 
    /// </summary>
    /// <param name="x">x location of pixel</param>
    /// <param name="y">y location of pixel</param>
    /// <returns>The pixel color as a Color object - will be black or white only</returns>
    public override Color GetPixel(int x, int y)
    {
        return GetPixelIsEnabled(x, y) ? ColorOn : ColorOff;
    }
}
