using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tetraminos;

public class MeadowApp : App<F7CoreComputeV2>
{
    IJuegoHardware juego;
    TetraminosGame game;
    MicroGraphics graphics;
    MicroAudio moveAudio;
    MicroAudio effectsAudio;

    GameState gameState = GameState.Ready;

    enum GameState
    {
        Ready,
        Playing,
        GameOver
    }

    public override Task Initialize()
    {
        Console.WriteLine("Initialize...");

        game = new TetraminosGame();

        juego = Juego.Create();
        juego.Left_UpButton.Clicked += (s, e) => game.Up();
        juego.Left_LeftButton.Clicked += (s, e) => game.Left();
        juego.Left_RightButton.Clicked += (s, e) => game.Right();
        juego.Left_DownButton.Clicked += (s, e) => game.Down();

        juego.Right_UpButton.Clicked += (s, e) => game.Up();
        juego.Right_LeftButton.Clicked += (s, e) => game.Left();
        juego.Right_RightButton.Clicked += (s, e) => game.Right();
        juego.Right_DownButton.Clicked += (s, e) => game.Drop();

        juego.StartButton.Clicked += StartButton_Clicked;

        graphics = new MicroGraphics(juego.Display)
        {
            CurrentFont = new Font12x16(),
        };

        moveAudio = new MicroAudio(juego.LeftSpeaker);
        effectsAudio = new MicroAudio(juego.RightSpeaker);

        game.Init(graphics, moveAudio, effectsAudio);

        Console.WriteLine("Initialize complete");

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Console.WriteLine("Run...");

        juego.BlinkyLed.SetBrightness(1.0f);

        DrawplashScreen();

        return Task.CompletedTask;
    }

    private void StartButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("StartButton_Clicked");

        if (GameState.Ready == gameState)
        {
            gameState = GameState.Playing;
            PlayGame();
        }
        else if (GameState.GameOver == gameState)
        {
            gameState = GameState.Ready;
            DrawplashScreen();
        }
    }

    void UpdateGame()
    {
        game.Update();
    }

    void DrawplashScreen()
    {
        graphics.Clear();
        graphics.DrawText(160, 70, "Tetraminos", Color.Cyan, ScaleFactor.X2, HorizontalAlignment.Center);
        graphics.DrawText(160, 140, "Press Start", Color.Yellow, ScaleFactor.X1, HorizontalAlignment.Center);
        graphics.Show();
    }

    Task PlayGame()
    {
        var t = new Task(() =>
        {
            game.Reset();
            while (true)
            {
                UpdateGame();
                Thread.Sleep(0);
            }
        }, TaskCreationOptions.LongRunning);

        t.Start();

        return t;
    }
}