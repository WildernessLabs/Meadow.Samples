using Meadow;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Speakers;
using System;

namespace MorseCodeTrainer.Controllers
{
    public class DisplayController
    {
        private static readonly Lazy<DisplayController> instance =
            new Lazy<DisplayController>(() => new DisplayController());
        public static DisplayController Instance => instance.Value;

        MicroGraphics graphics;
        MicroAudio audio;
        readonly int padding = 12;

        private DisplayController() { }

        public void Initialize(IPixelDisplay display, IToneGenerator piezoSpeaker)
        {
            audio = new MicroAudio(piezoSpeaker);

            graphics = new MicroGraphics(display)
            {
                Stroke = 1,
                CurrentFont = new Font12x20(),
            };

            graphics.Clear();
            graphics.DrawRectangle(0, 0, graphics.Width, graphics.Height - 1);
            graphics.DrawText(graphics.Width / 2, 15, "Morse Code Coach", alignmentH: HorizontalAlignment.Center);
            graphics.DrawHorizontalLine((graphics.Width - 196) / 2, 41, 196, Color.White);
            graphics.Show();
        }

        public void DrawCorrectIncorrectMessage(string question, string answer, bool isCorrect)
        {
            Color color = isCorrect ? Color.GreenYellow : Color.Red;

            _ = isCorrect
                ? audio.PlayGameSound(GameSoundEffect.LevelComplete)
                : audio.PlayGameSound(GameSoundEffect.Explosion);

            graphics.DrawText(graphics.Width / 2, 65, question, color, ScaleFactor.X3, alignmentH: HorizontalAlignment.Center);
            UpdateAnswer(answer, color);
            graphics.DrawText(graphics.Width / 2, 190, isCorrect ? "Correct!" : "Try again!", color, ScaleFactor.X1, alignmentH: HorizontalAlignment.Center);
            graphics.Show();
        }

        public void ShowLetterQuestion(string question)
        {
            graphics.DrawRectangle(padding, 60, graphics.Width - padding * 2, 60, Color.Black, true);
            graphics.DrawText(graphics.Width / 2, 65, question, Color.White, ScaleFactor.X3, alignmentH: HorizontalAlignment.Center);
            graphics.DrawRectangle(5, 120, graphics.Width - padding * 2, graphics.Height / 2 - padding, Color.Black, true);
            graphics.Show();
        }

        public void UpdateAnswer(string answer, Color color)
        {
            int symbolSize = 20;
            int symbolSpacing = 8;

            int x = (graphics.Width - ((symbolSize + symbolSpacing) * (answer.Length - 1))) / 2;
            int y = 155;

            graphics.DrawRectangle(padding, y - (symbolSize + symbolSpacing) / 2, graphics.Width - padding * 2, 30, Color.Black, true);

            foreach (var ch in answer)
            {
                DrawDashOrDot(x, y, ch == '-', color);
                x += symbolSize + symbolSpacing;
            }

            graphics.Show();
        }

        void DrawDashOrDot(int x, int y, bool isDash, Color color)
        {
            if (isDash)
            {
                graphics.DrawRectangle(x - 10, y - 3, 20, 8, color, true);
            }
            else
            {
                graphics.DrawCircle(x, y, 10, color, true);
            }

            graphics.Show();
        }
    }
}