using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Peripherals.Displays;
using System;
using System.Threading;

namespace Eyeball.Core;

public partial class EyeballController
{
    readonly MicroGraphics graphics;

    IPixelBuffer? eyeballBuffer;

    Color EyeOutlineColor = Color.FromHex("241E25");
    Color EyeOuterRedColor = Color.FromHex("F27274");
    Color EyeOuterPinkColor = Color.FromHex("F9C0C5");
    Color EyeWhiteColor = Color.FromHex("FFEADD");

    readonly CoronaColor[] CoronaColors = new CoronaColor[]
    {
        new CoronaColor { CoronaLight = Color.Red, CoronaDark = Color.DarkRed },
        new CoronaColor { CoronaLight = Color.FromHex("55B835"), CoronaDark = Color.FromHex("367D17") },
        new CoronaColor { CoronaLight = Color.Cyan, CoronaDark = Color.DarkCyan },
        new CoronaColor { CoronaLight = Color.Yellow, CoronaDark = Color.DarkGoldenrod },
        new CoronaColor { CoronaLight = Color.Orange, CoronaDark = Color.DarkOrange },
    };

    CoronaColor currentColor;

    int xOffset = 0;
    int yOffset = 0;

    int xLast = -1;
    int yLast = -1;

    readonly int MovementStep = 4;
    readonly int ReturnStep = 12;

    readonly int MinEyeMovement = 24;
    readonly int MaxEyeMovement = 50;

    readonly int FadeSteps = 12;

    readonly Random random = new();

    public EyeballController(IPixelDisplay display)
    {
        graphics = new MicroGraphics(display);
        currentColor = CoronaColors[0];

        InitializeEyeballBuffer();
    }

    void InitializeEyeballBuffer()
    {
        if (graphics.ColorMode == ColorMode.Format12bppRgb444)
        {
            eyeballBuffer = new BufferRgb444(240, 240);
        }
        else if (graphics.ColorMode == ColorMode.Format16bppRgb565)
        {
            eyeballBuffer = new BufferRgb565(240, 240);
        }
        else if (graphics.ColorMode == ColorMode.Format24bppRgb888)
        {
            eyeballBuffer = new BufferRgb888(240, 240);
        }

        var eyeballGraphics = new MicroGraphics(eyeballBuffer, false);

        eyeballGraphics.Clear();

        eyeballGraphics.DrawCircle(eyeballGraphics.Width / 2, eyeballGraphics.Height / 2, 120, EyeOutlineColor, true, true);
        eyeballGraphics.DrawCircle(eyeballGraphics.Width / 2, eyeballGraphics.Height / 2, 112, EyeOuterRedColor, true, true);
        eyeballGraphics.DrawCircle(eyeballGraphics.Width / 2, eyeballGraphics.Height / 2, 104, EyeOuterPinkColor, true, true);
        eyeballGraphics.DrawCircle(eyeballGraphics.Width / 2, eyeballGraphics.Height / 2, 96, EyeWhiteColor, true, true);
    }

    public void Delay()
    {
        Thread.Sleep(random.Next(500, 2000));
    }

    public void RandomEyeMovement()
    {
        EyeMovement movementType = (EyeMovement)random.Next((int)EyeMovement.Count);

        switch (movementType)
        {
            case EyeMovement.LookLeft:
                LookLeft();
                Delay();
                CenterEye();
                break;
            case EyeMovement.LookRight:
                LookRight();
                Delay();
                CenterEye();
                break;
            case EyeMovement.LookUp:
                LookUp();
                Delay();
                CenterEye();
                break;
            case EyeMovement.LookDown:
                LookDown();
                Delay();
                CenterEye();
                break;
            case EyeMovement.Blink:
                Blink();
                break;
            case EyeMovement.Dilate:
                Dilate();
                break;
            case EyeMovement.RetinaFade:
                FadeRetina();
                break;
        }
    }

    void ResetEye()
    {
        xOffset = 0;
        yOffset = 0;
        DrawEyeball();
    }

    void Blink()
    {
        DrawEyeball();
        graphics.DrawCircle(graphics.Width / 2, graphics.Height / 2, 120, EyeOutlineColor, true, true);
        graphics.Show();
        Thread.Sleep(100);

        xLast = -1;
        DrawEyeball();

        Thread.Sleep(100);
        graphics.DrawCircle(graphics.Width / 2, graphics.Height / 2, 120, EyeOutlineColor, true, true);
        graphics.Show();
        Thread.Sleep(100);

        xLast = -1;
        DrawEyeball();
    }

    void FadeRetina()
    {
        double step = 1 / (double)FadeSteps;

        for (int i = 0; i < FadeSteps; i++)
        {
            DrawRetinaWithFade(i * step);
            graphics.Show();
            Thread.Sleep(100);
        }

        Delay();

        var oldColor = currentColor;

        while (oldColor.CoronaLight == currentColor.CoronaLight)
        {
            currentColor = CoronaColors[random.Next(CoronaColors.Length)];
        }

        for (int i = 0; i < FadeSteps; i++)
        {
            DrawRetinaWithFade(1 - i * step);
            graphics.Show();
            Thread.Sleep(100);
        }
    }

    void Dilate()
    {
        DrawEyeball();

        int dilationAmount = 10;

        for (int i = -2; i < dilationAmount; i++)
        {
            graphics.DrawCircle(xLast, yLast, 16 + i, Color.Black, true, true);
            graphics.Show();
        }

        Thread.Sleep(1000);

        for (int i = dilationAmount; i > 0; i--)
        {
            graphics.DrawCircle(xLast, yLast, 32, currentColor.CoronaLight, true, true);
            graphics.DrawCircle(xLast, yLast, 16 + i, Color.Black, true, true);
            graphics.Show();
        }

        DrawEyeball();
    }

    int GetRandomMovementAmount()
    {
        return random.Next(MinEyeMovement, MaxEyeMovement);
    }

    void MoveEye(int xDirection, int yDirection)
    {
        ResetEye();
        var movement = GetRandomMovementAmount();
        for (int i = 0; i < movement; i += MovementStep)
        {
            xOffset = xDirection * i;
            yOffset = yDirection * i;
            DrawEyeball();
        }
    }

    void LookLeft()
    {
        MoveEye(-1, 0);
    }

    void LookRight()
    {
        MoveEye(1, 0);
    }

    void LookUp()
    {
        MoveEye(0, -1);
    }

    void LookDown()
    {
        MoveEye(0, 1);
    }

    public void CenterEye()
    {
        while (xOffset != 0 || yOffset != 0)
        {
            if (xOffset > 0)
            {
                xOffset -= ReturnStep;
                if (xOffset < 0)
                    xOffset = 0;
            }
            else if (xOffset < 0)
            {
                xOffset += ReturnStep;
                if (xOffset > 0)
                    xOffset = 0;
            }

            if (yOffset > 0)
            {
                yOffset -= ReturnStep;
                if (yOffset < 0)
                    yOffset = 0;
            }
            else if (yOffset < 0)
            {
                yOffset += ReturnStep;
                if (yOffset > 0)
                    yOffset = 0;
            }

            DrawEyeball();
        }
    }

    public void DrawEyeball()
    {
        if (xLast == -1)
        {
            graphics.DrawBuffer(graphics.Width / 2 - eyeballBuffer.Width / 2,
            graphics.Height / 2 - eyeballBuffer.Height / 2, eyeballBuffer);
        }
        else
        {
            graphics.DrawRectangle(xLast - 40, yLast - 40, 80, 80, EyeWhiteColor, true);
        }

        xLast = graphics.Width / 2 + xOffset;
        yLast = graphics.Height / 2 + yOffset;

        graphics.DrawCircle(xLast, yLast, 40, currentColor.CoronaDark, true, true);
        graphics.DrawCircle(xLast, yLast, 32, currentColor.CoronaLight, true, true);

        graphics.DrawCircle(xLast + (xOffset >> 2), yLast + (yOffset >> 2), 16, Color.Black, true, true);

        graphics.Show();
    }

    void DrawRetinaWithFade(double ratio)
    {
        graphics.DrawCircle(xLast, yLast, 40, currentColor.CoronaDark.Blend(EyeWhiteColor, ratio), true, true);
        graphics.DrawCircle(xLast, yLast, 32, currentColor.CoronaLight.Blend(EyeWhiteColor, ratio), true, true);
        graphics.DrawCircle(xLast, yLast, 16, Color.Black.Blend(EyeWhiteColor, ratio), true, true);
    }
}