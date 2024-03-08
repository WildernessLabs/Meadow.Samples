using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using System;

namespace MagicEightMeadow.Controllers
{
    internal class DisplayController
    {
        private readonly Random random;

        private readonly Image simonQuestion;

        private readonly DisplayScreen displayScreen;

        private readonly Picture simonImage;

        Color backgroundColor = Color.FromHex("#00000C");

        public DisplayController(IPixelDisplay display)
        {
            random = new Random();

            displayScreen = new DisplayScreen(display, RotationType._270Degrees)
            {
                BackgroundColor = backgroundColor
            };

            simonQuestion = Image.LoadFromResource("MagicEightMeadow.Resources.m8b_question.bmp");
            simonImage = new Picture((displayScreen.Width - displayScreen.Width) / 2, 0, displayScreen.Width, displayScreen.Height, simonQuestion);
            displayScreen.Controls.Add(simonImage);
        }

        public void ShowQuestion()
        {
            simonImage.Image = simonQuestion;
        }

        public void ShowAnswer()
        {
            var simonAnswer = Image.LoadFromResource($"MagicEightMeadow.Resources.m8b_{random.Next(1, 21).ToString("00")}.bmp");
            simonImage.Image = simonAnswer;
        }
    }
}