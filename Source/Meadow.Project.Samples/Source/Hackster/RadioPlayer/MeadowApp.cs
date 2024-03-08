using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio.Radio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Peripherals.Displays;
using Meadow.Units;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RadioPlayer
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        List<Frequency> stations;
        int currentStation = 0;

        Tea5767 radio;
        MicroGraphics graphics;
        PushButton btnNext;
        PushButton btnPrevious;

        public override Task Initialize()
        {
            var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            var i2CBus = Device.CreateI2cBus();

            radio = new Tea5767(i2CBus);

            var display = new Ssd1306(i2CBus, 60, Ssd1306.DisplayType.OLED128x32);
            graphics = new MicroGraphics(display);
            graphics.Rotation = RotationType._180Degrees;

            btnNext = new PushButton(Device.Pins.D03);
            btnNext.Clicked += BtnNextClicked;

            btnPrevious = new PushButton(Device.Pins.D04);
            btnPrevious.Clicked += BtnPreviousClicked;

            stations = new List<Frequency>();
            stations.Add(new Frequency(94.5f, Frequency.UnitType.Megahertz));
            stations.Add(new Frequency(95.3f, Frequency.UnitType.Megahertz));
            stations.Add(new Frequency(96.9f, Frequency.UnitType.Megahertz));
            stations.Add(new Frequency(102.7f, Frequency.UnitType.Megahertz));
            stations.Add(new Frequency(103.5f, Frequency.UnitType.Megahertz));
            stations.Add(new Frequency(104.3f, Frequency.UnitType.Megahertz));
            stations.Add(new Frequency(105.7f, Frequency.UnitType.Megahertz));

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        void BtnNextClicked(object sender, EventArgs e)
        {
            if (currentStation < stations.Count - 1)
            {
                DisplayText("      >>>>      ", 0);
                currentStation++;
                radio.SelectFrequency(stations[currentStation]);
                DisplayText($"<- FM {stations[currentStation].Megahertz} ->");
            }
        }

        void BtnPreviousClicked(object sender, EventArgs e)
        {
            if (currentStation > 0)
            {
                DisplayText("      <<<<      ", 0);
                currentStation--;
                radio.SelectFrequency(stations[currentStation]);
                DisplayText($"<- FM {stations[currentStation].Megahertz} ->");
            }
        }

        void DisplayText(string text, int x = 12)
        {
            graphics.Clear();
            graphics.CurrentFont = new Font8x12();
            graphics.DrawRectangle(0, 0, 128, 32);
            graphics.DrawText(x, 12, text);
            graphics.Show();
        }

        public override async Task Run()
        {
            DisplayText("Radio Player");
            await Task.Delay(1000);
            radio.SelectFrequency(stations[currentStation]);
            DisplayText($"<- FM {stations[currentStation].Megahertz} ->");
        }
    }
}