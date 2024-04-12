using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace MorseCodeTrainer;

// public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
public class MeadowApp : App<F7FeatherV2>
{
    Dictionary<string, string> morseCode;

    PushButton button;
    PiezoSpeaker piezo;
    DisplayControllers displayController;

    Timer timer;
    Stopwatch stopWatch;
    string answer;
    string question;

    public override Task Initialize()
    {
        var onboardLed = new RgbPwmLed(
            redPwmPin: Device.Pins.OnboardLedRed,
            greenPwmPin: Device.Pins.OnboardLedGreen,
            bluePwmPin: Device.Pins.OnboardLedBlue);
        onboardLed.SetColor(Color.Red);

        displayController = new DisplayControllers();

        piezo = new PiezoSpeaker(Device.Pins.D09);

        button = new PushButton(Device.Pins.D04);
        button.PressStarted += ButtonPressStarted;
        button.PressEnded += ButtonPressEnded;

        stopWatch = new Stopwatch();

        timer = new Timer(2000);
        timer.Elapsed += TimerElapsed;

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

    async void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        if (!morseCode.ContainsKey(answer)) { return; }

        timer.Stop();

        bool isCorrect = morseCode[answer] == question;

        displayController.DrawCorrectIncorrectMessage(question, answer, isCorrect);

        await Task.Delay(2000);

        if (isCorrect)
        {
            ShowLetterQuestion();
        }
        else
        {
            answer = string.Empty;
            displayController.ShowLetterQuestion(question);
        }

        timer.Start();
    }

    void ButtonPressStarted(object sender, EventArgs e)
    {
        piezo.PlayTone(new Meadow.Units.Frequency(440));
        stopWatch.Reset();
        stopWatch.Start();
        timer.Stop();
    }

    void ButtonPressEnded(object sender, EventArgs e)
    {
        piezo.StopTone();
        stopWatch.Stop();

        if (stopWatch.ElapsedMilliseconds < 200)
        {
            answer += "O";
        }
        else
        {
            answer += "-";
        }

        displayController.UpdateAnswer(answer, Color.White);
        timer.Start();
    }

    void ShowLetterQuestion()
    {
        answer = string.Empty;
        question = morseCode.ElementAt(new Random().Next(0, morseCode.Count)).Value;
        displayController.ShowLetterQuestion(question);
    }

    public override Task Run()
    {
        ShowLetterQuestion();

        return base.Run();
    }
}