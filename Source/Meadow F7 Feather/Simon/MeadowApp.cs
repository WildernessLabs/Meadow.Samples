using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace Simon
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        int ANIMATION_DELAY = 200;

        bool isAnimating;
        Frequency[] notes;

        SimonGame game;

        PwmLed[] leds;
        PushButton[] buttons; 
        PiezoSpeaker speaker;

        public override Task Initialize() 
        {
            var onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            notes = new Frequency[] 
            { 
                new Frequency(261.63f), 
                new Frequency(329.63f), 
                new Frequency(392),
                new Frequency(523.25f)
            };

            game = new SimonGame();

            leds = new PwmLed[4];
            leds[0] = new PwmLed(Device.Pins.D10, TypicalForwardVoltage.Red);
            leds[1] = new PwmLed(Device.Pins.D09, TypicalForwardVoltage.Green);
            leds[2] = new PwmLed(Device.Pins.D08, TypicalForwardVoltage.Blue);
            leds[3] = new PwmLed(Device.Pins.D07, TypicalForwardVoltage.Yellow);

            buttons = new PushButton[4];
            buttons[0] = new PushButton(Device.Pins.CIPO);
            buttons[0].Clicked += ButtonRedClicked;
            buttons[1] = new PushButton(Device.Pins.D01);
            buttons[1].Clicked += ButtonGreenClicked;
            buttons[2] = new PushButton(Device.Pins.D03);
            buttons[2].Clicked += ButtonBlueClicked;
            buttons[3] = new PushButton(Device.Pins.D04);
            buttons[3].Clicked += ButtonYellowClicked;

            speaker = new PiezoSpeaker(Device.Pins.D12);

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        async void ButtonRedClicked(object sender, EventArgs e)
        {
            await OnButton(0);
        }

        async void ButtonGreenClicked(object sender, EventArgs e)
        {
            await OnButton(1);
        }

        async void ButtonBlueClicked(object sender, EventArgs e)
        {
            await OnButton(2);
        }

        async void ButtonYellowClicked(object sender, EventArgs e)
        {
            await OnButton(3);
        }

        async Task OnButton(int buttonIndex)
        {
            if (isAnimating == false)
            {                
                await TurnOnLED(buttonIndex);
                game.EnterStep(buttonIndex);
            }
        }

        void OnGameStateChanged(object sender, SimonEventArgs e)
        {
            Task.Run(async () => 
            {
                switch (e.GameState)
                {
                    case GameState.Start:
                        break;
                    case GameState.NextLevel:
                        await ShowStartAnimation();
                        await ShowNextLevelAnimation(game.Level);
                        await ShowSequenceAnimation(game.Level);
                        break;
                    case GameState.GameOver:
                        await ShowGameOverAnimation();
                        game.Reset();
                        break;
                    case GameState.Win:
                        await ShowGameWonAnimation();
                        break;
                }
            });
        }

        async Task TurnOnLED(int index, int duration = 400)
        {
            leds[index].IsOn = true;
            await speaker.PlayTone(notes[index], TimeSpan.FromMilliseconds(duration));
            leds[index].IsOn = false;
        }

        void SetAllLEDs(bool isOn)
        {
            leds[0].IsOn = isOn;
            leds[1].IsOn = isOn;
            leds[2].IsOn = isOn;
            leds[3].IsOn = isOn;
        }

        async Task ShowStartAnimation()
        {
            if (isAnimating)
                return;
            isAnimating = true;

            SetAllLEDs(false);
            for (int i = 0; i < 4; i++)
            {
                leds[i].IsOn = true;
                await Task.Delay(ANIMATION_DELAY);
            }
            for (int i = 0; i < 4; i++)
            {
                leds[3 - i].IsOn = false;
                await Task.Delay(ANIMATION_DELAY);
            }

            isAnimating = false;
        }

        async Task ShowNextLevelAnimation(int level)
        {
            if (isAnimating)
                return;
            isAnimating = true;

            SetAllLEDs(false);
            for (int i = 0; i < level; i++)
            {
                await Task.Delay(ANIMATION_DELAY);
                SetAllLEDs(true);
                await Task.Delay(ANIMATION_DELAY * 3);
                SetAllLEDs(false);
            }

            isAnimating = false;
        }

        async Task ShowSequenceAnimation(int level)
        {
            if (isAnimating)
                return;
            isAnimating = true;
            
            var steps = game.GetStepsForLevel();
            SetAllLEDs(false);            
            for (int i = 0; i < level; i++)
            {
                await Task.Delay(200);
                await TurnOnLED(steps[i], 400);
            }

            isAnimating = false;
        }

        async Task ShowGameOverAnimation()
        {
            if (isAnimating)
                return;
            isAnimating = true;

            await speaker.PlayTone(new Frequency(123.47f), TimeSpan.FromMilliseconds(750));

            for (int i = 0; i < 20; i++)
            {
                SetAllLEDs(false);
                await Task.Delay(50);
                SetAllLEDs(true);
                await Task.Delay(50);
            }

            isAnimating = false;
        }

        async Task ShowGameWonAnimation()
        {
            await ShowStartAnimation();
            await ShowStartAnimation();
            await ShowStartAnimation();
            await ShowStartAnimation();
        }

        public override Task Run()
        {
            SetAllLEDs(true);
            game.OnGameStateChanged += OnGameStateChanged;
            game.Reset();

            return base.Run();
        }
    }
}