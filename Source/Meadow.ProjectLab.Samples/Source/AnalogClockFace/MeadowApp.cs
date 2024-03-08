using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace AnalogClockFace
{
    // Change F7CoreComputeV2 to F7FeatherV2 for ProjectLab v2
    public class MeadowApp : App<F7CoreComputeV2>
    {
        readonly Color WatchBackgroundColor = Color.White;

        MicroGraphics graphics;
        IProjectLabHardware projectLab;
        int tick;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            projectLab = ProjectLab.Create();
            Resolver.Log.Info($"Running on ProjectLab Hardware {projectLab.RevisionString}");

            projectLab.RgbLed.SetColor(Color.Red);

            graphics = new MicroGraphics(projectLab.Display)
            {
                IgnoreOutOfBoundsPixels = true,
                CurrentFont = new Font12x20(),
                Stroke = 3
            };

            projectLab.RgbLed.SetColor(Color.Green);

            return Task.CompletedTask;
        }

        void DrawWatchFace()
        {
            graphics.Clear();
            int hour = 12;
            int xCenter = graphics.Width / 2;
            int yCenter = graphics.Height / 2;
            int x, y;

            graphics.DrawRectangle(0, 0, graphics.Width, graphics.Height, Color.White);
            graphics.DrawRectangle(5, 5, graphics.Width - 10, graphics.Height - 10, Color.White);

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
            int xCenter = graphics.Width / 2;
            int yCenter = graphics.Height / 2;

            int TimeZoneOffSet = -8; // PST
            var today = DateTime.Now.AddHours(TimeZoneOffSet);
            int minute = today.Minute;
            int hour = today.Hour > 12 ? today.Hour - 12 : today.Hour;

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

        public override async Task Run()
        {
            graphics.Clear(true);

            DrawWatchFace();
            while (true)
            {
                tick++;
                await Task.Delay(1000);
                UpdateClock(second: tick % 60);
            }
        }
    }
}