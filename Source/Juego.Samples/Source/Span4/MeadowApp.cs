using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;
using System;
using System.Threading.Tasks;

namespace Span4;

public class MeadowApp : App<F7CoreComputeV2>
{
    IJuegoHardware juego;
    Span4Game game;
    MicroGraphics graphics;
    MicroAudio moveAudio;
    MicroAudio effectsAudio;

    public override Task Initialize()
    {
        Console.WriteLine("Initialize...");

        game = new Span4Game();

        juego = Juego.Create();
        juego.Left_LeftButton.Clicked += (s, e) => game.Left();
        juego.Left_RightButton.Clicked += (s, e) => game.Right();
        juego.Left_DownButton.Clicked += (s, e) => game.Down();

        juego.Right_LeftButton.Clicked += (s, e) => game.Left();
        juego.Right_RightButton.Clicked += (s, e) => game.Right();
        juego.Right_DownButton.Clicked += (s, e) => game.Down();

        juego.StartButton.Clicked += (s, e) => game.Start();

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

    void DrawplashScreen()
    {
        graphics.Clear();
        graphics.DrawText(160, 70, "Connect4", Color.Cyan, ScaleFactor.X3, HorizontalAlignment.Center);
        graphics.DrawText(160, 140, "Press Start", Color.Violet, ScaleFactor.X1, HorizontalAlignment.Center);
        graphics.Show();
    }
}