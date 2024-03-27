using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using System;
using System.Threading.Tasks;

namespace Starfield
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7CoreComputeV2>
    {
        IJuegoHardware juego;
        MicroGraphics graphics;

        private const int NumberOfStars = 128;

        private Random random;

        private byte[] starsX;
        private byte[] starsY;
        private byte[] starsZ;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize...");

            juego = Juego.Create();

            graphics = new MicroGraphics(juego.Display);

            random = new Random();

            starsX = new byte[NumberOfStars];
            starsY = new byte[NumberOfStars];
            starsZ = new byte[NumberOfStars];

            for (int i = 0; i < NumberOfStars; i++)
            {
                starsX[i] = (byte)(graphics.Width / 2 - graphics.Height / 2 + random.Next(256));
                starsY[i] = (byte)random.Next(256);
                starsZ[i] = (byte)(255 - i);
            }

            Console.WriteLine("Initialize complete");

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            Console.WriteLine("Run...");

            graphics.Clear();

            int halfWidth = graphics.Width / 2;
            int halfHeight = graphics.Height / 2;

            while (true)
            {
                for (int i = 0; i < NumberOfStars; i++)
                {
                    if (starsZ[i] <= 1)
                    {
                        starsX[i] = (byte)(halfWidth - halfHeight + random.Next(256));
                        starsY[i] = (byte)random.Next(256);
                        starsZ[i] = (byte)(255 - i);
                    }
                    else
                    {
                        int oldPosX = (starsX[i] - halfWidth) * 256 / starsZ[i] + halfWidth;
                        int oldPosY = (starsY[i] - halfHeight) * 256 / starsZ[i] + halfHeight;

                        graphics.DrawPixel(oldPosX, oldPosY, Color.Black);

                        starsZ[i] -= 2;
                        if (starsZ[i] > 1)
                        {
                            int posX = (starsX[i] - halfWidth) * 256 / starsZ[i] + halfWidth;
                            int posY = (starsY[i] - halfHeight) * 256 / starsZ[i] + halfHeight;

                            if (posX >= 0 && posY >= 0 &&
                                posX < graphics.Width && posY < graphics.Height)
                            {
                                byte intensity = (byte)(255 - starsZ[i]);
                                graphics.DrawPixel(posX, posY, Color.FromRgb(intensity, intensity, intensity));
                            }
                            else
                            {
                                starsZ[i] = 0;
                            }
                        }
                    }
                }

                graphics.Show();
            }
        }
    }
}