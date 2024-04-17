using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using System;

namespace HMI_Views.Views;

public class RotatingCube
{
    Color backgroundColor = Color.FromHex("91E46C");
    Color foregroundColor = Color.FromHex("000000");

    private MicroGraphics _graphics = default!;

    //needs cleanup - quick port from c code
    private double rot, rotationX, rotationY, rotationZ;
    private double rotationXX, rotationYY, rotationZZ;
    private double rotationXXX, rotationYYY, rotationZZZ;

    private int[,] cubeWireframe = new int[12, 3];
    private int[,] cubeVertices;

    public RotatingCube(IPixelDisplay display)
    {
        int cubeSize = 100;

        cubeVertices = new int[8, 3] {
                { -cubeSize, -cubeSize,  cubeSize},
                {  cubeSize, -cubeSize,  cubeSize},
                {  cubeSize,  cubeSize,  cubeSize},
                { -cubeSize,  cubeSize,  cubeSize},
                { -cubeSize, -cubeSize, -cubeSize},
                {  cubeSize, -cubeSize, -cubeSize},
                {  cubeSize,  cubeSize, -cubeSize},
                { -cubeSize,  cubeSize, -cubeSize},
        };

        _graphics = new MicroGraphics(display)
        {
            CurrentFont = new Font12x20(),
            Stroke = 5
        };
    }

    public void Run()
    {
        int originX = (int)_graphics.Width / 2;
        int originY = (int)_graphics.Height / 2;

        int angle = 0;

        ulong frames = 0;
        var start = 0;
        string frameRate = "";

        start = Environment.TickCount;

        while (true)
        {
            _graphics.Clear(backgroundColor);
            _graphics.DrawText(5, 5, frameRate, foregroundColor);

            angle++;
            for (int i = 0; i < 8; i++)
            {
                rot = angle * 0.0174532; //0.0174532 = one degree

                //rotateY
                rotationZ = cubeVertices[i, 2] * Math.Cos(rot) - cubeVertices[i, 0] * Math.Sin(rot);
                rotationX = cubeVertices[i, 2] * Math.Sin(rot) + cubeVertices[i, 0] * Math.Cos(rot);
                rotationY = cubeVertices[i, 1];

                //rotateX
                rotationYY = rotationY * Math.Cos(rot) - rotationZ * Math.Sin(rot);
                rotationZZ = rotationY * Math.Sin(rot) + rotationZ * Math.Cos(rot);
                rotationXX = rotationX;

                //rotateZ
                rotationXXX = rotationXX * Math.Cos(rot) - rotationYY * Math.Sin(rot);
                rotationYYY = rotationXX * Math.Sin(rot) + rotationYY * Math.Cos(rot);
                rotationZZZ = rotationZZ;

                //orthographic projection
                rotationXXX = rotationXXX + originX;
                rotationYYY = rotationYYY + originY;

                //store new vertices values for wireframe drawing
                cubeWireframe[i, 0] = (int)rotationXXX;
                cubeWireframe[i, 1] = (int)rotationYYY;
                cubeWireframe[i, 2] = (int)rotationZZZ;

                DrawVertices();
            }

            DrawWireframe();

            _graphics.Show();

            if (++frames % 1000 == 0)
            {
                var now = Environment.TickCount;
                var et = (now - start) / 1000d;

                frameRate = $"{(1000 / et):0.0}fps";
                start = Environment.TickCount;
            }
        }
    }

    void DrawVertices()
    {
        _graphics.DrawPixel((int)rotationXXX, (int)rotationYYY);
    }

    void DrawWireframe()
    {
        _graphics.DrawLine(cubeWireframe[0, 0], cubeWireframe[0, 1], cubeWireframe[1, 0], cubeWireframe[1, 1], foregroundColor);
        _graphics.DrawLine(cubeWireframe[1, 0], cubeWireframe[1, 1], cubeWireframe[2, 0], cubeWireframe[2, 1], foregroundColor);
        _graphics.DrawLine(cubeWireframe[2, 0], cubeWireframe[2, 1], cubeWireframe[3, 0], cubeWireframe[3, 1], foregroundColor);
        _graphics.DrawLine(cubeWireframe[3, 0], cubeWireframe[3, 1], cubeWireframe[0, 0], cubeWireframe[0, 1], foregroundColor);

        //cross face above
        _graphics.DrawLine(cubeWireframe[1, 0], cubeWireframe[1, 1], cubeWireframe[3, 0], cubeWireframe[3, 1], foregroundColor);
        _graphics.DrawLine(cubeWireframe[0, 0], cubeWireframe[0, 1], cubeWireframe[2, 0], cubeWireframe[2, 1], foregroundColor);

        _graphics.DrawLine(cubeWireframe[4, 0], cubeWireframe[4, 1], cubeWireframe[5, 0], cubeWireframe[5, 1], foregroundColor);
        _graphics.DrawLine(cubeWireframe[5, 0], cubeWireframe[5, 1], cubeWireframe[6, 0], cubeWireframe[6, 1], foregroundColor);
        _graphics.DrawLine(cubeWireframe[6, 0], cubeWireframe[6, 1], cubeWireframe[7, 0], cubeWireframe[7, 1], foregroundColor);
        _graphics.DrawLine(cubeWireframe[7, 0], cubeWireframe[7, 1], cubeWireframe[4, 0], cubeWireframe[4, 1], foregroundColor);

        _graphics.DrawLine(cubeWireframe[0, 0], cubeWireframe[0, 1], cubeWireframe[4, 0], cubeWireframe[4, 1], foregroundColor);
        _graphics.DrawLine(cubeWireframe[1, 0], cubeWireframe[1, 1], cubeWireframe[5, 0], cubeWireframe[5, 1], foregroundColor);
        _graphics.DrawLine(cubeWireframe[2, 0], cubeWireframe[2, 1], cubeWireframe[6, 0], cubeWireframe[6, 1], foregroundColor);
        _graphics.DrawLine(cubeWireframe[3, 0], cubeWireframe[3, 1], cubeWireframe[7, 0], cubeWireframe[7, 1], foregroundColor);
    }
}