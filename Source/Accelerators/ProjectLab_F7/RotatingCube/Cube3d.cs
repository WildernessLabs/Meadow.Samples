using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Units;
using System;

namespace RotatingCube
{
    public class Cube3d
    {
        public int[,] Wireframe = new int[12, 3];

        private int[,] cubeVertices;

        public Angle XRotation { get; set; }
        public Angle YRotation { get; set; }
        public Angle ZRotation { get; set; }

        public Angle XVelocity
        {
            get => _XVelocity;
            set => _XVelocity = new Angle(Math.Min(value.Degrees, MaxVelocity.Degrees));
        }
        Angle _XVelocity;

        public Angle YVelocity
        {
            get => _YVelocity;
            set => _YVelocity = new Angle(Math.Min(value.Degrees, MaxVelocity.Degrees));
        }
        Angle _YVelocity;

        public Angle ZVelocity
        {
            get => _ZVelocity;
            set => _ZVelocity = new Angle(Math.Min(value.Degrees, MaxVelocity.Degrees));
        }
        Angle _ZVelocity;

        public Angle MaxVelocity { get; } = new Angle(30);

        private double rotationX, rotationY, rotationZ;
        private double rotationXX, rotationYY, rotationZZ;
        private double rotationXXX, rotationYYY, rotationZZZ;

        private readonly int originX;
        private readonly int originY;

        public Cube3d(int xCenter, int yCenter, int cubeSize = 60)
        {
            InitVertices(cubeSize);

            originX = xCenter;
            originY = yCenter;
        }

        void InitVertices(int cubeSize = 100)
        {
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
        }

        public void Update()
        {
            XRotation += XVelocity;
            YRotation += YVelocity;
            ZRotation += ZVelocity;

            for (int i = 0; i < 8; i++)
            {
                //rotateY
                rotationZ = cubeVertices[i, 2] * Math.Cos(YRotation.Radians) - cubeVertices[i, 0] * Math.Sin(YRotation.Radians);
                rotationX = cubeVertices[i, 2] * Math.Sin(YRotation.Radians) + cubeVertices[i, 0] * Math.Cos(YRotation.Radians);
                rotationY = cubeVertices[i, 1];

                //rotateX
                rotationYY = rotationY * Math.Cos(XRotation.Radians) - rotationZ * Math.Sin(XRotation.Radians);
                rotationZZ = rotationY * Math.Sin(XRotation.Radians) + rotationZ * Math.Cos(XRotation.Radians);
                rotationXX = rotationX;

                //rotateZ
                rotationXXX = rotationXX * Math.Cos(ZRotation.Radians) - rotationYY * Math.Sin(ZRotation.Radians);
                rotationYYY = rotationXX * Math.Sin(ZRotation.Radians) + rotationYY * Math.Cos(ZRotation.Radians);
                rotationZZZ = rotationZZ;

                //orthographic projection
                rotationXXX += originX;
                rotationYYY += originY;

                //store new vertices values for wireframe drawing
                Wireframe[i, 0] = (int)rotationXXX;
                Wireframe[i, 1] = (int)rotationYYY;
                Wireframe[i, 2] = (int)rotationZZZ;
            }
        }

        void DrawWireframe(MicroGraphics graphics, Color color)
        {
            graphics.DrawLine(Wireframe[0, 0], Wireframe[0, 1], Wireframe[1, 0], Wireframe[1, 1], color);
            graphics.DrawLine(Wireframe[1, 0], Wireframe[1, 1], Wireframe[2, 0], Wireframe[2, 1], color);
            graphics.DrawLine(Wireframe[2, 0], Wireframe[2, 1], Wireframe[3, 0], Wireframe[3, 1], color);
            graphics.DrawLine(Wireframe[3, 0], Wireframe[3, 1], Wireframe[0, 0], Wireframe[0, 1], color);

            //cross face above
            graphics.DrawLine(Wireframe[1, 0], Wireframe[1, 1], Wireframe[3, 0], Wireframe[3, 1], color);
            graphics.DrawLine(Wireframe[0, 0], Wireframe[0, 1], Wireframe[2, 0], Wireframe[2, 1], color);

            graphics.DrawLine(Wireframe[4, 0], Wireframe[4, 1], Wireframe[5, 0], Wireframe[5, 1], color);
            graphics.DrawLine(Wireframe[5, 0], Wireframe[5, 1], Wireframe[6, 0], Wireframe[6, 1], color);
            graphics.DrawLine(Wireframe[6, 0], Wireframe[6, 1], Wireframe[7, 0], Wireframe[7, 1], color);
            graphics.DrawLine(Wireframe[7, 0], Wireframe[7, 1], Wireframe[4, 0], Wireframe[4, 1], color);

            graphics.DrawLine(Wireframe[0, 0], Wireframe[0, 1], Wireframe[4, 0], Wireframe[4, 1], color);
            graphics.DrawLine(Wireframe[1, 0], Wireframe[1, 1], Wireframe[5, 0], Wireframe[5, 1], color);
            graphics.DrawLine(Wireframe[2, 0], Wireframe[2, 1], Wireframe[6, 0], Wireframe[6, 1], color);
            graphics.DrawLine(Wireframe[3, 0], Wireframe[3, 1], Wireframe[7, 0], Wireframe[7, 1], color);
        }
    }
}