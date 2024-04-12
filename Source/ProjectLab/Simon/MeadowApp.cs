using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace Simon;

// Change F7CoreComputeV2 to F7FeatherV2 for ProjectLab v2
public class MeadowApp : App<F7CoreComputeV2>
{
    private int ANIMATION_DELAY = 50;
    private const int DOT_UP = 0;
    private const int DOT_DOWN = 1;
    private const int DOT_LEFT = 2;
    private const int DOT_RIGHT = 3;
    private bool isAnimating;
    private Frequency[] notes;
    private SimonGame game;
    private IRgbPwmLed onboardLed;
    private MicroGraphics graphics;
    private IProjectLabHardware projectLab;
    private IButton[] buttons;

    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        projectLab = ProjectLab.Create();
        Resolver.Log.Info($"Running on ProjectLab Hardware {projectLab.RevisionString}");

        onboardLed = projectLab.RgbLed;
        onboardLed.SetColor(Color.Red);

        notes = new Frequency[]
        {
            new Frequency(261.63f),
            new Frequency(329.63f),
            new Frequency(392),
            new Frequency(523.25f)
        };

        game = new SimonGame();

        graphics = new MicroGraphics(projectLab.Display)
        {
            Stroke = 5
        };
        graphics.Clear();

        buttons = new IButton[4];
        buttons[0] = projectLab.UpButton;
        buttons[0].Clicked += ButtonUpClicked;
        buttons[2] = projectLab.DownButton;
        buttons[2].Clicked += ButtonDownClicked;
        buttons[3] = projectLab.LeftButton;
        buttons[3].Clicked += ButtonLeftClicked;
        buttons[1] = projectLab.RightButton;
        buttons[1].Clicked += ButtonRightClicked;

        onboardLed.SetColor(Color.Green);

        return base.Initialize();
    }

    private async void ButtonUpClicked(object sender, EventArgs e)
    {
        await OnButton(DOT_UP);
    }

    private async void ButtonDownClicked(object sender, EventArgs e)
    {
        await OnButton(DOT_DOWN);
    }

    private async void ButtonLeftClicked(object sender, EventArgs e)
    {
        await OnButton(DOT_LEFT);
    }

    private async void ButtonRightClicked(object sender, EventArgs e)
    {
        await OnButton(DOT_RIGHT);
    }

    private async Task OnButton(int buttonIndex)
    {
        if (isAnimating == false)
        {
            await DrawDotFilled(buttonIndex);
            game.EnterStep(buttonIndex);
        }
    }

    private void OnGameStateChanged(object sender, SimonEventArgs e)
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

    private async Task DrawDotFilled(int index, int duration = 400)
    {
        DrawDot(index, true);
        await projectLab.Speaker.PlayTone(notes[index], TimeSpan.FromMilliseconds(duration));
        DrawDot(index, false);
    }

    private void DrawAllDots(bool isOn)
    {
        DrawDot(DOT_UP, isOn, false);
        DrawDot(DOT_DOWN, isOn, false);
        DrawDot(DOT_LEFT, isOn, false);
        DrawDot(DOT_RIGHT, isOn, false);
        graphics.Show();
    }

    private async Task ShowStartAnimation()
    {
        if (isAnimating)
            return;
        isAnimating = true;

        DrawAllDots(false);
        await Task.Delay(ANIMATION_DELAY);
        DrawAllDots(true);
        await Task.Delay(ANIMATION_DELAY);
        DrawAllDots(false);
        await Task.Delay(ANIMATION_DELAY);
        DrawAllDots(true);
        await Task.Delay(ANIMATION_DELAY);
        DrawAllDots(false);

        isAnimating = false;
    }

    private async Task ShowNextLevelAnimation(int level)
    {
        if (isAnimating)
            return;
        isAnimating = true;

        DrawAllDots(false);
        for (int i = 0; i < level; i++)
        {
            await Task.Delay(ANIMATION_DELAY);
            DrawAllDots(true);
            await Task.Delay(ANIMATION_DELAY * 3);
            DrawAllDots(false);
        }

        isAnimating = false;
    }

    private async Task ShowSequenceAnimation(int level)
    {
        if (isAnimating)
            return;
        isAnimating = true;

        var steps = game.GetStepsForLevel();
        DrawAllDots(false);
        for (int i = 0; i < level; i++)
        {
            await Task.Delay(100);
            await DrawDotFilled(steps[i], 400);
        }

        isAnimating = false;
    }

    private async Task ShowGameOverAnimation()
    {
        if (isAnimating)
            return;
        isAnimating = true;

        //await Task.Delay(750);
        await projectLab.Speaker.PlayTone(new Frequency(123.47f), TimeSpan.FromMilliseconds(750));

        for (int i = 0; i < 20; i++)
        {
            DrawAllDots(false);
            await Task.Delay(50);
            DrawAllDots(true);
            await Task.Delay(50);
        }

        isAnimating = false;
    }

    private async Task ShowGameWonAnimation()
    {
        await ShowStartAnimation();
        await ShowStartAnimation();
        await ShowStartAnimation();
        await ShowStartAnimation();
    }

    private void DrawDot(int index, bool isFilled, bool update = true)
    {
        switch (index)
        {
            case DOT_UP:
                graphics.DrawCircle(graphics.Width / 2, 55, 40, Color.Black, true);
                graphics.DrawCircle(graphics.Width / 2, 55, 40, Color.Red, isFilled);
                break;
            case DOT_DOWN:
                graphics.DrawCircle(graphics.Width / 2, 185, 40, Color.Black, true);
                graphics.DrawCircle(graphics.Width / 2, 185, 40, Color.Yellow, isFilled);
                break;
            case DOT_LEFT:
                graphics.DrawCircle((graphics.Width / 2) - 65, 120, 40, Color.Black, true);
                graphics.DrawCircle((graphics.Width / 2) - 65, 120, 40, Color.Green, isFilled);
                break;
            case DOT_RIGHT:
                graphics.DrawCircle((graphics.Width / 2) + 65, 120, 40, Color.Black, true);
                graphics.DrawCircle((graphics.Width / 2) + 65, 120, 40, Color.Blue, isFilled);
                break;
        }

        if (update)
        {
            graphics.Show();
        }
    }

    public override Task Run()
    {
        DrawAllDots(true);
        game.OnGameStateChanged += OnGameStateChanged;
        game.Reset();

        return base.Run();
    }
}