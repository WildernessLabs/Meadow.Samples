using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Displays;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowClockGraphics
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        readonly Color WatchBackgroundColor = Color.White;

        MicroGraphics graphics;
        int displayWidth, displayHeight;
        int hour, minute, tick;

        public override Task Initialize()
        {
            var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            var st7789 = new St7789
            (
                spiBus: Device.CreateSpiBus(),
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );
            displayWidth = Convert.ToInt32(st7789.Width);
            displayHeight = Convert.ToInt32(st7789.Height);

            graphics = new MicroGraphics(st7789);
            graphics.Rotation = RotationType._270Degrees;

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        void DrawShapes()
        {
            Random rand = new Random();

            graphics.Clear(true);

            int radius = 10;
            int originX = displayWidth / 2;
            int originY = displayHeight / 2;

            for (int i = 1; i < 5; i++)
            {
                graphics.DrawCircle
                (
                    centerX: originX,
                    centerY: originY,
                    radius: radius,
                    color: Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255))
                );
                graphics.Show();
                radius += 30;
            }

            int sideLength = 30;
            for (int i = 1; i < 5; i++)
            {
                graphics.DrawRectangle
                (
                    x: (displayWidth - sideLength) / 2,
                    y: (displayHeight - sideLength) / 2,
                    width: sideLength,
                    height: sideLength,
                    color: Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255))
                );
                graphics.Show();
                sideLength += 60;
            }

            graphics.DrawLine(0, displayHeight / 2, displayWidth, displayHeight / 2,
                Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
            graphics.DrawLine(displayWidth / 2, 0, displayWidth / 2, displayHeight,
                Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
            graphics.DrawLine(0, 0, displayWidth, displayHeight,
                Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
            graphics.DrawLine(0, displayHeight, displayWidth, 0,
                Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
            graphics.Show();

            //Thread.Sleep(5000);
        }

        void DrawTexts()
        {
            graphics.Clear(true);

            int indent = 20;
            int spacing = 20;
            int y = 5;

            graphics.CurrentFont = new Font12x16();
            graphics.DrawText(indent, y, "Meadow F7 SPI ST7789!!");
            graphics.DrawText(indent, y += spacing, "Red", Color.Red);
            graphics.DrawText(indent, y += spacing, "Purple", Color.Purple);
            graphics.DrawText(indent, y += spacing, "BlueViolet", Color.BlueViolet);
            graphics.DrawText(indent, y += spacing, "Blue", Color.Blue);
            graphics.DrawText(indent, y += spacing, "Cyan", Color.Cyan);
            graphics.DrawText(indent, y += spacing, "LawnGreen", Color.LawnGreen);
            graphics.DrawText(indent, y += spacing, "GreenYellow", Color.GreenYellow);
            graphics.DrawText(indent, y += spacing, "Yellow", Color.Yellow);
            graphics.DrawText(indent, y += spacing, "Orange", Color.Orange);
            graphics.DrawText(indent, y += spacing, "Brown", Color.Brown);
            graphics.Show();

            Thread.Sleep(5000);
        }

        void DrawClock()
        {
            graphics.Clear(true);

            hour = 8;
            minute = 54;
            DrawWatchFace();
            while (true)
            {
                tick++;
                Thread.Sleep(1000);
                UpdateClock(second: tick % 60);
            }
        }
        void DrawWatchFace()
        {
            graphics.Clear();
            int hour = 12;
            int xCenter = displayWidth / 2;
            int yCenter = displayHeight / 2;
            int x, y;

            graphics.DrawRectangle(0, 0, displayWidth, displayHeight, Color.White);
            graphics.DrawRectangle(5, 5, displayWidth - 10, displayHeight - 10, Color.White);

            graphics.CurrentFont = new Font12x20();
            graphics.DrawCircle(xCenter, yCenter, 100, WatchBackgroundColor, true);
            for (int i = 0; i < 60; i++)
            {
                x = (int)(xCenter + 80 * Math.Sin(i * Math.PI / 30));
                y = (int)(yCenter - 80 * Math.Cos(i * Math.PI / 30));

                if (i % 5 == 0)
                {
                    graphics.DrawText(hour > 9 ? x - 10 : x - 5, y - 5, hour.ToString(), Color.Black);
                    if (hour == 12) hour = 1; else hour++;
                }
            }

            graphics.Show();
        }
        void UpdateClock(int second = 0)
        {
            int xCenter = displayWidth / 2;
            int yCenter = displayHeight / 2;
            int x, y, xT, yT;

            if (second == 0)
            {
                minute++;
                if (minute == 60)
                {
                    minute = 0;
                    hour++;
                    if (hour == 12)
                    {
                        hour = 0;
                    }
                }
            }

            graphics.Stroke = 3;

            //remove previous hour
            int previousHour = (hour - 1) < -1 ? 11 : (hour - 1);
            x = (int)(xCenter + 43 * Math.Sin(previousHour * Math.PI / 6));
            y = (int)(yCenter - 43 * Math.Cos(previousHour * Math.PI / 6));
            xT = (int)(xCenter + 3 * Math.Sin((previousHour - 3) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((previousHour - 3) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            xT = (int)(xCenter + 3 * Math.Sin((previousHour + 3) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((previousHour + 3) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            //current hour
            x = (int)(xCenter + 43 * Math.Sin(hour * Math.PI / 6));
            y = (int)(yCenter - 43 * Math.Cos(hour * Math.PI / 6));
            xT = (int)(xCenter + 3 * Math.Sin((hour - 3) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((hour - 3) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);
            xT = (int)(xCenter + 3 * Math.Sin((hour + 3) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((hour + 3) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);
            //remove previous minute
            int previousMinute = minute - 1 < -1 ? 59 : (minute - 1);
            x = (int)(xCenter + 55 * Math.Sin(previousMinute * Math.PI / 30));
            y = (int)(yCenter - 55 * Math.Cos(previousMinute * Math.PI / 30));
            xT = (int)(xCenter + 3 * Math.Sin((previousMinute - 15) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((previousMinute - 15) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            xT = (int)(xCenter + 3 * Math.Sin((previousMinute + 15) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((previousMinute + 15) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, WatchBackgroundColor);
            //current minute
            x = (int)(xCenter + 55 * Math.Sin(minute * Math.PI / 30));
            y = (int)(yCenter - 55 * Math.Cos(minute * Math.PI / 30));
            xT = (int)(xCenter + 3 * Math.Sin((minute - 15) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((minute - 15) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);
            xT = (int)(xCenter + 3 * Math.Sin((minute + 15) * Math.PI / 6));
            yT = (int)(yCenter - 3 * Math.Cos((minute + 15) * Math.PI / 6));
            graphics.DrawLine(xT, yT, x, y, Color.Black);
            //remove previous second
            int previousSecond = second - 1 < -1 ? 59 : (second - 1);
            x = (int)(xCenter + 70 * Math.Sin(previousSecond * Math.PI / 30));
            y = (int)(yCenter - 70 * Math.Cos(previousSecond * Math.PI / 30));
            graphics.DrawLine(xCenter, yCenter, x, y, WatchBackgroundColor);
            //current second
            x = (int)(xCenter + 70 * Math.Sin(second * Math.PI / 30));
            y = (int)(yCenter - 70 * Math.Cos(second * Math.PI / 30));
            graphics.DrawLine(xCenter, yCenter, x, y, Color.Red);
            graphics.Show();
        }

        public override Task Run()
        {
            //DrawShapes();
            //DrawTexts();
            DrawClock();

            return base.Run();
        }
    }
}