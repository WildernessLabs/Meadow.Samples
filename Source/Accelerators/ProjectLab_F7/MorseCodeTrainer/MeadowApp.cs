using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;
using MorseCodeTrainer.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MorseCodeTrainer
{
    // Change F7CoreComputeV2 to F7FeatherV2 for ProjectLab v2
    public class MeadowApp : App<F7CoreComputeV2>
    {
        Dictionary<string, string> morseCode;

        IRgbPwmLed onboardLed;

        Stopwatch stopWatch;
        string answer;
        string question;

        IProjectLabHardware projectLab;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            projectLab = ProjectLab.Create();
            Resolver.Log.Info($"Running on ProjectLab Hardware {projectLab.RevisionString}");

            onboardLed = projectLab.RgbLed;
            onboardLed.SetColor(Color.Red);

            DisplayController.Instance.Initialize(projectLab.Display, projectLab.Speaker);

            projectLab.RightButton.PressStarted += ButtonPressStarted;
            projectLab.RightButton.PressEnded += ButtonPressEnded;

            projectLab.LeftButton.Clicked += BackSpaceButtonClicked;

            projectLab.DownButton.Clicked += SubmitButtonClicked;

            stopWatch = new Stopwatch();

            LoadMorseCode();

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        void LoadMorseCode()
        {
            morseCode = new Dictionary<string, string>
            {
                { "O-", "A" },
                { "-OOO", "B" },
                { "-O-O", "C" },
                { "-OO", "D" },
                { "O", "E" },
                { "OO-O", "F" },
                { "--O", "G" },
                { "OOOO", "H" },
                { "OO", "I" },
                { "O---", "J" },
                { "-O-", "K" },
                { "O-OO", "L" },
                { "--", "M" },
                { "-O", "N" },
                { "---", "O" },
                { "O--O", "P" },
                { "--O-", "Q" },
                { "O-O", "R" },
                { "OOO", "S" },
                { "-", "T" },
                { "OO-", "U" },
                { "OOO-", "V" },
                { "O--", "W" },
                { "-OO-", "X" },
                { "-O--", "Y" },
                { "--OO", "Z" },
                { "-----", "0" },
                { "O----", "1" },
                { "OO---", "2" },
                { "OOO--", "3" },
                { "OOOO-", "4" },
                { "OOOOO", "5" },
                { "-OOOO", "6" },
                { "--OOO", "7" },
                { "---OO", "8" },
                { "----O", "9" }
            };
        }

        private void BackSpaceButtonClicked(object sender, EventArgs e)
        {
            if (answer.Length == 0) { return; }

            answer = answer.Substring(0, answer.Length - 1);
            DisplayController.Instance.UpdateAnswer(answer, Color.White);
        }

        private async void SubmitButtonClicked(object sender, EventArgs e)
        {
            if (answer.Length == 0) { return; }

            bool isCorrect = morseCode[answer] == question;

            DisplayController.Instance.DrawCorrectIncorrectMessage(question, answer, isCorrect);

            await Task.Delay(2000);

            if (isCorrect)
            {
                ShowLetterQuestion();
            }
            else
            {
                answer = string.Empty;
                DisplayController.Instance.ShowLetterQuestion(question);
            }
        }

        async void ButtonPressStarted(object sender, EventArgs e)
        {
            await projectLab.Speaker.PlayTone(new Meadow.Units.Frequency(440), TimeSpan.MaxValue);
            stopWatch.Reset();
            stopWatch.Start();
        }

        void ButtonPressEnded(object sender, EventArgs e)
        {
            projectLab.Speaker.StopTone();
            stopWatch.Stop();

            if (stopWatch.ElapsedMilliseconds < 200)
            {
                answer += "O";
            }
            else
            {
                answer += "-";
            }

            DisplayController.Instance.UpdateAnswer(answer, Color.White);
        }

        void ShowLetterQuestion()
        {
            answer = string.Empty;
            question = morseCode.ElementAt(new Random().Next(0, morseCode.Count)).Value;
            DisplayController.Instance.ShowLetterQuestion(question);
        }

        public override Task Run()
        {
            ShowLetterQuestion();

            return base.Run();
        }
    }
}