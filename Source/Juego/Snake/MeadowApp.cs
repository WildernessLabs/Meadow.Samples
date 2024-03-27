using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Snake;

public class MeadowApp : App<F7CoreComputeV2>
{
    IJuegoHardware juego;
    SnakeGame game;
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

        game = new SnakeGame();

        juego = Juego.Create();
        juego.Left_UpButton.Clicked += (s, e) => game.Up();
        juego.Left_LeftButton.Clicked += (s, e) => game.Left();
        juego.Left_RightButton.Clicked += (s, e) => game.Right();
        juego.Left_DownButton.Clicked += (s, e) => game.Down();

        juego.Right_UpButton.Clicked += (s, e) => game.Up();
        juego.Right_LeftButton.Clicked += (s, e) => game.Left();
        juego.Right_RightButton.Clicked += (s, e) => game.Right();
        juego.Right_DownButton.Clicked += (s, e) => game.Down();

        juego.StartButton.Clicked += StartButton_Clicked;
        juego.SelectButton.Clicked += StartButton_Clicked;

        graphics = new MicroGraphics(juego.Display)
        {
            CurrentFont = new Font12x16(),
        };

        moveAudio = new MicroAudio(juego.LeftSpeaker);
        effectsAudio = new MicroAudio(juego.RightSpeaker);

        game.Init(graphics);

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
        if (juego.Left_LeftButton.State == true)
        {
            game.Left();
        }
        else if (juego.Left_RightButton.State == true)
        {
            game.Right();
        }
        else if (juego.Left_UpButton.State == true)
        {
            game.Up();
        }
        else if (juego.Left_DownButton.State == true)
        {
            game.Down();
        }
        else if (juego.SelectButton.State == true)
        {
            //    game.Quit();
        }

        game.Update();
    }

    void DrawplashScreen()
    {
        graphics.Clear();
        graphics.DrawText(160, 70, "Snake", Color.Cyan, ScaleFactor.X3, HorizontalAlignment.Center);
        graphics.DrawText(160, 140, "Press Start", Color.LawnGreen, ScaleFactor.X1, HorizontalAlignment.Center);
        graphics.Show();
    }

    void DrawEndScreen()
    {
        /*
        graphics.Clear();

        if (game.Winner)
        {
            graphics.DrawText(160, 80, "You Win!", FrogItGame.FrogColor, ScaleFactor.X3, HorizontalAlignment.Center);
            graphics.DrawText(160, 140, $"Your time: {game.GameTime:F1}s", FrogItGame.WaterColor, ScaleFactor.X1, HorizontalAlignment.Center);
            graphics.DrawText(160, 160, $"Your died: {game.Deaths} time(s)", FrogItGame.WaterColor, ScaleFactor.X1, HorizontalAlignment.Center);
        }
        else
        {
            graphics.DrawText(160, 80, "Game Over", FrogItGame.FrogColor, ScaleFactor.X3, HorizontalAlignment.Center);
        }

        graphics.Show();
        */
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
            gameState = GameState.GameOver;
            DrawEndScreen();
        }, TaskCreationOptions.LongRunning);

        t.Start();

        return t;
    }
}