using System;
using Meadow;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Logging;
using Meadow.Units;

namespace RainWaterMonitor
{
    public class DisplayController
    {
        protected Logger? Logger { get; } = Resolver.Log;
        readonly MicroGraphics canvas;

        public (Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance)? AtmosphericConditions
        {
            get => atmosphericConditions;
            set
            {
                atmosphericConditions = value;
                Update();
            }
        }
        (Temperature? Temperature, RelativeHumidity? Humidity, Pressure? Pressure, Resistance? GasResistance)? atmosphericConditions;

        /// <summary>
        /// 
        /// </summary>
        public double ContainerFillPercentage {
            get => containerFillPercentage;
            set {
                containerFillPercentage = value;
                Update();
            }
        } protected double containerFillPercentage = 0;

        bool isUpdating = false;
        bool needsUpdate = false;
        bool isV1 = false;

        public DisplayController(IGraphicsDisplay display)
        {
            canvas = new MicroGraphics(display)
            {
                Rotation = RotationType._90Degrees,
                CurrentFont = new Font12x20()
            };

            canvas.Clear(true);
        }

        public void Update()
        {
            Logger?.Info("DisplayController.Update()");

            if (isUpdating)
            {   //queue up the next update
                needsUpdate = true;
                return;
            }

            isUpdating = true;

            canvas.Clear();
            Draw();
            canvas.Show();

            isUpdating = false;

            if (needsUpdate)
            {
                needsUpdate = false;
                Update();
            }
        }

        void DrawStatus(string label, string value, Color color, int yPosition)
        {
            canvas.DrawText(x: 2, y: yPosition, label, color: color);
            canvas.DrawText(x: 238, y: yPosition, value, alignmentH: HorizontalAlignment.Right, color: color);
        }

        void Draw()
        {
            try {

                DrawTemp();
                DrawBarGraph(138, 2, 100, 220, (int)ContainerFillPercentage);
            }
            catch (Exception e) {
                Logger?.Info($"Draw crasher: {e.Message}");
            }
        }

        protected void DrawTemp()
        {
            if (AtmosphericConditions is { } conditions)
            {
                // draw temp canadienne°
                canvas.DrawText(2, 2, $"{conditions.Temperature?.Celsius:n0}°C", WildernessLabsColors.AzureBlue, ScaleFactor.X2, alignmentH: HorizontalAlignment.Left);
                // draw temp TeamAmerica°
                canvas.DrawText(2, 42, $"{conditions.Temperature?.Fahrenheit:n0}°F", WildernessLabsColors.AzureBlue, ScaleFactor.X2, alignmentH: HorizontalAlignment.Left);
            }
        }

        protected void DrawBarGraph(int x, int y, int width, int height, int fillPercent)
        {
            // draw the background of the graph
            Color color = Color.FromHex("004B6B");
            canvas.Stroke = 1;
            canvas.DrawRectangle(x, y, width, height, color, true);

            // calculate drawing bits
            int percentageGraph = (int)(fillPercent == 100 ? 9 : fillPercent / 10);
            int barSpacing = 1;
            int barHeight = (int)Math.Round((float)height / (float)10, 0) - barSpacing;

            //Logger?.Info($"percentageGraph: {percentageGraph}");

            // draw a bar for every 10% of fill
            for (int i = 0; i <= percentageGraph; i++)
            {
                // set the color based on fill percentage
                switch (i)
                {
                    case 0:
                    case 1:
                        color = Color.FromHex("FF3535");
                        break;
                    case 2:
                    case 3:
                    case 4:
                        color = Color.FromHex("FF8251");
                        break;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        color = Color.FromHex("35FF3D");
                        break;
                    case 9:
                        color = Color.FromHex("475AFF");
                        break;
                }
                // calculate the bar Y
                // barY = 220 - (22 * i)
                // (if i == 0) => barY = 220
                // (if i == 9) => barY = 2
                var barY = (height - ((barHeight + barSpacing) * i)) - barHeight;
                //Logger?.Info($"bar {i}; y = {barY}");
                // draw a bar
                canvas.DrawRectangle(x, barY, width, barHeight, color, true);
            }
        }
    }
}