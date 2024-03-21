using Meadow;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Peripherals.Displays;

namespace Froggit;

public partial class FrogItGame
{
    public static Color FrogColor = new Color(57, 255, 20);
    public static Color TruckColor = Color.FromHex("#93AF9E");
    public static Color LogColor = Color.FromHex("#8b6914");
    public static Color CarColor = Color.FromHex("#00E6FF");
    public static Color WaterColor = Color.FromHex("#00E6FF");
    public static Color SidewalkColor = Color.FromHex("#404A3C");

    IPixelBuffer frogUp, frogRight, frogLeft;
    IPixelBuffer logDarkLeft, logDarkRight, logDarkCenter;
    IPixelBuffer crocLeft, crocCenter, crocRight;
    IPixelBuffer truckLeft, truckCenter, truckRight;
    IPixelBuffer carLeft, carRight;

    void InitBuffers()
    {
        byte[] frogU = { 0x99, 0xbd, 0x5a, 0x7e, 0x7e, 0x3c, 0x66, 0xc3 };
        byte[] frogL = { 0xe3, 0x3a, 0x5e, 0xfc, 0xfc, 0x5e, 0x3a, 0xe3 };
        byte[] frogR = { 0xc7, 0x5c, 0x7a, 0x3f, 0x3f, 0x7a, 0x5c, 0xc7 };

        frogUp = LoadSprite(frogU, color: FrogColor);
        frogLeft = LoadSprite(frogL, color: FrogColor);
        frogRight = LoadSprite(frogR, color: FrogColor);

        byte[] logDarkL = { 0x3f, 0x66, 0xff, 0xcf, 0xf6, 0xdf, 0x63, 0x3f }; //log left
        byte[] logDarkC = { 0xff, 0x36, 0xff, 0xe5, 0x3e, 0xff, 0x28, 0xff };
        byte[] logDarkR = { 0xfc, 0x22, 0xdd, 0xd5, 0x55, 0xdd, 0xa2, 0xfc };

        logDarkLeft = LoadSprite(logDarkL, color: LogColor);
        logDarkCenter = LoadSprite(logDarkC, color: LogColor);
        logDarkRight = LoadSprite(logDarkR, color: LogColor);

        byte[] crocL = { 0x3f, 0x88, 0xc2, 0xe0, 0xf0, 0xa0, 0x01, 0xff }; //log left
        byte[] crocC = { 0xff, 0x83, 0x00, 0x00, 0x00, 0x00, 0x9c, 0x31 };
        byte[] crocR = { 0xff, 0xff, 0xff, 0x0f, 0x07, 0x01, 0x70, 0xff };

        crocLeft = LoadSprite(crocL, color: FrogColor);
        crocCenter = LoadSprite(crocC, color: FrogColor);
        crocRight = LoadSprite(crocR, color: FrogColor);

        byte[] truckL = { 0x7f, 0x40, 0x5f, 0x40, 0x5f, 0x40, 0x7f, 0x00 }; //log left
        byte[] truckC = { 0xff, 0x00, 0xff, 0x00, 0xff, 0x00, 0xff, 0x00 };
        byte[] truckR = { 0xdc, 0x7e, 0x59, 0x59, 0x59, 0x7e, 0xdc, 0x00 };

        truckLeft = LoadSprite(truckL, color: TruckColor);
        truckCenter = LoadSprite(truckC, color: TruckColor);
        truckRight = LoadSprite(truckR, color: TruckColor);

        byte[] carL = { 0x1c, 0x3f, 0x4c, 0x4c, 0x4c, 0x3f, 0x1c, 0x00 };
        byte[] carR = { 0x1c, 0xfe, 0x71, 0x71, 0x71, 0xfe, 0x1c, 0x00 };

        carLeft = LoadSprite(carL, color: CarColor);
        carRight = LoadSprite(carR, color: CarColor);
    }

    IPixelBuffer LoadSprite(byte[] data, Color color, int width = 8, int height = 8)
    {
        var buf = new Buffer1bppColor(width, height, data)
           .RotateAndConvert<Buffer1bppColor>(RotationType._90Degrees)
           .ScaleUp<Buffer1bppColor>(2);

        buf.ColorOn = color;
        return buf.ConvertPixelBuffer<BufferRgb444>();
    }
}