using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Ssd130x;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using System;
using System.Threading;

namespace Mp3Player
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Ssd1306 display;
        GraphicsLibrary graphics;
        PushButton btnNext;
        PushButton btnPrevious;
        PushButton btnPlayPause;

        bool isPlaying;

        public MeadowApp()
        {
            InitializePeripherals();            
        }

        void InitializePeripherals()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            var i2CBus = Device.CreateI2cBus();

            display = new Ssd1306(i2CBus, 60, Ssd1306.DisplayType.OLED128x32);
            graphics = new GraphicsLibrary(display);
            graphics.Rotation = RotationType._180Degrees;
            DisplayText("MUSIC PLAYER", 16);
            Thread.Sleep(1000);
            DisplayText("Song.mp3");

            btnNext = new PushButton(Device, Device.Pins.D02, ResistorMode.InternalPullUp);
            btnNext.Clicked += BtnNextClicked;

            btnPrevious = new PushButton(Device, Device.Pins.D03, ResistorMode.InternalPullUp);
            btnPrevious.Clicked += BtnPreviousClicked;

            btnPlayPause = new PushButton(Device, Device.Pins.D04, ResistorMode.InternalPullUp);
            btnPlayPause.Clicked += BtnPlayPauseClicked;

            led.SetColor(RgbLed.Colors.Green);
        }

        private void BtnPlayPauseClicked(object sender, System.EventArgs e)
        {
            isPlaying = !isPlaying;
            DisplayText(isPlaying ? "PLAY" : "STOP");
            Thread.Sleep(1000);
            DisplayText("Song.mp3");

            Console.WriteLine("Play button");
        }

        private void BtnPreviousClicked(object sender, System.EventArgs e)
        {
            DisplayText("Previous song");
            Thread.Sleep(1000);
            DisplayText("Song.mp3");
        }

        private void BtnNextClicked(object sender, System.EventArgs e)
        {
            DisplayText("Next song");
            Thread.Sleep(1000);
            DisplayText("Song.mp3");
        }

        void DisplayText(string text, int x = 12)
        {
            graphics.Clear();
            graphics.CurrentFont = new Font8x12();
            graphics.DrawRectangle(0, 0, 128, 32);
            graphics.DrawText(x, 12, text);
            graphics.Show();
        }
    }
}
